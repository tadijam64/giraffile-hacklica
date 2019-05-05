using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Girafile.Models
{
    public class DocumentDTO2
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> IDLanguge { get; set; }
        public string Name { get; set; }
        public string MetaData { get; set; }
        public string Keywords { get; set; }
        public string MD5 { get; set; }
        public Nullable<bool> Deleted { get; set; }
    }
}