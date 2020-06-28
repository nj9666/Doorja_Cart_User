using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopCartUser.Models;
using ShopCartUser.Models.ViewModels;
using paytm;
using Microsoft.AspNetCore.Http;

namespace ShopCartUser.Controllers
{
    public class CheckoutController : BaseController
    {
        public IActionResult Index()
        {
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                HttpCommonResponse ResData = ExecuteGetApi_Auth("Cart/GetAll", null);
                CheckOut CO = new CheckOut();
                if (ResData.statusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Index", "Login", new { msg = "Plz Re-Login, Your Last Login is one day ago" });
                }
                if (ResData.success == true)
                {
                    CO.FinalProductList = JsonConvert.DeserializeObject<List<CartTbl>>(JsonConvert.SerializeObject(ResData.data));

                    HttpCommonResponse ResData1 = ExecuteGetApi_Auth("Address/GetAll/My", null);
                    if (ResData1.success == true)
                    {
                        CO.useraddresses = JsonConvert.DeserializeObject<List<AddressTbl>>(JsonConvert.SerializeObject(ResData1.data));
                    }
                    else
                    {
                        CO.useraddresses = new List<AddressTbl>();
                    }

                    UserMstr logUser = JsonConvert.DeserializeObject<UserMstr>(Request.Cookies["User"]);
                    CO.userdt = logUser;
                    return View(CO);
                }
                else
                {
                    return this.RedirectToAction("Error", "Home");
                }

            }
            else
            {
                return RedirectToAction("Index", "Login", new { msg = "Plz Login First" }); ;
            }
            return View();
        }

        [HttpPost]
        public void CheckOut(CheckOut model)
        {
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                var cpCode = "";

                if (model.cpCode != null)
                {
                    cpCode = model.cpCode;
                }

                HttpCommonResponse ResData = ExecutePostApi_Auth("Order/Place/" + model.addId + "/" + cpCode, null);
                if (ResData.success == true)
                {
                    OrderTbl orderss = JsonConvert.DeserializeObject<OrderTbl>(JsonConvert.SerializeObject(ResData.data));
                    var totprice = decimal.Round(decimal.Add(orderss.TotalPrice, 100), 2, MidpointRounding.AwayFromZero);
                    PayPament(orderss.OrderIdV.ToString(), (totprice).ToString());
                    //PayPament(orderss.OrderIdV.ToString(), (orderss.TotalPrice + 100).ToString());

                }
                else
                {
                    var msg = "Qty Change NOt done plz Refresh this page and try again";
                }
            }
            else
            {
                var msg = "You Need to login first";
            }
        }

        [HttpPost]
        public void RepayPaytm(PayResponse model)
        {
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                    PayPament(model.ORDERID.ToString(), model.TXNAMOUNT.ToString());
                
            }
        }



        public void PayPament(string ORDER_ID, string TXN_AMOUNT)
        {
            UserMstr logUser = JsonConvert.DeserializeObject<UserMstr>(Request.Cookies["User"]);


            String merchantKey = "1HWtKdos1cqabDDU";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("MID", "ZXHwhY57939059221556");
            parameters.Add("CHANNEL_ID", "WEB");
            parameters.Add("INDUSTRY_TYPE_ID", "Retail");
            parameters.Add("WEBSITE", "WEBSTAGING");
            parameters.Add("EMAIL", logUser.Email);
            parameters.Add("MOBILE_NO", Convert.ToInt64(logUser.ContactNumber).ToString());
            parameters.Add("CUST_ID", Request.Cookies["UserId"]);
            parameters.Add("ORDER_ID", ORDER_ID);
            parameters.Add("TXN_AMOUNT", TXN_AMOUNT);
            parameters.Add("CALLBACK_URL", "https://localhost:44363/Checkout/OrderSuccess"); //This parameter is not mandatory. Use this to pass the callback url dynamically.

            string checksum = CheckSum.generateCheckSum(merchantKey, parameters);
            string paytmURL = "https://securegw-stage.paytm.in/theia/processTransaction?orderid=" + ORDER_ID;

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            Response.WriteAsync(outputHTML);
        }
        public IActionResult OrderSuccess()
        {
            PayResponse pyres = new PayResponse();

            String merchantKey = "1HWtKdos1cqabDDU"; // Replace the with the Merchant Key provided by Paytm at the time of registration.

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string paytmChecksum = "";
            foreach (string key in Request.Form.Keys)
            {
                parameters.Add(key.Trim(), Request.Form[key].ToString().Trim());
            }

            if (parameters.ContainsKey("CHECKSUMHASH"))
            {
                paytmChecksum = parameters["CHECKSUMHASH"];
                parameters.Remove("CHECKSUMHASH");
            }

            if (CheckSum.verifyCheckSum(merchantKey, parameters, paytmChecksum))
            {
                string paytmStatus = parameters["STATUS"];
                pyres.ORDERID = parameters["ORDERID"];
                pyres.TXNAMOUNT = parameters["TXNAMOUNT"];
                pyres.RESPMSG = parameters["RESPMSG"];
                pyres.STATUS = parameters["STATUS"];

                HttpCommonResponse ResData = ExecuteGetApi_Auth("Order/Get/" + pyres.ORDERID, null);
                if (ResData.success == true)
                {
                    pyres.order = JsonConvert.DeserializeObject<OrderTbl>(JsonConvert.SerializeObject(ResData.data));
                    pyres.userdt = pyres.order.User;
                }
                else
                {
                    return this.RedirectToAction("Error", "Home");
                }


                if (paytmStatus == "TXN_SUCCESS")
                {
                    if (parameters["RESPCODE"] == "01")
                    {
                        pyres.RESPMSG = "Payment Sucessfully Done";
                    }

                    pyres.TXNID = parameters["TXNID"];

                    if (parameters["PAYMENTMODE"] == "CC")
                    {
                        pyres.PAYMENTMODE = "Credit card";
                    }
                    else if (parameters["PAYMENTMODE"] == "DC")
                    {
                        pyres.PAYMENTMODE = "Debit card";
                    }
                    else if (parameters["PAYMENTMODE"] == "NB")
                    {
                        pyres.PAYMENTMODE = "Net banking";
                    }
                    else if (parameters["PAYMENTMODE"] == "UPI")
                    {
                        pyres.PAYMENTMODE = "UPI";
                    }
                    else if (parameters["PAYMENTMODE"] == "PPI")
                    {
                        pyres.PAYMENTMODE = "Paytm wallet";
                    }
                    else if (parameters["PAYMENTMODE"] == "PAYTMCC")
                    {
                        pyres.PAYMENTMODE = "Postpaid";
                    }

                    PaymentTbl payobj = new PaymentTbl();
                    payobj.OrderId = pyres.order.Id;
                    payobj.RespMsg = pyres.RESPMSG;
                    payobj.Status = 1;
                    payobj.TransectionId = pyres.TXNID;
                    payobj.Type = parameters["PAYMENTMODE"];

                    HttpCommonResponse ResDatapay = ExecutePostApi_Auth("Order/payment" , payobj);
                    if (ResDatapay.success == true)
                    {
                        
                    }
                    else
                    {
                        return this.RedirectToAction("Error", "Home");
                    }
                }
                else if (paytmStatus == "PENDING")
                {
                    PaymentTbl payobj = new PaymentTbl();
                    payobj.OrderId = pyres.order.Id;
                    payobj.RespMsg = pyres.RESPMSG;
                    payobj.Status = 2;
                    payobj.TransectionId = pyres.TXNID;
                    payobj.Type = parameters["PAYMENTMODE"];

                    HttpCommonResponse ResDatapay = ExecutePostApi_Auth("Order/payment", payobj);
                    if (ResDatapay.success == true)
                    {

                    }
                    else
                    {
                        return this.RedirectToAction("Error", "Home");
                    }
                }
                else if (paytmStatus == "TXN_FAILURE")
                {
                    PaymentTbl payobj = new PaymentTbl();
                    payobj.OrderId = pyres.order.Id;
                    payobj.RespMsg = pyres.RESPMSG;
                    payobj.Status = 3;
                    payobj.TransectionId = pyres.TXNID;
                    payobj.Type = parameters["PAYMENTMODE"];

                    HttpCommonResponse ResDatapay = ExecutePostApi_Auth("Order/payment", payobj);
                    if (ResDatapay.success == true)
                    {

                    }
                    else
                    {
                        return this.RedirectToAction("Error", "Home");
                    }
                }
            }
            else
            {
                return this.RedirectToAction("Error", "Home");
            }

            return View(pyres);
        }
    }
}