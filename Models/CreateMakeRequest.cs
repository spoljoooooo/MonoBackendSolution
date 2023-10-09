using System.ComponentModel.DataAnnotations;

namespace Mono.Models
{
    public class CreateMakeRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
    }
}
