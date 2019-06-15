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
            => this.SalesAsQuerable()
            .OrderByDescending(s => s.Id)
            .ToList();

        public IEnumerable<SaleModel> AllDiscounted(int? discount)
        {
            var salesAsQuerable = discount == null
                ? this.SalesAsQuerable().Where(s => s.Discount != 0).AsQueryable()
                : this.SalesAsQuerable().Where(s => Math.Abs(s.Discount * 100) == discount).AsQueryable();

            return salesAsQuerable
                .OrderByDescending(s => s.Id)
                .ToList();
        }

        public SaleModel ById(int id)
            => this.SalesAsQuerable().FirstOrDefault(s => s.Id == id);

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

        private IQueryable<SaleModel> SalesAsQuerable()
            => this.db
            .Sales
            .Select(s => new SaleModel
            {
                Id = s.Id,
                Discount = s.Discount, // total discount, incl. young driver
                Customer = s.Customer.Name,
                Make = s.Car.Make,
                Model = s.Car.Model,
                TravelledDistance = s.Car.TravelledDistance,
                Price = s.Car.Parts.Sum(p => p.Part.Price)
            })
            .AsQueryable();
    }
}
