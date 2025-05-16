using Banderas.Web.Business.Mappers;
using Banderas.Web.Business.UserInfo;
using Banderas.Web.Data;
using Banderas.Web.Data.Entities;
using Banderas.Web.Dtos;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace Banderas.Web.Business.UseCases.Flags
{
    public class GetSingleFlagUseCase(ApplicationDbContext context, IFlagUserDetails userDetails)
    {
        public async Task<Result<FlagDto>> Execute(string flagName)
            => await GetFromDB(flagName)
            .Bind(flag => flag?? Result.NotFound<FlagEntity>("Flag doesn't exist")).Map(x => x.ToDto());
        private async Task<Result<FlagEntity?>> GetFromDB(string flagName)
            => await context.Flags
                .Where(a => a.Name.Equals(flagName, StringComparison.InvariantCultureIgnoreCase))
                .AsNoTracking()
                .FirstOrDefaultAsync();
                //.SingleAsync(); //si no existeix llança excepcio

    }
}
