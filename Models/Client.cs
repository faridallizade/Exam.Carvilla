using CarVilla.Models.Common;

namespace CarVilla.Models
{
    public class Client:BaseEntity
    { 
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string CityLocation { get; set; }
    }
}
