namespace CarVilla.Areas.ViewModels
{
    public class UpdateClientVm
    {
        public int Id { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CityLocation { get; set; }
    }
}
