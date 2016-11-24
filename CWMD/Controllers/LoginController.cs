using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CWMD.Controllers
{
    [RoutePrefix("api/auth")]
    public class LoginController : ApiController
    {
        AuthorizationServerProvider authServerProvider;

        LoginController()
        {
            authServerProvider = new AuthorizationServerProvider();
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IHttpActionResult> Login()
        //{
            //List<User> result = await _repo.GetAllUsers();
            //return Ok(result);
        //}
    }
}
