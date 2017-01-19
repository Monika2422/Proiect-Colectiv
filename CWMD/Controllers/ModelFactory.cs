using Code7248.word_reader;
using CWMD.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace CWMD.Controllers
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private ApplicationUserManager _AppUserManager;
        private ApplicationDbContext _context;

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserReturnModel Create(User appUser)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                Name = appUser.Name,
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result,
                Department = appUser.Department != null ? appUser.Department.Name : ""
            };
        }

        public RoleReturnModel Create(IdentityRole appRole)
        {
            return new RoleReturnModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };
        }

        public DocumentReturnModel Create(Document document)
        {
            return new DocumentReturnModel
            {
                //Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = document.Id,
                FileName = document.FileName,
                FileExtension = document.FileExtension,
                CreationDate = document.CreationDate,
                TemplateName = document.TemplateName,
                Abstract = document.Abstract,
                Status = document.Status,
                KeyWords = document.KeyWords,
                AuthorUserName = document.AuthorUserName
            };
        }

        public DocumentVersionReturnModel Create(DocumentVersion documentVersion)
        {
            TextExtractor extractor = new TextExtractor(documentVersion.filePath);
            return new DocumentVersionReturnModel
            {
                Id = documentVersion.Id,
                Text = extractor.ExtractText(),
                //FilePath = documentVersion.filePath,
                DocumentId = documentVersion.DocumentId,
                ModifiedBy = documentVersion.ModifiedBy,
                CreationDate = documentVersion.CreationDate,
                VersionNumber = documentVersion.VersionNumber,
            };
        }
    }


    public class RoleReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class UserReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Department { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
    }

    public class DocumentReturnModel
    {
        //public string Url { get; set; }
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public DateTime CreationDate { get; set; }
        public string TemplateName { get; set; }
        public string Abstract { get; set; }
        public string Status { get; set; }
        public string KeyWords { get; set; }
        public string AuthorUserName { get; set; }
    }

    public class DocumentVersionReturnModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        //public string FilePath { get; set; }
        public int DocumentId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public float VersionNumber { get; set; }
    }
}