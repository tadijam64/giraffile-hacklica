using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Girafile.Models
{
    public class DocumentDTO
    {
        public Guid ID { get; set; }
        public string Language { get; set; }
        public string Keywords { get; set; }
        public List<Metadata> Metadata { get; set; }
    }
}