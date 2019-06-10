namespace CarDealer.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Services.Models;
    using CarDealer.Services.Models.Cars;
    using CarDealer.Services.Models.Customers;

    public class CustomerService : ICustomerService
    {
        private readonly CarDealerDbContext db;

        public CustomerService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CustomerBasicModel> AllDropdown()
            => this.db
            .Customers
            .OrderBy(c => c.Name)
            .Select(c => new CustomerBasicModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();

        public IEnumerable<CustomerModel> AllOrdered(OrderDirection order)
        {
            var customersQuery = this.db.Customers.AsQueryable();

            switch (order)
            {
                case OrderDirection.Ascending:
                    customersQuery = customersQuery
                        .OrderBy(c => c.BirthDate)
                        .ThenBy(c => c.IsYoungDriver);
                    break;
                case OrderDirection.Descending:
                    customersQuery = customersQuery
                        .OrderByDescending(c => c.BirthDate)
                        .ThenBy(c => c.IsYoungDriver);
                    break;
                default:
                    throw new InvalidOperationException($"{ServicesConstants.InvalidOrder}: {order}");
            }

            return customersQuery
                .Select(c => new CustomerModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();
        }

        public void Create(string name, DateTime birthDate, bool isYoungDriver)
        {
            var customer = new Customer
            {
                Name = name,
                BirthDate = birthDate,
                IsYoungDriver = isYoungDriver
            };

            this.db.Customers.Add(customer);
            this.db.SaveChanges();
        }

        public bool Exists(int id)
            => this.db.Customers.Any(c => c.Id == id);

        public CustomerModel GetById(int id)
            => this.db
            .Customers
            .Where(c => c.Id == id)
            .Select(c => new CustomerModel
            {
                Id = c.Id,
                Name = c.Name,
                BirthDate = c.BirthDate,
                IsYoungDriver = c.IsYoungDriver
            })
            .FirstOrDefault();

        public CustomerAdditionalDiscount GetByIdWithAdditionalDiscount(int id)
            => this.db
            .Customers
            .Where(c => c.Id == id)
            .Select(c => new CustomerAdditionalDiscount
            {
                Name = c.Name,
                AdditionalDiscount = c.IsYoungDriver
                    ? (int)(100 * ServicesConstants.YoungDriverDiscount)
                    : 0
            })
            .FirstOrDefault();

        public void Remove(int id)
        {
            if (!this.Exists(id))
            {
                return;
            }

            var customer = this.db.Customers.Find(id);
            this.db.Customers.Remove(customer);
            this.db.SaveChanges();
        }

        public CustomerTotalSalesModel TotalSalesById(int id)
            => this.db
            .Customers
            .Where(c => c.Id == id)
            .Select(c => new CustomerTotalSalesModel
            {
                Name = c.Name,
                CarSales = c.Sales
                    .Select(s => new CarPriceModel
                    {
                        Price = s.Car.Parts.Sum(p => p.Part.Price),
                        Discount = s.Discount
                            + (c.IsYoungDriver ? ServicesConstants.YoungDriverDiscount : 0)
                    })
                    .ToList()
            })
            .FirstOrDefault();

        public void Update(int id, string name, DateTime birthDate)
        {
            var customer = this.db.Customers.Find(id);

            if (customer == null)
            {
                return;
            }

            customer.Name = name;
            customer.BirthDate = birthDate;

            //this.db.Customers.Update(customer);
            this.db.SaveChanges();
        }
    }
}
