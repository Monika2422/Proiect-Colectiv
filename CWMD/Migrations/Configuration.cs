namespace CWMD.Migrations
{
    using CWMD.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var dep = new Department() { Name = "Admin Department" };

            context.Departments.Add(dep);
            context.Departments.Add(new Department() { Name = "Computer Science" });
            context.Departments.Add(new Department() { Name = "Mathematics" });
            context.Departments.Add(new Department() { Name = "Deanship" });

            context.SaveChanges();

            var found = context.Departments.Where(d => d.Name == dep.Name);

            var foundDep = found.ToList();

            dep = foundDep[0];

            var user = new User()
            {
                UserName = "admin",
                Email = "stefanfai94@gmail.com",
                EmailConfirmed = true,
                Name = "admin",
                DepartmentID = dep.DepartmentID
            };

            manager.Create(user, "adminpass123");

            var user2 = new User()
            {
                UserName = "manager",
                Email = "isabellaienciu@gmail.com",
                EmailConfirmed = true,
                Name = "manager",
                DepartmentID = dep.DepartmentID
            };

            manager.Create(user2, "manager007");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "Contributor" });
                roleManager.Create(new IdentityRole { Name = "Manager" });
                roleManager.Create(new IdentityRole { Name = "Reader" });
            }

            context.SaveChanges();

            var adminUser = manager.FindByName("admin");
            var managerUser = manager.FindByName("manager");

            manager.AddToRoles(adminUser.Id, new string[] {"Admin"});
            manager.AddToRoles(managerUser.Id, new string[] { "Manager" });

            context.SaveChanges();

            context.Documents.AddOrUpdate(x => x.Id,
                new Document()
                {
                    FileName = "doc1",
                    FileExtension = "doc",
                    CreationDate = DateTime.Today,
                    TemplateName = null,
                    Abstract = "abstract stuff",
                    Status = "DRAFT",
                    KeyWords = "keyword1 keyword2",
                    AuthorUserName = "admin"
                },
                new Document()
                {
                    FileName = "doc2",
                    FileExtension = "doc",
                    CreationDate = DateTime.Today,
                    TemplateName = null,
                    Abstract = "more abstract stuff",
                    Status = "DRAFT",
                    KeyWords = "keyword1 keyword2 keyword3",
                    AuthorUserName = "admin"
                },
                new Document()
                {
                    FileName = "doc3",
                    FileExtension = "doc",
                    CreationDate = DateTime.Today,
                    TemplateName = null,
                    Abstract = "manager abstract stuff",
                    Status = "DRAFT",
                    KeyWords = "keyword1 keyword2 keyword3",
                    AuthorUserName = "manager"
                }
            );

        string filesDirectory = "D:\\Dev\\ProiectColectiv\\CWMD\\CWMD\\App_Data\\UPLOADS\\";

        context.DocumentVersions.AddOrUpdate(x => x.Id,
                new DocumentVersion()
                {
                    VersionNumber = 0.1f,
                    filePath = filesDirectory + "doc1" + "1" + "." + "doc",
                    DocumentId = 1,
                    ModifiedBy = "admin",
                    CreationDate = DateTime.Today
                },
                new DocumentVersion()
                {
                    VersionNumber = 0.2f,
                    filePath = filesDirectory + "doc1" + "2" + "." + "doc",
                    DocumentId = 1,
                    ModifiedBy = "admin",
                    CreationDate = DateTime.Today
                },
                new DocumentVersion()
                {
                    VersionNumber = 0.1f,
                    filePath = filesDirectory + "doc2" + "1" + "." + "doc",
                    DocumentId = 2,
                    ModifiedBy = "admin",
                    CreationDate = DateTime.Today
                },
                new DocumentVersion()
                {
                    VersionNumber = 0.1f,
                    filePath = filesDirectory + "doc3" + "1" + "." + "doc",
                    DocumentId = 3,
                    ModifiedBy = "manager",
                    CreationDate = DateTime.Today
                }
            );
        }
    }
}
