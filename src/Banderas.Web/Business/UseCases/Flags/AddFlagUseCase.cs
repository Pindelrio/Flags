using Banderas.Web.Business.UserInfo;
using Banderas.Web.Data;
using Banderas.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace Banderas.Web.Business.UseCases.Flags
{
   public class AddFlagUseCase(ApplicationDbContext context, IFlagUserDetails userDetails)
    {
        public async Task<Result<bool>> Execute(string flagName, bool isActive)
            => await ValidateFlag(flagName).Bind(x => AddFlagToDatabase(x, isActive));

        private async Task<Result<string>> ValidateFlag(string flagName)
        {
            bool flagExist = await context.Flags
                .Where(a => a.UserId == userDetails.UserId
                            && a.Name.Equals(flagName, StringComparison.InvariantCultureIgnoreCase)).AnyAsync();
            if (flagExist)
                return Result.Failure<string>($"Flag {flagName} already exists");

            return flagName;
        }

        private async Task<Result<bool>> AddFlagToDatabase(string flag, bool isActive)
        {
            var request = await context.Flags.AddAsync(new FlagEntity
            {
                Name = flag,
                Value = isActive,
                UserId = userDetails.UserId
            });
            await context.SaveChangesAsync();
            return true;
        }
    }
}