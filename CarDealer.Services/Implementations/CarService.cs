namespace CarDealer.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Services.Models.Cars;
    using CarDealer.Services.Models.Parts;
    using Microsoft.EntityFrameworkCore;

    public class CarService : ICarService
    {
        private readonly CarDealerDbContext db;

        public CarService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CarBasicModel> AllBasic()
            => throw new NotImplementedException();

        public IEnumerable<CarModel> AllByMake(string make)
        {
            var cars = this.db.Cars.AsQueryable(); // make == null => all cars

            if (make != null)
            {
                cars = cars.Where(c => c.Make.ToLower() == make.ToLower());
            }

            return cars
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new CarModel
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToList();
        }

        public IEnumerable<CarWithPartsModel> AllWithParts()
            => this.db
            .Cars
            .OrderByDescending(c => c.Id)
            .Select(c => new CarWithPartsModel
            {
                Make = c.Make,
                Model = c.Model,
                TravelledDistance = c.TravelledDistance,
                Parts = c.Parts
                    .Select(p => new PartModel
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                    .ToList()
            })
            .ToList();

        public void Create(string make, string model, long travelledDistance, IEnumerable<int> selectedPartIds)
        {
            var car = new Car
            {
                Make = make,
                Model = model,
                TravelledDistance = travelledDistance
            };

            foreach (var partId in selectedPartIds)
            {
                car.Parts.Add(new PartCar { PartId = partId });
            }

            this.db.Cars.Add(car);
            this.db.SaveChanges();
        }

        public bool Exists(int id)
            => this.db.Cars.Any(c => c.Id == id);

        public CarEditModel GetById(int id)
            => this.db
            .Cars
            .Where(c => c.Id == id)
            .Select(c => new CarEditModel
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                TravelledDistance = c.TravelledDistance,
                Parts = c.Parts.Select(cp => cp.PartId).ToList()
            })
            .FirstOrDefault();

        public void Remove(int id)
        {
            var car = this.db.Cars.Find(id);

            if (car == null)
            {
                return;
            }

            this.db.Cars.Remove(car);
            this.db.SaveChanges();
        }

        public void Update(
            int id,
            string make,
            string model,
            long travelledDistance,
            IEnumerable<int> selectedParts)
        {
            var car = this.db.Cars
                .Include(c => c.Parts)
                .Where(c => c.Id == id)
                .FirstOrDefault();

            if (car == null)
            {
                return;
            }

            car.Make = make;
            car.Model = model;
            car.TravelledDistance = travelledDistance;

            // Update car parts
            var currentPartIds = car.Parts.Select(pc => pc.PartId).ToList();

            var partIdsToRemove = currentPartIds.Except(selectedParts).ToList();
            var partIdsToAdd = selectedParts.Except(currentPartIds).ToList();

            var validPartIdsToAdd = this.db.Parts.Select(p => p.Id)
                .Intersect(partIdsToAdd)
                .ToList();

            // Remove parts
            foreach (var partIdToRemove in partIdsToRemove)
            {
                var part = car.Parts.FirstOrDefault(p => p.PartId == partIdToRemove);
                car.Parts.Remove(part);
            }

            // Add parts
            foreach (var partIdToAdd in validPartIdsToAdd)
            {
                car.Parts.Add(new PartCar { PartId = partIdToAdd });
            }

            this.db.Cars.Update(car);
            this.db.SaveChanges();
        }
    }
}