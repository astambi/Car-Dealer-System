namespace CarDealer.Web.Models.Parts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using CarDealer.Common.Mapping;
    using CarDealer.Services.Models.Parts;
    using CarDealer.Web.Controllers;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PartFormModel : IMapFrom<PartEditDeleteModel>
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 symbols.")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price should be a positive number.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity should be at least 1.")]
        public int Quantity { get; set; } = 1; // default value

        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        [IgnoreMap]
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        // Create, Delete, Edit
        [IgnoreMap]
        public string Action { get; set; } = nameof(PartsController.Create); // default action
    }
}
