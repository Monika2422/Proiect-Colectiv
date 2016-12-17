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

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "Contributor" });
                roleManager.Create(new IdentityRole { Name = "Manager" });
                roleManager.Create(new IdentityRole { Name = "Reader" });
            }

            context.SaveChanges();

            var adminUser = manager.FindByName("admin");

            manager.AddToRoles(adminUser.Id, new string[] {"Admin"});

            context.SaveChanges();
        }
    }
}
