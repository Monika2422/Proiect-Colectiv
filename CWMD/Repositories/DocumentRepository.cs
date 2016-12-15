using CWMD.Dtos;
using CWMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace CWMD.Repositories
{
    public class DocumentRepository
    {
        private List<Document> documents;
        private List<DocumentVersion> documentVersions;
        private string filesDirectory = "D:\\university\\proiect-colectiv\\descarcat\\cwmd\\cwmd\\files\\";

        public DocumentRepository()
        {
            this.documents = new List<Document>();
            this.documentVersions = new List<DocumentVersion>();
            populate();
        }

        private void populate()
        {
            Document doc1 = new Document();
            doc1.Id = 1;
            doc1.FileName = "doc1";
            doc1.FileExtension = "doc";
            doc1.CreationDate = DateTime.Today;
            doc1.TemplateName = null;
            doc1.Abstract = "abstract stuff";
            doc1.Status = "DRAFT";
            doc1.KeyWords = "keyword1 keyword2";
            doc1.AuthorUserName = "SuperPowerUser";

            Document doc2 = new Document();
            doc2.Id = 2;
            doc2.FileName = "doc2";
            doc2.FileExtension = "doc";
            doc2.CreationDate = DateTime.Today;
            doc2.TemplateName = null;
            doc2.Abstract = "more abstract stuff";
            doc2.Status = "DRAFT";
            doc2.KeyWords = "keyword1 keyword2 keyword3";
            doc2.AuthorUserName = "SuperPowerUser";

            documents.Add(doc1);
            documents.Add(doc2);

            DocumentVersion docver1 = new DocumentVersion();
            docver1.Id = 1;
            docver1.VersionNumber = 0.1f;
            docver1.filePath = filesDirectory + doc1.FileName + docver1.Id.ToString() +"."+ doc1.FileExtension; 
            docver1.DocumentId = doc1.Id;
            docver1.ModifiedBy = "SuperPowerUser";
            docver1.CreationDate = DateTime.Today;

            DocumentVersion docver2 = new DocumentVersion();
            docver2.Id = 2;
            docver2.VersionNumber = 0.2f;
            docver2.filePath = filesDirectory + doc1.FileName + docver2.Id.ToString() + "." + doc1.FileExtension;
            docver2.DocumentId = doc1.Id;
            docver2.ModifiedBy = "SuperPowerUser";
            docver2.CreationDate = DateTime.Today;

            DocumentVersion docver21 = new DocumentVersion();
            docver21.Id = 3;
            docver21.VersionNumber = 0.1f;
            docver21.filePath = filesDirectory + doc2.FileName + docver21.Id.ToString() + "." + doc2.FileExtension;
            docver21.DocumentId = doc2.Id;
            docver21.ModifiedBy = "SuperPowerUser";
            docver21.CreationDate = DateTime.Today;

            //add versions for doc1
            documentVersions.Add(docver1);
            documentVersions.Add(docver2);

            //add versions for doc2
            documentVersions.Add(docver21);

        }

        public List<Document> getDocuments()
        {
            return this.documents;
        }

        public List<DocumentVersion> getDocumentVersions()
        {
            return this.documentVersions;
        }

        public List<DocumentVersion> getVersionsForDocument(int id)
        {
            List<DocumentVersion> result = new List<DocumentVersion> ();
            for(int i = 0; i < this.documentVersions.Count; i++)
            {
                if (documentVersions[i].DocumentId == id)
                {
                    result.Add(documentVersions[i]);
                }
            }
            return result;
        }
    }
}