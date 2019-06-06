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

        public IEnumerable<SaleModel> Discounted(int? discount)
        {
            var salesAsQuerable =
                discount == null
                ? this.db.Sales
                    .Where(s => s.Discount != 0) // any discount
                    .AsQueryable()
                : this.db.Sales
                    .Where(s => this.CalcTotalDiscount(s.Discount, s.Customer.IsYoungDriver) * 100 == discount)
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
                Discount = this.CalcTotalDiscount(s.Discount, s.Customer.IsYoungDriver),
                Id = s.Id
            })
            .FirstOrDefault();

        //public SaleReviewModel SaleReview(int carId, int customerId, double discount) => throw new NotImplementedException();

        public void Create(int customerId, int carId, double discount)
            => throw new NotImplementedException();

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
                Discount = this.CalcTotalDiscount(s.Discount, s.Customer.IsYoungDriver),
                Id = s.Id
            })
            .ToList();

        private double CalcTotalDiscount(double discount, bool isYoungDriver)
            => Math.Min(1,
                discount + (isYoungDriver ? ServicesConstants.YoungDriverDiscount : 0));
    }
}
