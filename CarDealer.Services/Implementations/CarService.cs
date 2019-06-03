namespace CarDealer.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Services.Models.Cars;
    using CarDealer.Services.Models.Parts;

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
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToList();
        }

        public IEnumerable<CarWithPartsModel> AllWithParts() 
            => this.db
            .Cars
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

        public void Create(string make, string model, long travelledDistance, IEnumerable<int> selectedPartIds) => throw new NotImplementedException();
    }
}
