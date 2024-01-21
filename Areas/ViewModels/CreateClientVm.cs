namespace CarVilla.Areas.ViewModels
{
    public class CreateClientVm
    {
        public IFormFile ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public string Name {  get; set; }
        public string Description { get; set; }
        public string CityLocation { get; set; }
    }
}
