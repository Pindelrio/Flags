using System.Security.Cryptography.X509Certificates;

namespace Banderas.Web.Dtos
{
    public class FlagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
    
}
