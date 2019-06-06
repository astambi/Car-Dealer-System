namespace CarDealer.Services.Models.Customers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CustomerModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Is young driver")]
        public bool IsYoungDriver { get; set; }
    }
}
