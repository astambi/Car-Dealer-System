namespace CarDealer.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Services.Models.Suppliers;

    public class SupplierService : ISupplierService
    {
        private readonly CarDealerDbContext db;

        public SupplierService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SupplierListingModel> AllByType(bool isImporter)
            => this.db
            .Suppliers
            .OrderByDescending(s => s.Id)
            .Where(s => s.IsImporter == isImporter)
            .Select(s => new SupplierListingModel
            {
                Id = s.Id,
                Name = s.Name,
                TotalParts = s.Parts.Count
            })
            .ToList();

        public IEnumerable<SupplierModel> AllDropDown()
            => this.db
            .Suppliers
            .OrderBy(s => s.Name)
            .Select(s => new SupplierModel
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToList();

        public bool Exists(int id)
            => this.db.Suppliers.Any(s => s.Id == id);
    }
}
