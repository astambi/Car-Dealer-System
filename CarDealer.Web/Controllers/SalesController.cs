namespace CarDealer.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Services;
    using CarDealer.Web.Infrastructure.Extensions;
    using CarDealer.Web.Infrastructure.Filters;
    using CarDealer.Web.Models.Sales;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize]
    public class SalesController : Controller
    {
        private const string AllView = "all";

        private readonly ICarService carService;
        private readonly ICustomerService customerService;
        private readonly ISaleService saleService;

        public SalesController(
            ICarService carService,
            ICustomerService customerService,
            ISaleService saleService)
        {
            this.carService = carService;
            this.customerService = customerService;
            this.saleService = saleService;
        }

        [AllowAnonymous]
        [Route("sales")]
        public IActionResult All()
        {
            var sales = this.saleService.All();
            return this.View(sales);
        }

        [AllowAnonymous]
        [Route("sales/{id}")]
        public IActionResult Details(int id)
        {
            var sales = this.saleService.ById(id);
            return this.ViewOrRedirect(sales);
        }

        [AllowAnonymous]
        [Route("sales/discounted/{discount?}")]
        public IActionResult Discounted(int? discount) // in percentage
        {
            var sales = this.saleService.AllDiscounted(discount);
            return this.View(AllView, sales);
        }

        public IActionResult Create()
        {
            var model = new SaleFormModel
            {
                Cars = this.GetCarsSelectList(),
                Customers = this.GetCustomersSelectList(),
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult ReviewCreate(SaleFormModel model)
        {
            if (!this.customerService.Exists(model.CustomerId))
            {
                this.ModelState.AddModelError(nameof(model.CustomerId), "Invalid customer.");
            }

            if (!this.carService.Exists(model.CarId))
            {
                this.ModelState.AddModelError(nameof(model.CarId), "Invalid car.");
            }

            if (!this.ModelState.IsValid)
            {
                model.Cars = this.GetCarsSelectList();
                model.Customers = this.GetCustomersSelectList();
                return this.View(nameof(Create), model);
            }

            var customer = this.customerService.GetByIdWithAdditionalDiscount(model.CustomerId);
            var car = this.carService.GetByIdWithPrice(model.CarId);

            var reviewModel = new SaleReviewModel
            {
                CustomerId = model.CustomerId,
                CarId = model.CarId,
                Discount = model.Discount,
                Customer = customer.Name,
                AdditionalDiscount = customer.AdditionalDiscount,
                Car = car.MakeModel,
                Price = car.Price,
            };

            return this.View(reviewModel);
        }

        [HttpPost]
        [Log(actionName: "create")]
        public IActionResult FinalizeCreate(SaleReviewModel model)
        {
            if (!this.customerService.Exists(model.CustomerId))
            {
                this.ModelState.AddModelError(nameof(model.CustomerId), "Invalid customer.");
            }

            if (!this.carService.Exists(model.CarId))
            {
                this.ModelState.AddModelError(nameof(model.CarId), "Invalid car.");
            }

            if (!this.ModelState.IsValid)
            {
                var createModel = new SaleFormModel
                {
                    CarId = model.CarId,
                    CustomerId = model.CustomerId,
                    Discount = model.Discount,
                    Cars = this.GetCarsSelectList(),
                    Customers = this.GetCustomersSelectList()
                };
                return this.View(nameof(Create), createModel);
            }

            this.saleService.Create(model.CustomerId, model.CarId, model.TotalDiscount);

            return this.RedirectToAction(nameof(All));
        }

        private IEnumerable<SelectListItem> GetCarsSelectList()
            => this.carService
            .AllDropdown()
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Make} {c.Model} {(c.TravelledDistance / 1000).ToNumber()} km"
            })
            .ToList();

        private IEnumerable<SelectListItem> GetCustomersSelectList()
           => this.customerService
            .AllDropdown()
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToList();
    }
}
