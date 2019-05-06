using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Girafile.Models
{
    public class SearchDTO
    {
        public string intent { get; set; }
        public string response { get; set; }
        public string dateRangeType { get; set; }
        public string language { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public List<string> Keywords { get; set; }
    }
}