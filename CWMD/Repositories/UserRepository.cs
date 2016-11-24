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
        private AuthContext context;

        private UserManager<IdentityUser> userManager;

        private static readonly ILog LOG = LogManager.GetLogger(typeof(UserRepository));

        public UserRepository()
        {
            context = new AuthContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
    }

        public async Task<IdentityResult> AddUser(User userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await userManager.CreateAsync(user, userModel.Password);
            LOG.Info("Add User");
            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            LOG.Info("Find User");
            return await userManager.FindAsync(userName, password);
        }

        public async Task<List<IdentityUser>> GetAllUsers()
        {
            LOG.Info("Get All Users");
            return await userManager.Users.ToListAsync();
        }

        public void Dispose()
        {
            context.Dispose();
            userManager.Dispose();

        }
    }
}