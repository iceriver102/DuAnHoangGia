using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.Models
{


    /*
     * 
     *   "id": 10,
                "customer_id": null,
                "company_id": 2,
                "title": "tiêu đề",
                "description": "nội dung",
                "status": 2,
                "created_at": "2018-04-03 23:59:29",
                "updated_at": "2018-04-03 23:59:29"
     * 
     * 
     */
    public class HelpModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string created_at;
        public string Date { get { return Convert.ToDateTime(this.created_at).ToString("dd/MM/yyyy"); } }
        public string Thumbnail { get; set; }
    }
}