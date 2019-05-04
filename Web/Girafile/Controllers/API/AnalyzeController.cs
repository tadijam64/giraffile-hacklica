using Girafile.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Girafile.Controllers.API
{
    public class AnalyzeController : ApiController
    {
        private GiraffileEntities db = new GiraffileEntities();

        public bool PostDocument(DocumentDTO document)
        {
            Document temp = db.Document.Find(document.ID);

            if(temp != null)
            {
                temp.Keywords = document.Keywords;
                temp.MetaData = document.Metadata;
                temp.IDLanguge = (db.Language.Where(l => l.Name.Contains(document.Language)).FirstOrDefault().ID);

                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
