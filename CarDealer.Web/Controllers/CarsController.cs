namespace CarDealer.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Services;
    using CarDealer.Services.Models.Parts;
    using CarDealer.Web.Infrastructure.Extensions;
    using CarDealer.Web.Models.Cars;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CarsController : Controller
    {
        private const string All = "all";
        private const string CarFormView = "CarForm";

        private readonly ICarService carService;
        private readonly IPartService partService;

        public CarsController(ICarService carService, IPartService partService)
        {
            this.carService = carService;
            this.partService = partService;
        }

        [Route(WebConstants.CarsControllerName + "/{make?}")]
        public IActionResult AllByMake(string make)
        {
            var cars = this.carService.AllByMake(make);

            var model = new CarsByMakeModel
            {
                Make = make ?? All,
                Cars = cars
            };

            return this.View(model);
        }

        public IActionResult Create()
            => this.View(CarFormView,
                new CarFormModel { PartsSelectList = this.GetPartsSelectListItems(true) });

        [HttpPost]
        public IActionResult Create(CarFormModel carModel)
        {
            if (!this.ModelState.IsValid)
            {
                carModel.PartsSelectList = this.GetPartsSelectListItems(true);
                return this.View(CarFormView, carModel);
            }

            try
            {
                this.carService.Create(
                    carModel.Make,
                    carModel.Model,
                    carModel.TravelledDistance,
                    carModel.Parts);

                return this.RedirectToAction(nameof(AllByMake));
            }
            catch (Exception)
            {
                carModel.PartsSelectList = this.GetPartsSelectListItems();
                return this.View(CarFormView, carModel);
            }
        }

        public IActionResult Edit(int id)
            => this.LoadEditDeleteView(id, nameof(Edit));

        [HttpPost]
        public IActionResult Edit(int id, CarFormModel carModel)
        {
            if (!this.carService.Exists(id))
            {
                return this.RedirectToAction(nameof(AllByMake));
            }

            if (!this.ModelState.IsValid)
            {
                carModel.PartsSelectList = this.GetPartsSelectListItems();
                return this.View(CarFormView, carModel);
            }

            try
            {
                this.carService.Update(id, carModel.Make, carModel.Model, carModel.TravelledDistance, carModel.Parts);

                return this.RedirectToAction(nameof(AllByMake));
            }
            catch (Exception)
            {
                carModel.PartsSelectList = this.GetPartsSelectListItems();
                return this.View(CarFormView, carModel);
            }
        }

        public IActionResult Delete(int id)
            => this.LoadEditDeleteView(id, nameof(Delete));

        [HttpPost]
        public IActionResult Delete(int id, CarFormModel carModel)
        {
            if (!this.carService.Exists(id))
            {
                return this.RedirectToAction(nameof(AllByMake));
            }

            try
            {
                this.carService.Remove(id);
                return this.RedirectToAction(nameof(AllByMake));
            }
            catch (Exception)
            {
                return this.LoadEditDeleteView(id, nameof(Delete));
            }
        }

        public IActionResult Parts()
        {
            var cars = this.carService.AllWithParts();
            return this.View(cars);
        }

        private IEnumerable<SelectListItem> GetPartsSelectListItems(bool hasPositiveQuantity = false)
        {
            var parts = hasPositiveQuantity
                ? this.partService.AllDropdown().Where(p => p.Quantity != 0)
                : this.partService.AllDropdown();

            return this.MapToSelectList(parts);
        }

        private IEnumerable<SelectListItem> MapToSelectList(IEnumerable<PartBasicModel> parts)
            => parts
            .Select(p => new SelectListItem
            {
                Text = $"{p.Name} ({p.Quantity.ToNumber()} items in store)",
                Value = p.Id.ToString()
            })
            .ToList();

        private IActionResult LoadEditDeleteView(int id, string action)
        {
            var car = this.carService.GetById(id);

            if (car == null)
            {
                return this.RedirectToAction(nameof(AllByMake));
            }

            var carModel = new CarFormModel
            {
                Action = action,
                Make = car.Make,
                Model = car.Model,
                TravelledDistance = car.TravelledDistance,
                Parts = car.Parts,
                PartsSelectList = this.GetPartsSelectListItems()
            };

            return this.View(CarFormView, carModel);
        }
    }
}
