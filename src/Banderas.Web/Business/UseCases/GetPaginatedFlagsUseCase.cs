using Banderas.Web.Business.Mappers;
using Banderas.Web.Business.UserInfo;
using Banderas.Web.Data;
using Banderas.Web.Data.Entities;
using Banderas.Web.Dtos;
using Banderas.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ROP;
using System.Drawing;

namespace Banderas.Web.Business.UseCases
{
    public class GetPaginatedFlagsUseCase(ApplicationDbContext context, IFlagUserDetails userDetails)
    {
        public async Task<Result<Pagination<FlagDto>>> Execute(string? search, int page, int size)
        => await GetFromDB(search, page, size)
            .Map(x => x.ToDto())
            .Combine(x=>TotalElements(search))
            .Map(x=>new Pagination<FlagDto>(x.Item1, x.Item2, size, page, search));

        private async Task<Result<int>> TotalElements(string? search)
        {
            var query = context.Flags
            .Where(a => a.UserId == userDetails.UserId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase));
            }

            return await query.CountAsync();
        }

        //{
        //    string userId = userDetails.UserId;
        //    var flags = await context.Flags
        //        .Where(f => f.UserId == userId)
        //        .Select(f => new FlagDto
        //        {
        //            Name = f.Name,
        //            IsEnabled = f.Value
        //        })
        //        .AsNoTracking()
        //        .ToListAsync();
        //    return flags;
        //}

        private async Task<Result<List<FlagEntity>>> GetFromDB(string? search, int page, int size)
        {
            var query = context.Flags
                .Where(a => a.UserId == userDetails.UserId)
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
