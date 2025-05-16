using Banderas.Web.Business.UseCases.Flags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;
using ROP.APIExtensions;

namespace Banderas.Web.Controllers.Api
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class FlagsController(GetSingleFlagUseCase getSingleFlag) : Controller
    {
        [ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status404NotFound)]
        [HttpGet("{flagName}")]
        public async Task<IActionResult> GetSingleFlag(string flagName)
            => await getSingleFlag.Execute(flagName).Map(a => a.IsEnabled).ToActionResult();
    }
}
