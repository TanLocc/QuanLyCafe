using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyQuanCafe.Models
{
    public class FoodData
    {
        public int id { get; set; }
        public string name { get; set; }
        public byte[] image { get; set; }
        public String Category { get; set; }
        public int count { get; set; }
        public double price { get; set; }
       
    }
}