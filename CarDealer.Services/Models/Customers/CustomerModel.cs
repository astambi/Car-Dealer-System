namespace CarDealer.Services.Models.Customers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using CarDealer.Common.Mapping;
    using CarDealer.Data.Models;

    public class CustomerModel : IMapFrom<Customer>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Is young driver")]
        public bool IsYoungDriver { get; set; }
    }
}
