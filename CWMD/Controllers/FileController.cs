using CWMD.Models;
using CWMD.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CWMD.Utils;

namespace CWMD.Controllers
{
    [RoutePrefix("api/files")]
    public class FileController : BaseApiController
    {
        private DocumentsController documentsController = new DocumentsController();
        private DocumentVersionsController documentVersionsController = new DocumentVersionsController();

        [Route("upload")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadSingleFile()
        {

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    string filename = postedFile.FileName;
                    string[] filenameParts = filename.Split('.');

                    string filenameForSave = string.Empty;

                    var fileName = string.Join(".", filenameParts.Take(filenameParts.Length - 1).ToList());
                    var fileExt = filenameParts[filenameParts.Length - 1];

                    Document doc = documentsController.GetDocumentByName(fileName);

                    if (doc == null || doc.FileExtension != fileExt)
                    {
                        //document is new, create it
                        doc = new Document()
                        {
                            FileName = string.Join(".", fileName),
                            FileExtension = fileExt,
                            CreationDate = DateTime.Today,
                            TemplateName = null,
                            Abstract = HttpContext.Current.Request["Abstract"],
                            Status = "DRAFT",
                            KeyWords = HttpContext.Current.Request["KeyWords"],
                            AuthorUserName = HttpContext.Current.Request["Username"]
                        };

                        await documentsController.PostDocument(doc);
                        Document postedDoc = documentsController.GetDocumentByName(fileName);

                        //add first version
                        filenameForSave = $"{fileName}1.{fileExt}";
                        DocumentVersion docVersion = new DocumentVersion()
                        {
                            VersionNumber = 0.1f,
                            filePath = StringConstants.FolderPath + filenameForSave,
                            DocumentId = postedDoc.Id,
                            ModifiedBy = HttpContext.Current.Request["Username"],
                            CreationDate = DateTime.Today
                        };

                        await documentVersionsController.PostDocumentVersion(docVersion);
                    }

                    else
                    {
                        //doc already exists, add only new version

                        //get latest version number
                        List<DocumentVersion> existingVersions = documentVersionsController.GetVersions(doc.Id);
                        float latestVersionNumber = existingVersions?[0].VersionNumber ?? 0;

                        latestVersionNumber =
                            existingVersions?.Where(dv => dv.VersionNumber >= latestVersionNumber)
                                .Select(dv => dv.VersionNumber).OrderByDescending(verNum=>verNum).First() ?? 0;
                        //foreach (DocumentVersion dv in existingVersions)
                        //{
                        //    if (dv.VersionNumber > latestVersionNumber)
                        //    {
                        //        latestVersionNumber = dv.VersionNumber;
                        //    }
                        //}

                        //increment version number
                        float currentVersionNumber = latestVersionNumber + 0.1f;

                        //create document name
                        string s = currentVersionNumber.ToString("#.#########", System.Globalization.CultureInfo.InvariantCulture);
                        int i = Int32.Parse(s.Substring(s.IndexOf(".") + 1));
                        filenameForSave = $"{fileName}{i}.{fileExt}";
                        DocumentVersion docVersion = new DocumentVersion()
                        {
                            VersionNumber = currentVersionNumber,
                            filePath = Path.Combine(StringConstants.FolderPath, filenameForSave),
                            DocumentId = doc.Id,
                            ModifiedBy = HttpContext.Current.Request["Username"],
                            CreationDate = DateTime.Today
                        };
                        UpdateFileDescription(doc, HttpContext.Current.Request["Abstract"],
                            HttpContext.Current.Request["KeyWords"]);
                        await documentVersionsController.PostDocumentVersion(docVersion);

                    }

                    //save document version to disk
                    //mai dati-va-n cacat de-aci
                    //var filePath = HttpContext.Current.Server.MapPath(Path.Combine(StringConstants.FolderPath, filenameForSave));
                    
                    postedFile.SaveAs($"{StringConstants.FolderPath}\\{filenameForSave}");
                }
                return Ok();
            }
            else
            {
                return InternalServerError();
            }

        }

        private void UpdateFileDescription(Document doc, string @abstract, string keywords)
        {
            var document = Context.Documents.Where(docs => docs.Id == doc.Id)
                .Select(_doc => _doc).First();

            document.Abstract = @abstract;
            document.KeyWords = keywords;

            using(var context = new ApplicationDbContext())
            {
                context.Documents.AddOrUpdate(document);
                context.SaveChanges();
            }
        }
    }
}
