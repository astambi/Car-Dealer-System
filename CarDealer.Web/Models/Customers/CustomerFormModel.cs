namespace CarDealer.Web.Models.Customers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using CarDealer.Web.Controllers;

    public class CustomerFormModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Young Driver")]
        public bool IsYoungDriver { get; set; }

        // Create, Delete, Edit
        public string Action { get; set; } = nameof(CustomersController.Create); // default action
    }
}
