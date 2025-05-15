using Banderas.Web.Business.Mappers;
using Banderas.Web.Business.UserInfo;
using Banderas.Web.Data;
using Banderas.Web.Data.Entities;
using Banderas.Web.Dtos;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace Banderas.Web.Business.UseCases.Flags
{
    public class UpdateFlagUseCase(ApplicationDbContext context, IFlagUserDetails userDetails)
    {
        public async Task<Result<FlagDto>> Execute(FlagDto flagDto)
        => await VerifyIsTheOnlyOneWithThatName(flagDto).Bind(x => GetFromDb(x.Id)).Bind(x => Update(x, flagDto)).Map(x => x.ToDto());

        private async Task<Result<FlagDto>> VerifyIsTheOnlyOneWithThatName(FlagDto flagDto)
        {
            bool alreadyExist = await context.Flags
                .Where(a => a.Name.Equals(flagDto.Name, StringComparison.InvariantCultureIgnoreCase)
                    && a.Id != flagDto.Id)
                .AnyAsync();
            if ( alreadyExist)
                return Result.Failure<FlagDto>("Flag with the same name already exist");

            return flagDto;
        }   
        
        private async Task<Result<FlagEntity>> GetFromDb(int id)
        => await context.Flags
            .Where(a => a.Id == id)
            //.AsNoTracking() aqui no fem tracking perque volem que es pugui modificar
            .SingleAsync(); 

        private async Task<Result<FlagEntity>> Update(FlagEntity entity, FlagDto flagDto)
        {
            entity.Name = flagDto.Name;
            entity.Value = flagDto.IsEnabled;
            await context.SaveChangesAsync();
            return entity;
        }

    }
}
