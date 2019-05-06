using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Girafile.Common;
using Girafile.Models;
using Newtonsoft.Json;
using Action = Girafile.Models.Action;

namespace Girafile.Controllers.API
{
    public class DocumentsController : ApiController
    {
        private GiraffileEntities db = new GiraffileEntities();

        // GET: api/Documents
        public List<DocumentDTO2> GetDocument(string query)
        {
            Guid temp = new Guid("e674aeac-6d25-4e3c-a54e-d889419f79a5"); //Guid.NewGuid();

            string basePath = HttpContext.Current.Server.MapPath("~") + @"\Query\";

            File.WriteAllText(basePath + temp +".txt", query);

            while(!File.Exists(basePath + @"\Response\" + temp + ".txt"))
            {
                Thread.Sleep(300);
            }

            string keywordsTemp = File.ReadAllText(basePath + @"\Response\" + temp + ".txt");
            File.Delete(basePath + temp + ".txt");
            //File.Delete(basePath + @"\Response\" + temp + ".txt");


            //TODO: Search 
            List<Document> documents = db.Document.ToList();
            List<Document> ret = new List<Document>();

            SearchDTO searchDTO = JsonConvert.DeserializeObject<SearchDTO>(keywordsTemp);



           
            
            foreach(Document doc in documents)
            {
                int brojPogodaka = 0;
                if (doc.Language != null && searchDTO.language != null)
                {
                    if(doc.Language.Name.Contains(searchDTO.language))
                    {
                        brojPogodaka += 100;
                    }    
                }

                bool dateS = false;
                if(searchDTO.dateFrom != null && searchDTO.dateTo == null)
                {
                    searchDTO.dateTo = DateTime.MaxValue.ToString("yyyy-MM-dd");
                    dateS = true;
                }
                else if (searchDTO.dateFrom == null && searchDTO.dateTo != null)
                {
                    searchDTO.dateFrom = DateTime.MinValue.ToString("yyyy-MM-dd");
                    dateS = true;
                }
                else if (searchDTO.dateFrom != null && searchDTO.dateTo != null)
                {
                    dateS = true;
                }

                if(dateS)
                {
                    DateTime to = DateTime.Parse(searchDTO.dateTo);
                    DateTime from = DateTime.Parse(searchDTO.dateFrom);

                    if(doc.History.Where(l => l.Date >= from && l.Date <= to && l.Action.Name == "Add").Count() > 0)
                    {
                        brojPogodaka += 50;
                    }
                }

                if (doc.Keywords != null)
                {
                    string[] keys = doc.Keywords.Split(';');
                    
                    foreach (string key in keys)
                    {
                        if (searchDTO.Keywords.Contains(key))
                        {
                            brojPogodaka++;
                        }
                    }
                }
                doc.MD5 = brojPogodaka.ToString();
                if (brojPogodaka > 0)
                {
                    doc.Language = null;
                    doc.History = null;
                    
                    ret.Add(doc);
                }
            }

            

            List<DocumentDTO2> lista = Mapper.Map<List<DocumentDTO2>>(ret.OrderByDescending(l => int.Parse(l.MD5)).ToList());
            return lista;
        }

        // GET: api/Documents/5
        [ResponseType(typeof(Document))]
        public IHttpActionResult GetDocument(Guid id)
        {
            Document document = db.Document.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        // PUT: api/Documents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDocument(Guid id)
        {
            
            if (!ModelState.IsValid && HttpContext.Current.Request.Files.Count != 1)
            {
                return BadRequest(ModelState);
            }
            Document document = db.Document.Find(id);
            if (document == null)
            {
                return BadRequest();
            }

            int number = db.History.Where(l => l.IDAction != db.Action.Where(a => a.Name.Contains("Delete")).First().ID).Count();
            string path = "";
            try
            {

                 path = HttpContext.Current.Server.MapPath("~") + "/Files/";
                string pathOld = document.ID + "-" + number + Path.GetExtension(document.Name);
                path += document.ID + Path.GetExtension(document.Name);

                System.IO.File.Move(path, pathOld);
                HttpContext.Current.Request.Files[0].SaveAs(path);
                document.MD5 = checkMD5(path);
            }
            catch
            {

            }

            //var proc1 = new ProcessStartInfo();
            //proc1.UseShellExecute = true;
            //proc1.WorkingDirectory = @"C:\Windows\System32";
            //proc1.FileName = @"cmd.exe";
            //proc1.Arguments = "/k " + @"python C:\Users\zirafa\Desktop\Document Analysis\analyze.py " + path + " " + document.ID;

            var proc2 = new ProcessStartInfo();
            proc2.UseShellExecute = true;
            proc2.WorkingDirectory = @"C:\Windows\System32";
            proc2.FileName = @"cmd.exe";
            proc2.Arguments = "/k " + @"mkdir C:\Leo " + path + " " + document.ID;

            //Process.Start(proc1);
            Process.Start(proc2);
            

            db.Entry(document).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        /*
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
        }*/

        // POST: api/Documents
        [ResponseType(typeof(Document))]
        public IHttpActionResult PostDocument()
        {
            Document document = new Document();
            if (!ModelState.IsValid || HttpContext.Current.Request.Files.Count != 1)
            {
                return BadRequest(ModelState);
            }

            document.ID = Guid.NewGuid();
            document.Name = HttpContext.Current.Request.Files[0].FileName;
            string path = "";
            try
            {
                 path = HttpContext.Current.Server.MapPath("~") + "Files\\"; 
                path += document.ID + Path.GetExtension(document.Name);
                HttpContext.Current.Request.Files[0].SaveAs(path);
                document.MD5 = checkMD5(path);
            }
            catch
            {

            }

            db.Document.Add(document);

            Action akcija = db.Action.Where(l => l.Name.Contains("Add")).FirstOrDefault();

            History history = new History();
            history.ID = Guid.NewGuid();
            history.IDAction = akcija.ID;
            history.IDDocument = document.ID;
            history.Date = DateTime.Now;

            db.History.Add(history);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DocumentExists(document.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

         
            return CreatedAtRoute("DefaultApi", new { id = document.ID }, document);
        }

        // DELETE: api/Documents/5
        [ResponseType(typeof(Document))]
        public IHttpActionResult DeleteDocument(Guid id)
        {
            Document document = db.Document.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            document.Deleted = true;
            db.SaveChanges();

            return Ok(document);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DocumentExists(Guid id)
        {
            return db.Document.Count(e => e.ID == id) > 0;
        }

        public string checkMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }
    }
}