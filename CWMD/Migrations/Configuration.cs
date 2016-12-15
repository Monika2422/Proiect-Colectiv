namespace CWMD.Migrations
{
    using CWMD.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
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

            var user = new User()
            {
                UserName = "SuperPowerUser",
                Email = "stefanfai94@gmail.com",
                EmailConfirmed = true,
                Name = "Stefan Faiciuc"
            };

            var user2 = new User()
            {
                UserName = "CommonUser",
                Email = "isabella.ienciu13@gmail.com",
                EmailConfirmed = true,
                Name = "Isabella Ienciu"
            };

            manager.Create(user, "MySuperP@ss!");
            manager.Create(user2, "MyCommonP@ss!");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            roleManager.Create(new IdentityRole { Name = "Cititor" });
            roleManager.Create(new IdentityRole { Name = "Contributor" });
            roleManager.Create(new IdentityRole { Name = "Manager" });

            var adminUser = manager.FindByName("SuperPowerUser");
            var simpleUser = manager.FindByName("CommonUser");

            manager.AddToRoles(adminUser.Id, new string[] {"Admin" });
            manager.AddToRoles(simpleUser.Id, new string[] { "Cititor" });
        }
    }
}
