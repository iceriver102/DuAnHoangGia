using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.Models
{
    /*
     * 
     *   {
                "id": 16,
                "title": "Giảm giá sốc",
                "description_short": "Áo khoác giảm 50 %",
                "description_long": "<p>SALE OFF 70%<img alt='' src='http://project1.caikho.com/storage/app/public/793_1522948330_case.png' /></p>\r\n",
                "image": "",
                "status": 1,
                "category_id": 2,
                "created_at": "2018-04-06 00:11:07",
                "updated_at": "2018-04-09 21:55:16"
            }, 
     * 
     */
    public class NewsModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description_short { get; set; }
        public string Description_long { get; set; }
        public string Created_at;
        public string Date { get { return Convert.ToDateTime(this.Created_at).ToString("dd/MM/yyyy"); } }

    }
}
