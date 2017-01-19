using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CWMD;
using CWMD.Models;
using CWMD.Services;
using CWMD.Utils;

namespace CWMD.Controllers
{
    [RoutePrefix("api/docs")]
    public class DocumentsController : BaseApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("documents")]
        public IHttpActionResult GetDocuments()
        {
            return Ok(db.Documents);
        }

        [Route("documents/{username}")]
        [ResponseType(typeof(List<DocumentVersion>))]
        public async Task<IHttpActionResult> GetDocumentsForUser(String username)
        {
            List<Document> documents = db.Documents.Where(d => d.AuthorUserName == username).ToList();
            if (!documents.Any())
            {
                return NotFound();
            }

            return Ok(documents);
        }

        public Document GetDocumentByName(String name)
        {
            List<Document> documents = db.Documents.Where(d => d.FileName == name).ToList();
            if (!documents.Any())
            {
                return null;
            }

            return documents[0];
        }

        // GET: api/Documents/5
        [ResponseType(typeof(Document))]
        public async Task<IHttpActionResult> GetDocument(int id)
        {
            Document document = await db.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        // GET: api/Documents/download/1
        [ResponseType(typeof(Document))]
        [Route("download/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> DownloadDocument(int id)
        {
            var fileInfo = new FileInfo(StringConstants.FolderPath + "\\altfish5.doc");
            return !fileInfo.Exists
                ? (IHttpActionResult)NotFound()
                : new FileResult(fileInfo.FullName);
        }

        // PUT: api/Documents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDocument(int id, Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.Id)
            {
                return BadRequest();
            }

            db.Entry(document).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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
        public async Task<IHttpActionResult> PostDocument(Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Documents.Add(document);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = document.Id }, document);
        }

        // DELETE: api/Documents/5
        [ResponseType(typeof(Document))]
        public async Task<IHttpActionResult> DeleteDocument(int id)
        {
            Document document = await db.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            db.Documents.Remove(document);
            await db.SaveChangesAsync();

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

        private bool DocumentExists(int id)
        {
            return db.Documents.Count(e => e.Id == id) > 0;
        }
    }
}