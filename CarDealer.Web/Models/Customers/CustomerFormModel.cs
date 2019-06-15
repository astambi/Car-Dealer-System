namespace CarDealer.Web.Models.Customers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using CarDealer.Common.Mapping;
    using CarDealer.Services.Models.Customers;
    using CarDealer.Web.Controllers;

    public class CustomerFormModel : IMapFrom<CustomerModel>
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
        [IgnoreMap]
        public string Action { get; set; } = nameof(CustomersController.Create); // default action
    }
}
