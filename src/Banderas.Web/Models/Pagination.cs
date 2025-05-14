namespace Banderas.Web.Models
{
    public record Pagination<T>(List<T> Items, int TotalItems, int PageSize, int CurrentPage, string? Search);
    
}
