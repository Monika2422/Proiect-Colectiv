using CWMD.Models;
using CWMD.Repositories;
using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CWMD.Controllers
{
    [RoutePrefix("api/Users")]
    public class UserController : ApiController
    {
        private UserRepository _repo = null;

        private static readonly ILog LOG = LogManager.GetLogger(typeof(UserController));
        

        public UserController()
        {
            _repo = new UserRepository();
        }

        // POST api/User
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> AddUser(User userModel)
        {
            LOG.Info("Add User");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.AddUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                LOG.Warn("Error in add user");
                return errorResult;
            }
            LOG.Info("User added!");
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsers()
        {
            List<User> result = await _repo.GetAllUsers();
            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
