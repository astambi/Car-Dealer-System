namespace CarDealer.Web.Controllers
{
    using AutoMapper;
    using CarDealer.Services;
    using CarDealer.Web.Infrastructure.Filters;
    using CarDealer.Web.Models.Suppliers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class SuppliersController : Controller
    {
        private const string SuppliersView = "Suppliers";
        private const string SupplierFormView = "SupplierForm";

        private readonly ISupplierService supplierService;
        private readonly IMapper mapper;

        public SuppliersController(ISupplierService supplierService, IMapper mapper)
        {
            this.supplierService = supplierService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        public IActionResult Local()
        {
            var model = this.GetByType(nameof(Local));
            return this.View(SuppliersView, model);
        }

        [AllowAnonymous]
        public IActionResult Importers()
        {
            var model = this.GetByType(nameof(Importers));
            return this.View(SuppliersView, model);
        }

        public IActionResult Create() 
            => this.View(SupplierFormView, new SupplierFormModel());

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

        public IActionResult Delete(int id) 
            => this.LoadEditDeleteForm(id, nameof(Delete));

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

        public IActionResult Edit(int id) 
            => this.LoadEditDeleteForm(id, nameof(Edit));

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
            => new SuppliersByTypeModel
            {
                Type = type,
                Suppliers = this.supplierService.AllByType(type == nameof(Importers))
            };

        private IActionResult LoadEditDeleteForm(int id, string action)
        {
            if (!this.supplierService.Exists(id))
            {
                return this.RedirectToAction(nameof(All));
            }

            var supplier = this.supplierService.GetById(id);

            var model = this.mapper.Map<SupplierFormModel>(supplier);
            model.Action = action;

            return this.View(SupplierFormView, model);
        }
    }
}
