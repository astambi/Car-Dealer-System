namespace CarDealer.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Services;
    using CarDealer.Web.Infrastructure.Filters;
    using CarDealer.Web.Models;
    using CarDealer.Web.Models.Parts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PartsController : Controller
    {
        private const string PartFormView = "PartForm";

        private readonly IPartService partService;
        private readonly ISupplierService supplierService;

        public PartsController(IPartService partService, ISupplierService supplierService)
        {
            this.partService = partService;
            this.supplierService = supplierService;
        }

        // GET: Parts
        public IActionResult Index(int currentPage = 1)
        {
            var model = new PartPageListingModel
            {
                Parts = this.partService.All(currentPage, WebConstants.PageSize),
                Pagination = new PaginationModel
                {
                    CurrentPage = currentPage,
                    TotalPages = (int)Math.Ceiling(this.partService.Total() / (double)WebConstants.PageSize),
                    Controller = WebConstants.PartsControllerName,
                    Action = nameof(Index)
                }
            };

            return this.View(model);
        }

        public IActionResult Details(int id) => this.View();

        [Authorize]
        public IActionResult Create()
        {
            var model = new PartFormModel { Suppliers = this.GetSuppliersSelectListItems() };
            return this.View(PartFormView, model);
        }

        [Authorize]
        [HttpPost]
        [Log]
        public IActionResult Create(PartFormModel model)
        {
            if (!this.supplierService.Exists(model.SupplierId))
            {
                this.ModelState.AddModelError(nameof(PartFormModel.SupplierId), "Invalid supplier.");
            }

            if (!this.ModelState.IsValid)
            {
                model.Suppliers = this.GetSuppliersSelectListItems();
                return this.View(PartFormView, model);
            }

            try
            {
                this.partService.Create(model.Name, model.Price, model.Quantity, model.SupplierId);
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                model.Suppliers = this.GetSuppliersSelectListItems();
                return this.View(PartFormView, model);
            }
        }

        [Authorize]
        public IActionResult Edit(int id) => this.LoadEditDeleteView(id, nameof(Edit));

        [Authorize]
        [HttpPost]
        [Log]
        public IActionResult Edit(int id, PartFormModel model)
        {
            if (!this.partService.Exists(id))
            {
                return this.RedirectToAction(nameof(Index));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(PartFormView, model);
            }

            try
            {
                this.partService.Update(id, model.Price, model.Quantity);
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return this.View(PartFormView, model);
            }
        }

        [Authorize]
        public IActionResult Delete(int id) => this.LoadEditDeleteView(id, nameof(Delete));

        [Authorize]
        [HttpPost]
        [Log]
        public IActionResult Delete(int id, PartFormModel model)
        {
            if (!this.partService.Exists(id))
            {
                return this.RedirectToAction(nameof(Index));
            }

            try
            {
                this.partService.Remove(id);
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return this.View(PartFormView, model);
            }
        }

        private IEnumerable<SelectListItem> GetSuppliersSelectListItems()
            => this.supplierService
            .AllDropDown()
            .Select(s => new SelectListItem // NB!
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });

        private IActionResult LoadEditDeleteView(int id, string action)
        {
            var part = this.partService.GetById(id);
            if (part == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            var model = new PartFormModel
            {
                Action = action,
                Name = part.Name,
                Price = part.Price,
                Quantity = part.Quantity
            };

            return this.View(PartFormView, model);
        }
    }
}