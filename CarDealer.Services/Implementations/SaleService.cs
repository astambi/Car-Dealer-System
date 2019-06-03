namespace CarDealer.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using CarDealer.Data;
    using CarDealer.Services.Models.Sales;

    public class SaleService : ISaleService
    {
        private readonly CarDealerDbContext db;

        public SaleService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SaleModel> All() => throw new NotImplementedException();

        public IEnumerable<SaleModel> Discounted(int? discount) => throw new NotImplementedException();

        public SaleModel ById(int id) => throw new NotImplementedException();

        //public SaleReviewModel SaleReview(int carId, int customerId, double discount) => throw new NotImplementedException();

        public void Create(int customerId, int carId, double discount) => throw new NotImplementedException();
    }
}
