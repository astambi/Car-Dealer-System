namespace CarDealer.Services.Models.Parts
{
    using CarDealer.Common.Mapping;
    using CarDealer.Data.Models;

    public class PartBasicModel : IMapFrom<Part>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}
