using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Girafile.Models;
using Action = Girafile.Models.Action;

namespace Girafile.Controllers.API
{
    public class DocumentsController : ApiController
    {
        private GiraffileEntities db = new GiraffileEntities();

        // GET: api/Documents
        public IQueryable<Document> GetDocument()
        {
            return db.Document;
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
            try
            {

                string path = HttpContext.Current.Server.MapPath("~") + "/Files/";
                string pathOld = document.ID + "-" + number + Path.GetExtension(document.Name);
                path += document.ID + Path.GetExtension(document.Name);

                System.IO.File.Move(path, pathOld);
                HttpContext.Current.Request.Files[0].SaveAs(path);
                document.MD5 = checkMD5(path);
            }
            catch
            {

            }

            //TODO: logika za dohvacanje jezika
            //TODO: metadata
            //TODO: keywords

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

            try
            {
                string path = HttpContext.Current.Server.MapPath("~") + "/Files/"; 
                path += document.ID + Path.GetExtension(document.Name);
                HttpContext.Current.Request.Files[0].SaveAs(path);
                document.MD5 = checkMD5(path);
            }
            catch
            {

            }

            //TODO: logika za dohvacanje jezika
            //TODO: metadata
            //TODO: keywords
            
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