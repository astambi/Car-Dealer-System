namespace CarDealer.Services.Models.Cars
{
    using CarDealer.Common.Mapping;
    using CarDealer.Data.Models;

    public class CarBasicModel : IMapFrom<Car>
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }
    }
}
