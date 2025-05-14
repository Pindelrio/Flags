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
        AddFlagUseCase addFlagUseCase,
        GetPaginatedFlagsUseCase getFlagsUseCase,
        GetSingleFlagUseCase getSingleFlagUseCase,
        UpdateFlagUseCase updateFlagUseCase,
        DeleteFlagUseCase deleteFlagUseCase
        ) : Controller
    {
        //[HttpGet("index")]
        [HttpGet("")]
        [HttpGet("{page:int}")]
        public async Task<IActionResult> Index(string? search, int page=1, int size=5)
        {
            var listFlags = await getFlagsUseCase.Execute(search, page, size);
            return View(new FlagIndexViewModel() { Pagination = listFlags.Value });
        }

        [HttpGet("{flagName}")]
        public IActionResult GetSingleFlag(string flagName, string message)
        {
            var singleFlag = getSingleFlagUseCase.Execute(flagName);
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
            
            Result<bool> isCreated = await addFlagUseCase.Execute(request.Name, request.IsEnabled);
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
            var singleFlag = await updateFlagUseCase.Execute(flag);
            return View("Single", new SingleFlagViewModel()
            {
                Flag = singleFlag.Value,
                Message = singleFlag.Success ? "Updated correctly" : singleFlag.Errors.First().Message
            });
        }

        [HttpGet("delete/{flagName}")]
        public async Task<IActionResult> Delete(string flagName)
        {
            Result<bool> isDeleted = await deleteFlagUseCase.Execute(flagName);
            if (isDeleted.Success)
                return RedirectToAction("Index");
            else
                return View("Single", new SingleFlagViewModel() { Flag = new FlagDto() { Name = flagName}, Message = isDeleted.Errors.First().Message });
        }
    }
}
