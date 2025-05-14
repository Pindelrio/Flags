using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Banderas.Web.Data.Entities
{
    public class FlagEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public IdentityUser User { get; set; }  //Aquest no existeix a la Bd
        public required virtual string UserId { get; set; } // Es la clau forana de la taula de usuaris
        public required bool  Value { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTimeUtc { get; set; }
    }
}
