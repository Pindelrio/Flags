using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Banderas.Web.Business.UserInfo
{
    public interface IFlagUserDetails
    {
        public string UserId { get; }
    }

    // Es pot fer de les dues formes pero si ho posem al ApplicationDbContext el IdentityDbContext hi ha una referencia circular.
    // per això fem servir directament el IHttpContextAccessor
    public class FlagUserDetails(IHttpContextAccessor httpContextAccessor) : IFlagUserDetails
    //public class FlagUserDetails(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager) : IFlagUserDetails
    {
        public string UserId => httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("This workflow requiere authentication");
        //public string UserId => userManager.GetUserId(httpContextAccessor.HttpContext!.User) 
        //    ?? throw new Exception("This workflow requiere authentication");
    }
}
