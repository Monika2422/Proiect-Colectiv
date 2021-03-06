﻿using CWMD.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace CWMD
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Department> Departments { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<CWMD.Models.Document> Documents { get; set; }

        public System.Data.Entity.DbSet<CWMD.Models.DocumentVersion> DocumentVersions { get; set; }
    }
}