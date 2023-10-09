namespace Mono.ViewModels
{
    public class MakeViewModel
    {
        public MakeViewModel(string name, string abrv)
        {
            Name = name;
            Abrv = abrv;
        }

        public MakeViewModel(int id, string name, string? abrv)
        {
            Id = id;
            Name = name;
            Abrv = abrv;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }
    }
}
