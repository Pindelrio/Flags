using Banderas.Web.Data.Entities;
using Banderas.Web.Dtos;

namespace Banderas.Web.Models
{
    internal class SingleFlagViewModel
    {
        public FlagDto Flag { get; set; }
        public string? Message { get; set; }
    }
}