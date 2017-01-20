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
                    bool save = true;

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
                                .Select(dv => dv.VersionNumber).OrderByDescending(verNum => verNum).First() ?? 0;

                        float currentVersionNumber=0;
                        

                        //if status didn't change from draft to final
                        String status = HttpContext.Current.Request["Status"];
                        if (status == "DRAFT" && doc.Status == "DRAFT")
                        {
                            //increment version number
                            currentVersionNumber = latestVersionNumber + 0.1f;
                        }
                        else if (status == "FINAL" && doc.Status == "DRAFT")
                        {
                            currentVersionNumber = 1.0f;
                            doc.Status = "FINAL";
                            await documentsController.PutDocument(doc.Id, doc);

                        }
                        else if (status == "FINAL" && doc.Status == "FINAL")
                        {
                            currentVersionNumber = latestVersionNumber + 1.0f;
                        }

                        else //illegal arguments
                        {
                            save = false;
                        }

                        if (save)
                        {
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
                        else
                        {
                            return BadRequest();
                        }
                        

                    }

                    if(save){
                        //save document version to disk
                        postedFile.SaveAs($"{StringConstants.FolderPath}\\{filenameForSave}");
                    }
                    else
                    {
                        return BadRequest();
                    }
                    
                }
                return Ok();
            }
            else
            {
                return InternalServerError();
            }

        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteDocument(int id)
        {
            await documentsController.DeleteDocument(id);

            int versionsDeleted = 0;
            List<DocumentVersion> versions = documentVersionsController.GetVersions(id);
            for (int i = 0; i < versions.Count; i++)
            {
                await documentVersionsController.DeleteDocumentVersion(versions[i].Id);
                File.Delete(versions[i].filePath);
                versionsDeleted++;
            }
            if (versionsDeleted > 0)
            {
                return Ok();
            }
            else
            {
                return NotFound();
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
