using CWMD.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Code7248.word_reader;
using CWMD.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CWMD.Controllers
{
    [RoutePrefix("api/doc")]
    public class DocumentController : BaseApiController
    {
        private DocumentRepository documentRepository;

        public DocumentController()
        {
            this.documentRepository = new DocumentRepository();
        }

        [Route("documents")]
        public IHttpActionResult GetDocuments()
        {
            return Ok(this.documentRepository.getDocuments().ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Route("versions/{id}")]
        public IHttpActionResult GetVersionById(int id)
        {
            return Ok(this.documentRepository.getVersionsForDocument(id).ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Route("versions")]
        public IHttpActionResult GetVersions()
        {
            return Ok(this.documentRepository.getDocumentVersions().ToList().Select(u => this.TheModelFactory.Create(u)));
        }


    }
}
