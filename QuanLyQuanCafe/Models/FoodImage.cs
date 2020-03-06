using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyQuanCafe.Models
{
    public class FoodImage
    {
        public int id { get; set; }
        public string name { get; set; }
        public HttpPostedFileBase image { get; set; }
        public int idCategory { get; set; }
        public double price { get; set; }
    }
}