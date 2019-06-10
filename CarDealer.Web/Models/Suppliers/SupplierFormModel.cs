namespace CarDealer.Web.Models.Suppliers
{
    using System.ComponentModel.DataAnnotations;
    using CarDealer.Web.Controllers;

    public class SupplierFormModel
    {
        [Required]
        [MaxLength(100, ErrorMessage = "{0} should contain up to {1} symbols.")]
        public string Name { get; set; }

        [Display(Name = "Is importer")]
        public bool IsImporter { get; set; }

        // Create, Delete, Edit
        public string Action { get; set; } = nameof(SuppliersController.Create); // default action
    }
}
