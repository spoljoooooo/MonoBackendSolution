namespace Mono.Models
{
    public class VehicleModel
    {
        public VehicleModel(int makeId, string name, string abrv)
        {
            MakeId = makeId;
            Name = name;
            Abrv = abrv;
        }

        public VehicleModel(int id, int makeId, string name, string abrv)
        {
            Id = id;
            MakeId = makeId;
            Name = name;
            Abrv = abrv;
        }

        public int Id { get; set; }
        public int MakeId { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }

        public VehicleMake Make { get; set; }
    }
}
