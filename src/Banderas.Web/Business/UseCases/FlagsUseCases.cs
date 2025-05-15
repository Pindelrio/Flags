using Banderas.Web.Business.UseCases.Flags;

namespace Banderas.Web.Business.UseCases
{
    public record class FlagsUseCases(
        AddFlagUseCase Add,
        GetPaginatedFlagsUseCase GetPaginated,
        GetSingleFlagUseCase Get,
        UpdateFlagUseCase Update,
        DeleteFlagUseCase Delete
    )
    {
    }
}
