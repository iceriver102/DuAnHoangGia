using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.Models
{
    /// <summary>
    /*
     * {
                 "id": 1,
                 "name": "lê hữu duyên",
                 "email": "superadmin@gmail.com",
                 "phone": "10010101",
                 "address": "123/aa",
                 "avatar": "project1.caikho.com/storage/app/public/462_1523459056_acc2.png",
                 "birthday": "2018-04-04",
                 "cmnd": "1010299221",
                 "user_id": 1,
                 "latitude": "106.677883",
                 "longitude": "10.738544",
                 "created_at": "2018-04-04 21:36:08",
                 "updated_at": "2018-04-09 20:29:30"
             }
             */
    /// </summary>
    public class User:ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Birthday { get; set; }
        public string Cmnd { get; set; }
        public string Phone { get; set; }
        public string Pasword { get;  set; }
        public int Is_company { get; set; }
        public string Code { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
