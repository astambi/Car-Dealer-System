namespace CarDealer.Data.Models
{
    public class PartCar
    {
        public int CarId { get; set; }

        public Car Car { get; set; }

        public int PartId { get; set; }

        public Part Part { get; set; }
    }
}
