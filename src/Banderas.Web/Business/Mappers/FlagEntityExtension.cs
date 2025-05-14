using Banderas.Web.Data.Entities;
using Banderas.Web.Dtos;

namespace Banderas.Web.Business.Mappers
{
    public static class FlagEntityExtension
    {
        public static FlagDto ToDto(this FlagEntity flagEntity)
        {
            return new FlagDto
            {
                Id = flagEntity.Id,
                Name = flagEntity.Name,
                IsEnabled = flagEntity.Value
            };
        }

        public static List<FlagDto> ToDto(this List<FlagEntity> flagEntities)
        {
            return flagEntities.Select(flag => new FlagDto
            {
                Id = flag.Id,
                Name = flag.Name,
                IsEnabled = flag.Value
            }).ToList();
        }
    }
}
