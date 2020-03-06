using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyQuanCafe.Models
{
    public class TableData
    {
        public IEnumerable<TableFood> tableFoods { get; set; }
        public IEnumerable<Bill> bills { get; set; }
        public IEnumerable<BillInfo> billInfors { get; set; }
        public IEnumerable<Food> foods { get; set; }
       
    }
}

