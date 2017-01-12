using CWMD.Models;
using CWMD.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CWMD.Controllers
{
    [RoutePrefix("api/files")]
    public class FileController : BaseApiController
    {
        private DocumentsController documentsController = new DocumentsController();
        private DocumentVersionsController documentVersionsController = new DocumentVersionsController();
        private string filesDirectory = "D:\\Dev\\ProiectColectiv\\CWMD\\CWMD\\App_Data\\UPLOADS\\";

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

                    string filenameForSave;

                    Document doc = documentsController.GetDocumentByName(filenameParts[0]);
                    if (doc == null)
                    {
                        //document is new, create it
                        doc = new Document()
                        {
                            FileName = filenameParts[0],
                            FileExtension = filenameParts[1],
                            CreationDate = DateTime.Today,
                            TemplateName = null,
                            Abstract = HttpContext.Current.Request["Abstract"],
                            Status = "DRAFT",
                            KeyWords = HttpContext.Current.Request["KeyWords"],
                            AuthorUserName = HttpContext.Current.Request["Username"]
                        };

                        await documentsController.PostDocument(doc);
                        Document postedDoc = documentsController.GetDocumentByName(filenameParts[0]);

                        //add first version
                        filenameForSave = filenameParts[0] + "1" + "." + filenameParts[1];
                        DocumentVersion docVersion = new DocumentVersion()
                        {
                            VersionNumber = 0.1f,
                            filePath = filesDirectory + filenameForSave,
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
                        float latestVersionNumber = existingVersions[0].VersionNumber;
                        foreach (DocumentVersion dv in existingVersions)
                        {
                            if (dv.VersionNumber > latestVersionNumber)
                            {
                                latestVersionNumber = dv.VersionNumber;
                            }
                        }

                        //increment version number
                        float currentVersionNumber = latestVersionNumber + 0.1f;

                        //create document name
                        string s = currentVersionNumber.ToString("#.#########", System.Globalization.CultureInfo.InvariantCulture);
                        int i = Int32.Parse(s.Substring(s.IndexOf(".") + 1));
                        filenameForSave = filenameParts[0] + i.ToString() + "." + filenameParts[1];
                        DocumentVersion docVersion = new DocumentVersion()
                        {
                            VersionNumber = currentVersionNumber,
                            filePath = filesDirectory + filenameForSave,
                            DocumentId = doc.Id,
                            ModifiedBy = HttpContext.Current.Request["Username"],
                            CreationDate = DateTime.Today
                        };

                        await documentVersionsController.PostDocumentVersion(docVersion);

                    }

                    //save document version to disk
                    var filePath = HttpContext.Current.Server.MapPath("~/App_Data/UPLOADS/" + filenameForSave);
                    postedFile.SaveAs(filePath);
                }
                return Ok();
            }
            else
            {
                return InternalServerError();
            }

        }
    }
}
