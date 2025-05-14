using Banderas.Web.Business.UserInfo;
using Banderas.Web.Data;
using Banderas.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace Banderas.Web.Business.UseCases
{
    public class DeleteFlagUseCase(ApplicationDbContext context, IFlagUserDetails userDetails)
    {
        public async Task<Result<bool>> Execute(string flagName)
            => await GetFromDB(flagName).Bind(x => Delete(x));
        
        private async Task<Result<FlagEntity>> GetFromDB(string flagName)
            => await context.Flags
                .Where(a => a.UserId == userDetails.UserId
                    && a.Name.Equals(flagName, StringComparison.InvariantCultureIgnoreCase))
                .SingleAsync(); //si no existeix llança excepcio
        private async Task<Result<bool>> Delete(FlagEntity entity)
        {
            //context.Flags.Remove(entity);
            entity.IsDeleted = true;
            entity.DeletedTimeUtc = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return true;
        } 
    }
}
