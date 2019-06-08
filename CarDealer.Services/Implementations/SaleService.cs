namespace CarDealer.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Services.Models.Sales;

    public class SaleService : ISaleService
    {
        private readonly CarDealerDbContext db;

        public SaleService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SaleModel> All()
        {
            var salesAsQuerable = this.db.Sales.AsQueryable();
            return this.MapToSaleModel(salesAsQuerable);
        }

        public IEnumerable<SaleModel> AllDiscounted(int? discount)
        {
            var salesAsQuerable =
                discount == null
                ? this.db.Sales
                    .Where(s => s.Discount != 0) // any discount
                    .AsQueryable()
                : this.db.Sales
                    //.Where(s => this.CalcTotalDiscount(s.Discount, s.Customer.IsYoungDriver) * 100 == discount)
                    .Where(s => Math.Abs(s.Discount * 100) == discount) // total discount, incl. young driver
                    .AsQueryable();

            return this.MapToSaleModel(salesAsQuerable);
        }

        public SaleModel ById(int id)
            => this.db
            .Sales
            .Where(s => s.Id == id)
            .Select(s => new SaleModel
            {
                Make = s.Car.Make,
                Model = s.Car.Model,
                TravelledDistance = s.Car.TravelledDistance,
                Customer = s.Customer.Name,
                Price = s.Car.Parts.Sum(p => p.Part.Price),
                //Discount = this.CalcTotalDiscount(s.Discount, s.Customer.IsYoungDriver),
                Discount = s.Discount, // total discount, incl. young driver
                Id = s.Id
            })
            .FirstOrDefault();

        public void Create(int customerId, int carId, double discount) // [0, 100]
        {
            var sale = new Sale
            {
                CustomerId = customerId,
                CarId = carId,
                Discount = discount / 100 // [0, 1]
            };

            this.db.Sales.Add(sale);
            this.db.SaveChanges();
        }

        private IEnumerable<SaleModel> MapToSaleModel(IQueryable<Sale> salesAsQuerable)
            => salesAsQuerable
            .OrderByDescending(s => s.Id)
            .Select(s => new SaleModel
            {
                Make = s.Car.Make,
                Model = s.Car.Model,
                TravelledDistance = s.Car.TravelledDistance,
                Customer = s.Customer.Name,
                Price = s.Car.Parts.Sum(p => p.Part.Price),
                //Discount = this.CalcTotalDiscount(s.Discount, s.Customer.IsYoungDriver),
                Discount = s.Discount, // total discount, incl. young driver
                Id = s.Id
            })
            .ToList();

        private double CalcTotalDiscount(double discount, bool isYoungDriver)
            => Math.Min(1, discount + (isYoungDriver ? ServicesConstants.YoungDriverDiscount : 0));
    }
}
