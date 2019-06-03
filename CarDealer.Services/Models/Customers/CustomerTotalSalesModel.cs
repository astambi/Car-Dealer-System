namespace CarDealer.Services.Models.Customers
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Services.Models.Cars;

    public class CustomerTotalSalesModel
    {
        public string Name { get; set; }

        public List<CarPriceModel> CarSales { get; set; }

        public decimal TotalMoneySpent => this.CarSales.Sum(s => s.Price * (decimal)(1 - s.Discount));

        public int CarsBought => this.CarSales.Count;
    }
}
