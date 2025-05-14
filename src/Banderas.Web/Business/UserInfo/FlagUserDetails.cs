using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Banderas.Web.Business.UserInfo
{
    public interface IFlagUserDetails
    {
        public string UserId { get; }
    }

    // Es pot fer de les dues formes
    //public class FlagUserDetails(IHttpContextAccessor httpContextAccessor) : IFlagUserDetails
    public class FlagUserDetails(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager) : IFlagUserDetails
    {
        //public string UserId => httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        public string UserId => userManager.GetUserId(httpContextAccessor.HttpContext!.User) 
            ?? throw new Exception("This workflow requiere authentication");
    }
}
