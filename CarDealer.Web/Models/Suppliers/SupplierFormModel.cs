namespace CarDealer.Web.Models.Suppliers
{
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using CarDealer.Common.Mapping;
    using CarDealer.Services.Models.Suppliers;
    using CarDealer.Web.Controllers;

    public class SupplierFormModel : IMapFrom<SupplierWithTypeModel>
    {
        [Required]
        [MaxLength(100, ErrorMessage = "{0} should contain up to {1} symbols.")]
        public string Name { get; set; }

        [Display(Name = "Is importer")]
        public bool IsImporter { get; set; }

        // Create, Delete, Edit
        [IgnoreMap]
        public string Action { get; set; } = nameof(SuppliersController.Create); // default action
    }
}
