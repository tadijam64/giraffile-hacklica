using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Girafile.Models;

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
        public IHttpActionResult PutDocument(Guid id, Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.ID)
            {
                return BadRequest();
            }

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
        public IHttpActionResult PostDocument(Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Document.Add(document);

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

            db.Document.Remove(document);
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
    }
}