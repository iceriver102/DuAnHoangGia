using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.Models
{
    /*
     * 
     *  "id": 2,
                "name": "Công ty Chứng Khoán HSC",
                "address": "Nguyễn Cư Trinh Quận 1, Hồ Chí Minh, Việt Nam ",
                "avatar": "http://project1.caikho.com/storage/app/public/462_1523459056_acc2.png",
                "master": "Nguyễn Quang Thịnh",
                "description": "",
                "customer_id": 45,
                "latitude": "106.685380",
                "longitude": "10.757788",
                "created_at": "2018-04-04 21:56:50",
                "updated_at": "2018-04-08 11:50:48"
     *
     */
    public class CompanyModel
    {
        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(avatar))
                    return string.Empty;
                if (avatar.StartsWith("http"))
                    return avatar;
                return $"http://{avatar}";
            }
            set { avatar = value; }
        }
        public string Title { get => this.name; set { this.name = value; } }
        public double Latitude { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public int id;
        public string name;
        public string avatar;
        public float Distance { get; set; }
        public float Time { get; set; }
        public string Bonus
        {
            get
            {
                return $"{this.Time} phút ({Distance} km)";
            }
        }
    }
}