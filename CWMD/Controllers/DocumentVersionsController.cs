using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CWMD;
using CWMD.Models;

namespace CWMD.Controllers
{
    [RoutePrefix("api/ver")]
    public class DocumentVersionsController : BaseApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DocumentVersions
        [Route("versions")]
        public IHttpActionResult GetDocumentVersions()
        {
            return Ok(db.DocumentVersions.ToList().Select(u => this.TheModelFactory.Create(u)));
            //return Ok(db.DocumentVersions);
        }

        [Route("versionsForDoc/{id}")]
        [ResponseType(typeof(List<DocumentVersion>))]
        public async Task<IHttpActionResult> GetVersionsForDocument(int id)
        {
            List<DocumentVersion> documentVersions = db.DocumentVersions.Where(b => b.DocumentId ==id).ToList();
            if (documentVersions.Count==0)
            {
                return NotFound();
            }

            return Ok(documentVersions);
        }

        // GET: api/DocumentVersions/5
        [ResponseType(typeof(DocumentVersion))]
        public async Task<IHttpActionResult> GetDocumentVersion(int id)
        {
            DocumentVersion documentVersion = await db.DocumentVersions.FindAsync(id);
            if (documentVersion == null)
            {
                return NotFound();
            }

            return Ok(documentVersion);
        }

        // PUT: api/DocumentVersions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDocumentVersion(int id, DocumentVersion documentVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != documentVersion.Id)
            {
                return BadRequest();
            }

            db.Entry(documentVersion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentVersionExists(id))
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

        // POST: api/DocumentVersions
        [ResponseType(typeof(DocumentVersion))]
        public async Task<IHttpActionResult> PostDocumentVersion(DocumentVersion documentVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DocumentVersions.Add(documentVersion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = documentVersion.Id }, documentVersion);
        }

        // DELETE: api/DocumentVersions/5
        [ResponseType(typeof(DocumentVersion))]
        public async Task<IHttpActionResult> DeleteDocumentVersion(int id)
        {
            DocumentVersion documentVersion = await db.DocumentVersions.FindAsync(id);
            if (documentVersion == null)
            {
                return NotFound();
            }

            db.DocumentVersions.Remove(documentVersion);
            await db.SaveChangesAsync();

            return Ok(documentVersion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DocumentVersionExists(int id)
        {
            return db.DocumentVersions.Count(e => e.Id == id) > 0;
        }
    }
}