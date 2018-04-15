using DuAnHoangGia.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DuAnHoangGia.Sevices
{
    public interface IHttpSevices
    {
         NewsModel News { get; set; }
        User User { get; set; }
        Task<(HttpStatusCode status, string msg, JObject data)> LoginAsync(string username, string password, string sContentType = "application/json");
    
        Task<JObject> GetCompanysAsync(int page=1);
        Task<JObject> GetCompanyAsync(int com);
        Task<JObject> GetHelpsAsync(int page=1);
        Task<JObject> GetNotifisAsync(int page=1);
        Task<JObject> GetNewsAsync(int page=1);
        Task<JObject> GetNewAsync(int id);
        Task<JObject> GetUser();
        Task<JObject> PostHelpAsync(string title, string command);
        Task<(JObject data, bool result)> RegisterAsync(User u);
        Task<(JObject data, bool result)> UpdateAsync(User u);
    }
}
