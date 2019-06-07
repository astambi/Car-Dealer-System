﻿namespace CarDealer.Services.Implementations
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

            this.AddCarParts(car, selectedPartIds);

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

            // Assuming car parts cannot be reused, thus not undating car part quantities
            this.db.Cars.Remove(car);
            this.db.SaveChanges();
        }

        public void Update(int id, string make, string model, long travelledDistance, 
            IEnumerable<int> selectedParts)
        {
            var car = this.db
                .Cars
                .Include(c => c.Parts) // with car parts
                .Where(c => c.Id == id)
                .FirstOrDefault();

            if (car == null)
            {
                return;
            }

            // Update car properties
            car.Make = make;
            car.Model = model;
            car.TravelledDistance = travelledDistance;

            // Get parts to update
            var currentPartIds = car.Parts.Select(pc => pc.PartId).ToList();
            var selectedPartIdsToAdd = selectedParts.Except(currentPartIds).ToList();
            var currentPartIdsToRemove = currentPartIds.Except(selectedParts).ToList();

            // Update car parts
            this.AddCarParts(car, selectedPartIdsToAdd);
            this.RemoveCarParts(car, currentPartIdsToRemove);

            this.db.Cars.Update(car);
            this.db.SaveChanges();
        }

        private void AddCarParts(Car car, IEnumerable<int> selectedPartIdsToAdd)
        {
            var partsToAdd = this.db
                .Parts
                .Where(p => selectedPartIdsToAdd.Contains(p.Id)) // parts to add from db
                .Where(p => p.Quantity != 0) // available only
                .ToList();

            foreach (var part in partsToAdd)
            {
                // Add part to car
                car.Parts.Add(new PartCar { PartId = part.Id });

                // Descrease part quantity
                part.Quantity--;
            }
        }

        private void RemoveCarParts(Car car, IEnumerable<int> partIdsToRemove)
        {
            var partsToRemove = this.db
                .Parts
                .Where(p => partIdsToRemove.Contains(p.Id))
                .ToList();

            foreach (var part in partsToRemove)
            {
                // Remove part from car
                var partCar = car.Parts.First(pc => pc.PartId == part.Id);
                car.Parts.Remove(partCar);

                // Increase part quantity
                part.Quantity++;
            }
        }
    }
}