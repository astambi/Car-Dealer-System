namespace CarDealer.Web.Models.Cars
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CarDealer.Web.Controllers;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CarFormModel
    {
        [Required]
        [MaxLength(50)]
        public string Make { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        [Display(Name = "Travelled distance")]
        [Range(0, long.MaxValue, ErrorMessage = "{0} should be a positive number.")]
        public long TravelledDistance { get; set; }

        public IEnumerable<int> Parts { get; set; } = new List<int>();
        public IEnumerable<SelectListItem> PartsSelectList { get; set; }

        // Create, Delete, Edit
        public string Action { get; set; } = nameof(CarsController.Create); // default action
    }
}
