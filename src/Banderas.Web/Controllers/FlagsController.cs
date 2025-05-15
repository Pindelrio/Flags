using Banderas.Web.Business.UseCases;
using Banderas.Web.Dtos;
using Banderas.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;

namespace Banderas.Web.Controllers
{
    [Authorize] //Comprova que estigui loguejat
    [Route("Flags")]
    public class FlagsController(
        FlagsUseCases flags
        ) : Controller
    {
        //[HttpGet("index")]
        [HttpGet("")]
        [HttpGet("{page:int}")]
        public async Task<IActionResult> Index(string? search, int page=1, int size=5)
        {
            var listFlags = await flags.GetPaginated.Execute(search, page, size);
            return View(new FlagIndexViewModel() { Pagination = listFlags.Value });
        }

        [HttpGet("{flagName}")]
        public IActionResult GetSingleFlag(string flagName, string message)
        {
            var singleFlag = flags.Get.Execute(flagName);
            return View("Single", new SingleFlagViewModel() { Flag = singleFlag.Result.Value, Message = message });
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new FlagViewModel());
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddFlagToDatabase(FlagViewModel request)
        {
            
            Result<bool> isCreated = await flags.Add.Execute(request.Name, request.IsEnabled);
            if(isCreated.Success)
                return RedirectToAction("Index");

            return View("Create", new FlagViewModel() 
                { 
                Name = request.Name, 
                IsEnabled = request.IsEnabled, 
                Error = isCreated.Errors.First().Message
            });
        }

        [HttpPost("{flagName}")]
        public async Task<IActionResult> UpdateFlag(FlagDto flag)
        {
            var singleFlag = await flags.Update.Execute(flag);
            return View("Single", new SingleFlagViewModel()
            {
                Flag = singleFlag.Value,
                Message = singleFlag.Success ? "Updated correctly" : singleFlag.Errors.First().Message
            });
        }

        [HttpGet("delete/{flagName}")]
        public async Task<IActionResult> Delete(string flagName)
        {
            Result<bool> isDeleted = await flags.Delete.Execute(flagName);
            if (isDeleted.Success)
                return RedirectToAction("Index");
            else
                return View("Single", new SingleFlagViewModel() { Flag = new FlagDto() { Name = flagName}, Message = isDeleted.Errors.First().Message });
        }
    }
}
