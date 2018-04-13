using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.Models
{
    /*
     * 
     * "id": 40,
                "name": "tên thông báo 3",
                "description": "<p>TH&ocirc;ng b&aacute;o 3</p>\n",
                "type": 3,
                "status": 1,
                "created_at": "2018-04-01 20:56:22",
                "updated_at": "2018-04-01 20:56:34"
     * 
     */
    public class NotificationModel
    {
        public string Thumbnail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string created_at;
        public string Date { get { return Convert.ToDateTime(created_at).ToString("dd/MM/yyyy"); } }

    }
}
