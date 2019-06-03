namespace CarDealer.Web.Models.Customers
{
    using System.Collections.Generic;
    using CarDealer.Services.Models;
    using CarDealer.Services.Models.Customers;

    public class CustomersAllModel
    {
        public IEnumerable<CustomerModel> Customers { get; set; }

        public OrderDirection OrderDirection { get; set; }
    }
}
