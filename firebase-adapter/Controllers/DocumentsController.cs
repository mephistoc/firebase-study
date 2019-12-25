using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace firebase_adapter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IConfiguration _config;
        private FirebaseAuthPayload _firebaseAuthPayload;
        private static string App_Key = "";
        private static string Auth_UserName = "";
        private static string Auth_Password = "";
        private static string ID_Token = "";
        private static string Access_Token = "";
        private static string Refresh_Token = "";
        private static string AuthUrl = "";
        private static string GetDocumentsUrl = "";
        private static string AuthPayloadJson = "{'email':'{0}','password':'{1}','returnSecureToken': true}";

        private static bool IsTokenTimeout = true;

        [HttpGet]
        public ContentResult Get()
        {
            var firebaseAuthResult = JsonConvert.DeserializeObject<FirebaseAuthResult>(GetAccessToken());
            JObject documents = JObject.Parse( GetDocuments(firebaseAuthResult.IdToken));
            List<JObject> returnObject = new List<JObject>();
            foreach(var j in documents){
                string name = j.Key;
                JToken token = j.Value;
                returnObject.Add(JObject.Parse(token.ToString()));
            }
            return Content(JsonConvert.SerializeObject(returnObject), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }

        [HttpGet("{document_name}")]
        public object Get(string document_name)
        {
            return null;
        }
        public DocumentsController(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _firebaseAuthPayload = new FirebaseAuthPayload()
            {
                Email = config["FirebaseCredential:AuthUsername"],
                Password = config["FirebaseCredential:AuthPassword"],
                ReturnSecureToken = true
            };
            App_Key = config["FirebaseCredential:AppKey"];
            AuthUrl = config["FirebaseCredential:AuthUrl"];
            GetDocumentsUrl = config["FirebaseCredential:GetDocumentsUrl"];
        }

        public string GetDocuments(string id_token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(GetDocumentsUrl, id_token));
            string result = "";

            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                try
                {
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    result = response.Content.ReadAsStringAsync().Result;
                }
                catch (HttpRequestException e)
                {
                    result = e.Message;
                }
            }
            return result;
        }

        public string GetAccessToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, string.Format(AuthUrl, App_Key));
            HttpContent content = new StringContent(JsonConvert.SerializeObject(_firebaseAuthPayload), Encoding.UTF8, "application/json");
            // HttpContent content = new StringContent(string.Format(AuthPayloadJson, Auth_UserName, Auth_Password));
            request.Content = content;

            string resutl = "";
            // var client = _httpClientFactory.CreateClient();
            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                try
                {
                    var response = client.SendAsync(request).Result;

                    response.EnsureSuccessStatusCode();
                    resutl = response.Content.ReadAsStringAsync().Result;

                }
                catch (HttpRequestException e)
                {
                    resutl = e.Message;
                }
            }
            return resutl;
        }
    }
}