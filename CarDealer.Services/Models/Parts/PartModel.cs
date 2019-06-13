namespace CarDealer.Services.Models.Parts
{
    using CarDealer.Common.Mapping;
    using CarDealer.Data.Models;

    public class PartModel : IMapFrom<Part>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
