namespace CarDealer.Services.Models.Cars
{
    using System.ComponentModel.DataAnnotations;

    public class CarModel
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        [Display(Name = "Travelled distance")]
        public long TravelledDistance { get; set; }
    }
}
