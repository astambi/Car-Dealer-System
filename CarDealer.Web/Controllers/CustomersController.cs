namespace CarDealer.Web.Controllers
{
    using CarDealer.Services;
    using CarDealer.Services.Models;
    using CarDealer.Web.Models.Customers;
    using Microsoft.AspNetCore.Mvc;

    public class CustomersController : Controller
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        public IActionResult All(string order)
        {
            var orderDirection =
                order == null ||
                order.ToLower() == OrderDirection.Ascending.ToString().ToLower()
                ? OrderDirection.Ascending
                : OrderDirection.Descending;

            var customers = this.customerService.AllOrdered(orderDirection);

            var model = new CustomersAllModel
            {
                Customers = customers,
                OrderDirection = orderDirection
            };

            return this.View(model);
        }

        public IActionResult Create() => this.View();

        public IActionResult Edit(int id) => this.View();

        [Route("customers/{id}")]
        public IActionResult TotalSales(int id)
        {
            var totalSales = this.customerService.TotalSalesById(id);

            return this.View(totalSales);
        }
    }
}