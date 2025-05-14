using Banderas.Web.Business.UserInfo;
using Banderas.Web.Data;
using Banderas.Web.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Banderas.Web.Business.UseCases
{
    public class GetFlagsUseCase(ApplicationDbContext context, IFlagUserDetails userDetails)
    {
        public async Task<List<FlagDto>> Execute()
        {
            string userId = userDetails.UserId;
            var flags = await context.Flags
                .Where(f => f.UserId == userId)
                .Select(f => new FlagDto
                {
                    Name = f.Name,
                    IsEnabled = f.Value
                })
                .AsNoTracking()
                .ToListAsync();
            return flags;
        }
    }
}
