namespace CarDealer.Web.Controllers
{
    using CarDealer.Services;
    using CarDealer.Web.Models.Suppliers;
    using Microsoft.AspNetCore.Mvc;

    public class SuppliersController : Controller
    {
        private const string SuppliersView = "Suppliers";

        private readonly ISupplierService supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            this.supplierService = supplierService;
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
