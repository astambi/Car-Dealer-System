namespace CarDealer.Web.Controllers
{
    using CarDealer.Services;
    using CarDealer.Web.Infrastructure.Filters;
    using CarDealer.Web.Models.Suppliers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class SuppliersController : Controller
    {
        private const string SuppliersView = "Suppliers";
        private const string SupplierFormView = "SupplierForm";

        private readonly ISupplierService supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            this.supplierService = supplierService;
        }

        [Route(WebConstants.SuppliersControllerName)]
        public IActionResult All()
        {
            var model = new SuppliersByTypeModel
            {
                Type = "All",
                Suppliers = this.supplierService.All()
            };

            return this.View(SuppliersView, model);
        }

        public IActionResult Local()
        {
            var model = this.GetByType(nameof(Local));
            return this.View(SuppliersView, model);
        }

        public IActionResult Importers()
        {
            var model = this.GetByType(nameof(Importers));
            return this.View(SuppliersView, model);
        }

        [Authorize]
        public IActionResult Create()
            => this.View(SupplierFormView, new SupplierFormModel());

        [Authorize]
        [HttpPost]
        [Log]
        public IActionResult Create(SupplierFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(SupplierFormView, model);
            }

            this.supplierService.Create(model.Name, model.IsImporter);

            return this.RedirectToAction(model.IsImporter ? nameof(Importers) : nameof(Local));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!this.supplierService.Exists(id))
            {
                return this.RedirectToAction(nameof(All));
            }

            var supplier = this.supplierService.GetById(id);

            var model = new SupplierFormModel
            {
                Action = nameof(Delete),
                Name = supplier.Name,
                IsImporter = supplier.IsImporter
            };

            return this.View(SupplierFormView, model);
        }

        [Authorize]
        [HttpPost]
        [Log]
        public IActionResult Delete(int id, SupplierFormModel model)
        {
            if (!this.supplierService.Exists(id))
            {
                return this.RedirectToAction(nameof(All));
            }

            this.supplierService.Remove(id);

            return this.RedirectToAction(model.IsImporter ? nameof(Importers) : nameof(Local));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!this.supplierService.Exists(id))
            {
                return this.RedirectToAction(nameof(All));
            }

            var supplier = this.supplierService.GetById(id);

            var model = new SupplierFormModel
            {
                Action = nameof(Edit),
                Name = supplier.Name,
                IsImporter = supplier.IsImporter
            };

            return this.View(SupplierFormView, model);
        }

        [Authorize]
        [HttpPost]
        [Log]
        public IActionResult Edit(int id, SupplierFormModel model)
        {
            if (!this.supplierService.Exists(id))
            {
                return this.RedirectToAction(nameof(All));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(SupplierFormView, model);
            }

            this.supplierService.Update(id, model.Name, model.IsImporter);

            return this.RedirectToAction(model.IsImporter ? nameof(Importers) : nameof(Local));
        }

        private SuppliersByTypeModel GetByType(string type)
        {
            var suppliers = this.supplierService.AllByType(type == nameof(Importers));

            return new SuppliersByTypeModel
            {
                Type = type,
                Suppliers = suppliers
            };
        }
    }
}
