using System.Linq;
using System.Web.Http;

namespace CWMD.Controllers
{
    [Authorize]
    [RoutePrefix("api/departments")]
    public class DepartmentsController : BaseApiController
    {

        [Route("", Name = "GetAllDepartments")]
        public IHttpActionResult GetAllDepartments()
        {
            var departments = Context.Departments.ToArray();

            return Ok(departments);
        }

    }
}