namespace Mono.ViewModels
{
    public class ModelViewModel
    {
        public ModelViewModel(int makeId, string name, string abrv)
        {
            MakeId = makeId;
            Name = name;
            Abrv = abrv;
        }

        public ModelViewModel(int id, int makeId, string name, string abrv)
        {
            Id = id;
            MakeId = makeId;
            Name = name;
            Abrv = abrv;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }
        public int MakeId { get; set; }
    }
}
