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

        public async Task<JObject> GetCompanysAsync(int page = 1, int nums = 10)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/company/all?page={page}&offset={nums}");
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

        public async Task<JObject> GetHelpsAsync(int page = 1, int nums = 10)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/help/all?page={page}&offset={nums}");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return result["data"] as JObject;
                return null;
            }
        }

        public async Task<JObject> GetNotifisAsync(int page = 1, int nums = 10)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/notification/?page={page}&offset={nums}");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"].HasValues)
                    return result["data"] as JObject;
                return null;
            }
        }

        public async Task<JObject> GetNewsAsync(int page = 1, int nums = 10)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                //http://project1.caikho.com/api/news/all?page=1
                var oHttpResponseMessage = await oHttpClient.GetAsync($"{url}/news/all/?page={page}&offset={nums}");
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
                    return (result["data"] as JObject, oHttpResponseMessage.IsSuccessStatusCode);
                return (null, oHttpResponseMessage.IsSuccessStatusCode);
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

        public async Task<(JArray data, bool result)> GetCompanysOnMapAsync(double lat, double log)
        {
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                List<KeyValuePair<string, string>> forms = new List<KeyValuePair<string, string>>(new[]
                {
                    new KeyValuePair<string, string>("latitude",lat.ToString()),
                    new KeyValuePair<string, string>("longitude",log.ToString())
                });
                //http://project1.caikho.com/api/company/toado
                var oHttpResponseMessage = await oHttpClient.PostAsync($"{url}/company/toado", new FormUrlEncodedContent(forms));// &lat={lat}&long={log}");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(Content);
                if (result["data"] is JArray j)
                    return (j, oHttpResponseMessage.IsSuccessStatusCode);
                return (null, oHttpResponseMessage.IsSuccessStatusCode);

            }
        }
        public async Task<(JObject data, bool result)> GetCompanysByNameAsync(string key)
        {
            //return Task.FromResult<(JObject data,bool result)>((JObject.Parse("{'id': 18, 'name': 'Chi Nhánh Công Ty Tnhh Dv Bảo Vệ Thái Long',            'address': '54 Đường 218 Cao Lỗ Phường 4 Quận 8, Hồ Chí Minh, Việt Nam',            'avatar': 'http://project1.caikho.com/storage/app/public/860_1523628674_22554926_129460434477352_8464463052739651062_n.jpg',            'master': 'Nguyễn Quang Thịnh',            'description': 'Hiện nay, Vietlott chỉ ủy quyền cung cấp dịch vụ thông báo kết quả QSMT qua kênh SMS bằng các đầu số: 9141, 9939, 9911, 8179, 8130, 997, 8193. Vietlott không chịu trách nhiệm về tính chính xác đối với kết quả QSMT do các đầu số ngoài danh sách trên cung cấp.\r\n\r\nThời hạn lĩnh thưởng của vé trúng thưởng: là 60 (sáu mươi) ngày, kể từ ngày xác định kết quả trúng thưởng. Quá thời hạn trên, các vé trúng thưởng không còn giá trị lĩnh thưởng.',            'customer_id': 1,            'latitude': '10.736600',            'longitude': '106.679480',            'created_at': '2018-04-04 22:01:13',            'updated_at': '2018-04-06 21:17:29','distance': 4.6104122271616577 }"), true));
            using (HttpClient oHttpClient = new HttpClient())
            {
                oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Current.Token);
                List<KeyValuePair<string, string>> forms = new List<KeyValuePair<string, string>>(new[]
                {
                    new KeyValuePair<string, string>("search",key),
                });
                //http://project1.caikho.com/api/company/toado
                var oHttpResponseMessage = await oHttpClient.PostAsync($"{url}/company/search", new FormUrlEncodedContent(forms));// &lat={lat}&long={log}");
                string Content = await oHttpResponseMessage.Content.ReadAsStringAsync();
                try
                {
                    JArray result = JArray.Parse(Content);
                    if (result is JArray j && j.Count > 0)
                        return (j[0] as JObject, oHttpResponseMessage.IsSuccessStatusCode);
                }
                catch
                {

                }
                return (null, oHttpResponseMessage.IsSuccessStatusCode);

            }
        }

        //http://project1.caikho.com/api/company/search
    }
}