using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banderas.Web.Controllers.Api
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class FlagsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
