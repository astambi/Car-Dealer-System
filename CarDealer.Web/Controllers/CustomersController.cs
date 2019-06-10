namespace CarDealer.Web.Controllers
{
    using System;
    using CarDealer.Services;
    using CarDealer.Services.Models;
    using CarDealer.Web.Infrastructure.Extensions;
    using CarDealer.Web.Infrastructure.Filters;
    using CarDealer.Web.Models.Customers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CustomersController : Controller
    {
        private const string CustomerFormView = "CustomerForm";

        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        public IActionResult All(string order)
        {
            var orderDirection =
                order == null
                || order.ToLower() == OrderDirection.Ascending.ToString().ToLower()
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

        [Route(WebConstants.CustomersControllerName + "/{id}")]
        public IActionResult TotalSales(int id)
        {
            var totalSales = this.customerService.TotalSalesById(id);
            return this.ViewOrRedirect(totalSales);
        }

        [Authorize]
        public IActionResult Create()
            => this.View(CustomerFormView,
                new CustomerFormModel
                {
                    Action = nameof(Create),
                    BirthDate = DateTime.Now // to avoid incorrect date initialisation
                });

        [Authorize]
        [HttpPost]
        [Log]
        public IActionResult Create(CustomerFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(CustomerFormView, model);
            }

            try
            {
                this.customerService.Create(model.Name, model.BirthDate, model.IsYoungDriver);
                return this.RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                return this.View(CustomerFormView, model);
            }
        }

        [Authorize]
        public IActionResult Edit(int id)
            => this.LoadEditDeleteView(id, nameof(Edit));

        [Authorize]
        [HttpPost]
        [Log]
        public IActionResult Edit(int id, CustomerFormModel model)
        {
            if (!this.customerService.Exists(id))
            {
                return this.RedirectToAction(nameof(All));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(CustomerFormView, model);
            }

            try
            {
                this.customerService.Update(id, model.Name, model.BirthDate);
                return this.RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                return this.View(CustomerFormView, model);
            }
        }

        [Authorize]
        public IActionResult Delete(int id)
            => this.LoadEditDeleteView(id, nameof(Delete));

        [Authorize]
        [HttpPost]
        [Log]
        public IActionResult Delete(int id, CustomerFormModel model)
        {
            if (!this.customerService.Exists(id))
            {
                return this.RedirectToAction(nameof(All));
            }

            try
            {
                this.customerService.Remove(id);
                return this.RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                return this.View(CustomerFormView, model);
            }
        }

        private IActionResult LoadEditDeleteView(int id, string action)
        {
            var customer = this.customerService.GetById(id);
            if (customer == null)
            {
                return this.RedirectToAction(nameof(All));
            }

            var model = new CustomerFormModel
            {
                Action = action,
                Name = customer.Name,
                BirthDate = customer.BirthDate,
                IsYoungDriver = customer.IsYoungDriver
            };

            return this.View(CustomerFormView, model);
        }
    }
}