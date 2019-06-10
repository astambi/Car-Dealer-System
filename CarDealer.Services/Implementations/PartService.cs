namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Services.Models.Parts;
    using Microsoft.EntityFrameworkCore;

    public class PartService : IPartService
    {
        private readonly CarDealerDbContext db;

        public PartService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<PartListingModel> All(int page = 1, int pageSize = ServicesConstants.PageSize)
            => this.db
            .Parts
            .OrderByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PartListingModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                Supplier = p.Supplier.Name
            })
            .ToList();

        public IEnumerable<PartBasicModel> AllDropdown()
            => this.db
            .Parts
            .OrderBy(p => p.Name)
            .Select(p => new PartBasicModel
            {
                Id = p.Id,
                Name = p.Name,
                Quantity = p.Quantity
            })
            .ToList();

        public void Create(string name, decimal price, int quantity, int supplierId)
        {
            var part = new Part
            {
                Name = name,
                Price = price,
                Quantity = quantity,
                SupplierId = supplierId
            };

            this.db.Parts.Add(part);
            this.db.SaveChanges();
        }

        public bool Exists(int id)
            => this.db.Parts.Any(p => p.Id == id);

        public PartEditDeleteModel GetById(int id)
            => this.db
            .Parts
            .Where(p => p.Id == id)
            .Select(p => new PartEditDeleteModel
            {
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity
            })
            .FirstOrDefault();

        public void Remove(int id)
        {
            var part = this.db
                .Parts
                .Include(p => p.Cars)
                .Where(p => p.Id == id)
                .FirstOrDefault();

            if (part == null)
            {
                return;
            }

            if (part.Cars.Any())
            {
                part.Quantity = 0; // part is in use, cannot be removed
            }
            else
            {
                this.db.Parts.Remove(part); // no cars using the part
            }

            this.db.SaveChanges();
        }

        public int Total()
            => this.db.Parts.Count();

        public void Update(int id, decimal price, int quantity)
        {
            var part = this.db.Parts.Find(id);

            if (part == null)
            {
                return;
            }

            part.Price = price;
            part.Quantity = quantity;

            //this.db.Parts.Update(part);
            this.db.SaveChanges();
        }
    }
}
