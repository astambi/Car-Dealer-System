namespace CarDealer.Web.Models.Sales
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SaleFormModel
    {
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }

        [Display(Name = "Car")]
        public int CarId { get; set; }
        public IEnumerable<SelectListItem> Cars { get; set; }

        [Range(0, 100, ErrorMessage = "{0} should be between {1} and {2}.")]
        public double Discount { get; set; }
    }
}
