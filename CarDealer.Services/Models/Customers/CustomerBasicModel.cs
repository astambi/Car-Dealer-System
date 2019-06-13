namespace CarDealer.Services.Models.Customers
{
    using CarDealer.Common.Mapping;
    using CarDealer.Data.Models;

    public class CustomerBasicModel : IMapFrom<Customer>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
