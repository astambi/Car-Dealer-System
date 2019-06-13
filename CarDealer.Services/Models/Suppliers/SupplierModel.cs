namespace CarDealer.Services.Models.Suppliers
{
    using CarDealer.Common.Mapping;
    using CarDealer.Data.Models;

    public class SupplierModel : IMapFrom<Supplier>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
