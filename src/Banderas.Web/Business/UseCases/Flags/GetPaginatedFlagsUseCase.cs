using Banderas.Web.Business.Mappers;
using Banderas.Web.Business.UserInfo;
using Banderas.Web.Data;
using Banderas.Web.Data.Entities;
using Banderas.Web.Dtos;
using Banderas.Web.Models;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace Banderas.Web.Business.UseCases.Flags
{
    public class GetPaginatedFlagsUseCase(ApplicationDbContext context, IFlagUserDetails userDetails)
    {
        public async Task<Result<Pagination<FlagDto>>> Execute(string? search, int page, int size)
        => await ValidatePage(page)
            .Fallback( _=> 
                {
                    page = 1; 
                    return Result.Unit; 
                })
                .Bind(_ => ValidatePageSize(size)
                    .Fallback( _ => 
                    { 
                        size = 5; 
                        return Result.Unit; 
                    })
            ).Async()
            .Bind(x => GetFromDB(search, page, size))
            .Map(x => x.ToDto())
            .Combine(x=>TotalElements(search))
            .Map(x=>new Pagination<FlagDto>(x.Item1, x.Item2, size, page, search));
        private Result<Unit> ValidatePage(int page)
        {
            if(page < 1)
            {
                return Result.Failure("Page must be greater than 0");
            }
            return Result.Unit;
        }

        private Result<Unit> ValidatePageSize(int size)
        {
            int[] allowedValues = [5, 10, 15];
            if (!allowedValues.Contains(size))
            {
                return Result.Failure($"Page size must be one of the following values: {string.Join(", ", allowedValues)}");
            }
            return Result.Unit;
        }


        private async Task<Result<int>> TotalElements(string? search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return await context.Flags.Where(a => a.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase)).CountAsync();
            }

            return await context.Flags.CountAsync();
        }

        private async Task<Result<List<FlagEntity>>> GetFromDB(string? search, int page, int size)
        {
            var query = context.Flags
                .Skip((page - 1) * size)
                .Take(size);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase));
            }

            return await query.ToListAsync();

        }
    }
}
