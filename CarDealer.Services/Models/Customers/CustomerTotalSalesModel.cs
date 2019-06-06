namespace CarDealer.Services.Models.Customers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CarDealer.Services.Models.Cars;

    public class CustomerTotalSalesModel
    {
        public string Name { get; set; }

        [Display(Name = "Car sales")]
        public List<CarPriceModel> CarSales { get; set; }

        [Display(Name = "Total money spent")]
        public decimal TotalMoneySpent
            => this.CarSales.Sum(s => s.Price * (decimal)(1 - s.Discount));

        [Display(Name = "Cars bought")]
        public int CarsBought
            => this.CarSales.Count;
    }
}
