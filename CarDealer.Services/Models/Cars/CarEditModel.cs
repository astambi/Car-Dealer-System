namespace CarDealer.Services.Models.Cars
{
    using System.Collections.Generic;

    public class CarEditModel : CarModel
    {
        public IEnumerable<int> Parts { get; set; } = new List<int>();
    }
}
