using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopCartUser.Models;
using ShopCartUser.Models.ViewModels;

namespace ShopCartUser.Controllers
{
    public class BaseController : Controller
    {
        public static string uri = "https://localhost:44336/public/subproduct/";
        private HttpClient ShopCartAPI = new HttpClient();
        public BaseController()
        {
            if (ShopCartAPI.BaseAddress == null)
            {
                ShopCartAPI.BaseAddress = new Uri("https://localhost:44336/api/ShopAPI/");
            }
            MenuData.setmenu();
        }
        public HttpCommonResponse ExecuteGetApi(string apiurl)
        {
            MediaTypeWithQualityHeaderValue ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            ShopCartAPI.DefaultRequestHeaders.Accept.Add(ContentType);
            HttpResponseMessage apiResponce = ShopCartAPI.GetAsync(apiurl).Result;
            string responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            HttpCommonResponse objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);
            return objlist;
        }
        public HttpCommonResponse ExecuteGetApi(string apiurl,int parmId)
        {
            MediaTypeWithQualityHeaderValue ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            ShopCartAPI.DefaultRequestHeaders.Accept.Add(ContentType);
            HttpResponseMessage apiResponce = ShopCartAPI.GetAsync(apiurl+"/"+parmId).Result;
            string responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            HttpCommonResponse objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);
            return objlist;
        }
        public HttpCommonResponse ExecutePostApi(string apiurl,dynamic sendData)
        {

            String newData = JsonConvert.SerializeObject(sendData);
            var sendobj = new StringContent(newData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage apiResponce = ShopCartAPI.PostAsync(apiurl, sendobj).Result;
            string responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            HttpCommonResponse objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);
            return objlist;
        }
        public HttpCommonResponse ExecutePostApi_Auth(string apiurl,dynamic sendData)
        {

            String newData = JsonConvert.SerializeObject(sendData);
            var sendobj = new StringContent(newData, System.Text.Encoding.UTF8, "application/json");
            string AuthToken = Request.Cookies["Token"];

            HttpRequestMessage apireq = new HttpRequestMessage(HttpMethod.Post, apiurl);
            apireq.Headers.TryAddWithoutValidation("Authorization", AuthToken);
            apireq.Content = sendobj;
            HttpResponseMessage apiResponce = ShopCartAPI.SendAsync(apireq).Result;
            HttpStatusCode responsecode = apiResponce.StatusCode;
            HttpStatusCode Unauth = HttpStatusCode.Unauthorized;

            
            string responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            HttpCommonResponse objlist = new HttpCommonResponse();
            objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);
            if (responsecode == Unauth)
            {
                objlist = new HttpCommonResponse();
                objlist.statusCode = Unauth;
            }
            return objlist;
        }
        public HttpCommonResponse ExecuteGetApi_Auth(string apiurl, dynamic sendData)
        {

            String newData = JsonConvert.SerializeObject(sendData);
            var sendobj = new StringContent(newData, System.Text.Encoding.UTF8, "application/json");
            string AuthToken = Request.Cookies["Token"];

            HttpRequestMessage apireq = new HttpRequestMessage(HttpMethod.Get, apiurl);
            apireq.Headers.TryAddWithoutValidation("Authorization", AuthToken);
            apireq.Content = sendobj;
            HttpResponseMessage apiResponce = ShopCartAPI.SendAsync(apireq).Result;
            HttpStatusCode responsecode = apiResponce.StatusCode;
            HttpStatusCode Unauth = HttpStatusCode.Unauthorized;


            string responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            HttpCommonResponse objlist = new HttpCommonResponse();
            objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);
            if (responsecode == Unauth)
            {
                objlist = new HttpCommonResponse();
                objlist.statusCode = Unauth;
            }
            return objlist;
        }


    }
    public class HttpCommonResponse
    {
        public string AuthToken { get; set; }
        public bool? success { get; set; }
        public bool isedit { get; set; }
        public string message { get; set; }
        public DateTime timestamp { get; set; }
        public dynamic data { get; set; }
        //TODO: update to Naming to metaData
        public MetaData metadata { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }
}