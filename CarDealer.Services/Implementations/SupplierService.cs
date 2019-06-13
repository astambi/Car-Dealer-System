namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Services.Models.Suppliers;

    public class SupplierService : ISupplierService
    {
        private readonly CarDealerDbContext db;
        private readonly IMapper mapper;

        public SupplierService(CarDealerDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public IEnumerable<SupplierListingModel> All()
            => this.db
            .Suppliers
            .OrderByDescending(s => s.Id)
            .Select(s => new SupplierListingModel
            {
                Id = s.Id,
                Name = s.Name,
                TotalParts = s.Parts.Count
            })
            .ToList();

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
            .Select(s => this.mapper.Map<SupplierModel>(s))
            .ToList();

        public void Create(string name, bool isImporter)
        {
            var supplier = new Supplier
            {
                Name = name,
                IsImporter = isImporter
            };

            this.db.Suppliers.Add(supplier);
            this.db.SaveChanges();
        }

        public bool Exists(int id)
            => this.db.Suppliers.Any(s => s.Id == id);

        public SupplierWithTypeModel GetById(int id)
            => this.db
            .Suppliers
            .Where(s => s.Id == id)
            .Select(s => this.mapper.Map<SupplierWithTypeModel>(s))
            .FirstOrDefault();

        public void Remove(int id)
        {
            var supplier = this.db.Suppliers.Find(id);

            if (supplier == null)
            {
                return;
            }

            this.RemoveSupplierParts(id);

            this.db.Suppliers.Remove(supplier);
            this.db.SaveChanges();
        }

        public void Update(int id, string name, bool isImporter)
        {
            var supplier = this.db.Suppliers.Find(id);

            if (supplier == null)
            {
                return;
            }

            supplier.Name = name;
            supplier.IsImporter = isImporter;

            //this.db.Suppliers.Update(supplier);
            this.db.SaveChanges();
        }

        private void RemoveSupplierParts(int id)
        {
            var parts = this.db
                .Parts
                .Where(p => p.SupplierId == id)
                .ToList();

            this.db.Parts.RemoveRange(parts);
        }
    }
}
