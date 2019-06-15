﻿namespace CarDealer.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using CarDealer.Services;
    using CarDealer.Web.Infrastructure.Filters;
    using CarDealer.Web.Infrastructure.Helpers;
    using CarDealer.Web.Models;
    using CarDealer.Web.Models.Parts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize]
    public class PartsController : Controller
    {
        private const string PartFormView = "PartForm";

        private readonly IPartService partService;
        private readonly ISupplierService supplierService;
        private readonly IMapper mapper;

        public PartsController(
            IPartService partService,
            ISupplierService supplierService,
            IMapper mapper)
        {
            this.partService = partService;
            this.supplierService = supplierService;
            this.mapper = mapper;
        }

        // All Parts
        [AllowAnonymous]
        public IActionResult Index(int currentPage = 1)
        {
            var partsTotal = this.partService.Total();

            var totalPages = PaginationHelpers.GetTotalPages(partsTotal);
            currentPage = PaginationHelpers.GetValidCurrentPage(currentPage, totalPages);

            var partsCurrentPage = this.partService.All(currentPage, WebConstants.PageSize);

            var model = new PartPageListingModel
            {
                Parts = partsCurrentPage,
                Pagination = new PaginationModel
                {
                    Controller = WebConstants.PartsControllerName,
                    Action = nameof(Index),
                    CurrentPage = currentPage,
                    TotalPages = totalPages
                }
            };

            return this.View(model);
        }

        [AllowAnonymous]
        public IActionResult Details(int id) => this.View(); // TODO

        public IActionResult Create()
        {
            var model = new PartFormModel { Suppliers = this.GetSuppliersSelectListItems() };
            return this.View(PartFormView, model);
        }

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

        public IActionResult Edit(int id) 
            => this.LoadEditDeleteView(id, nameof(Edit));

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

        public IActionResult Delete(int id) 
            => this.LoadEditDeleteView(id, nameof(Delete));

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

            var model = this.mapper.Map<PartFormModel>(part);
            model.Action = action;
            model.Suppliers = this.GetSuppliersSelectListItems(); // cannot edit, display only

            return this.View(PartFormView, model);
        }
    }
}