using System.ComponentModel.DataAnnotations;

namespace Mono.Models
{
    public class VehicleMake
    {
        public VehicleMake(string name, string abrv)
        {
            Name = name;
            Abrv = abrv;
        }

        public VehicleMake(int id, string name, string abrv)
        {
            Id = id;
            Name = name;
            Abrv = abrv;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Abrv { get; set; }

        public ICollection<VehicleModel> Models { get; set; }
    }
}
