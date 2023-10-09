using System.ComponentModel.DataAnnotations;

namespace Mono.Models
{
    public class CreateModelRequest
    {
        [Required]
        public int MakeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
    }
}
