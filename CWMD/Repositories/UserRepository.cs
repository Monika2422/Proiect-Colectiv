using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using CWMD.Models;
using System.Data.Entity;

namespace CWMD.Repositories
{
    public class UserRepository : IDisposable
    {
        private ApplicationDbContext context;

        private UserManager<User> userManager;

        private static readonly ILog LOG = LogManager.GetLogger(typeof(UserRepository));

        public UserRepository()
        {
            context = new ApplicationDbContext();
            userManager = new UserManager<User>(new UserStore<User>(context));
        }

        public async Task<IdentityResult> AddUser(User user)
        {
            var result = await userManager.CreateAsync(user, "parola");
            LOG.Info("Add User");
            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            LOG.Info("Find User");
            return await userManager.FindAsync(userName, password);
        }

        public async Task<List<User>> GetAllUsers()
        {
            LOG.Info("Get All Users");
            return await context.Users.ToListAsync();
        }

        public void Dispose()
        {
            context.Dispose();
            userManager.Dispose();

        }
    }
}