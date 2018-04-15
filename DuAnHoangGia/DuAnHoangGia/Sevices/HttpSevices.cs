using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DuAnHoangGia.Models;
using Newtonsoft.Json.Linq;

namespace DuAnHoangGia.Sevices
{
    public class HttpSevices : IHttpSevices
    {

        public const string url = "http://project1.caikho.com/api";

        public string url_login { get => $"{url}/user/login"; }
        public User User { get; set; }
        public NewsModel News { get; set; }

        public async Task<JObject> GetCompanysAsync(int page = 1)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/company/all?page={page}");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return result["data"] as JObject;
                return null;

            }
        }

        public async Task<JObject> GetCompanyAsync(int com)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/company/{com}/");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return result["data"] as JObject;
                return null;

            }
        }

        public async Task<(HttpStatusCode status, string msg, JObject data)> LoginAsync(string username, string password, string sContentType = "application/json")
        {
            JObject oJsonObject = new JObject();
            oJsonObject.Add("user", username);
            oJsonObject.Add("password", password);
            using (HttpClient oHttpClient = new HttpClient())
            {
                var oHttpResponseMessage = await oHttpClient.PostAsync(url_login, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (oHttpResponseMessage.IsSuccessStatusCode)
                    return (status: oHttpResponseMessage.StatusCode, msg: result["message"].ToString(), data: result["data"].ToObject<JObject>());
                return (status: oHttpResponseMessage.StatusCode, msg: result["message"].ToString(), data: null);
            }
        }

        public async Task<JObject> PostHelpAsync(string title, string command)
        {
            //api/help/?
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("title", title),
                    new KeyValuePair<string, string>("description", command)
                });
                var oHttpResponseMessage = await oHttpClient.PostAsync($"{url}/help/", content);
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return result["data"] as JObject;
                return null;
            }
        }

        public async Task<JObject> GetHelpsAsync(int page = 1)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                //http://project1.caikho.com/api/help/all?page=1
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/help/all?page={page}");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return result["data"] as JObject;
                return null;
            }
        }

        public async Task<JObject> GetNotifisAsync(int page = 1)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                //http://project1.caikho.com/api/help/all?page=1
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/notification/?page={page}");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return result["data"] as JObject;
                return null;
            }
        }

        public async Task<JObject> GetNewsAsync(int page = 1)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                //http://project1.caikho.com/api/news/all?page=1
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/news/all/?page={page}");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return result["data"] as JObject;
                return null;
            }
        }

        public async Task<JObject> GetNewAsync(int id)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                //http://project1.caikho.com/api/news/16
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/news/{id}");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return result["data"] as JObject;
                return null;
            }
        }

        public async Task<(JObject data, bool result)> RegisterAsync(User u)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("name", u.Name),
                    new KeyValuePair<string, string>("email", u.Email),
                    new KeyValuePair<string, string>("phone", u.Phone),
                    new KeyValuePair<string, string>("password", u.Pasword),
                });
                var oHttpResponseMessage = await oHttpClient.PostAsync($"{url}/user/signup", content);
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return (result["data"] as JObject, oHttpResponseMessage.IsSuccessStatusCode);
                return (null, oHttpResponseMessage.IsSuccessStatusCode);
            }
        }

        public async Task<(JObject data, bool result)> UpdateAsync(User u)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                List<KeyValuePair<string, string>> forms = new List<KeyValuePair<string, string>>(new[]
                {
                    new KeyValuePair<string, string>("name", u.Name),
                    new KeyValuePair<string, string>("email", u.Email),
                    new KeyValuePair<string, string>("phone", u.Phone),
                    new KeyValuePair<string, string>("address", u.Address),
                    new KeyValuePair<string, string>("cmnd", u.Cmnd),
                });

                if (!string.IsNullOrEmpty(u.Pasword))
                {
                    forms.Add(new KeyValuePair<string, string>("password", u.Pasword));
                }
                var oHttpResponseMessage = await oHttpClient.PutAsync($"{url}/customer/update/", new FormUrlEncodedContent(forms));
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return (result["data"] as JObject,oHttpResponseMessage.IsSuccessStatusCode);
                return (null,oHttpResponseMessage.IsSuccessStatusCode);
            }
        }

        public async Task<JObject> GetUser()
        {
            //api/user
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                //http://project1.caikho.com/api/user
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/user");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return result["data"] as JObject;
                return null;
            }
        }
    }
}