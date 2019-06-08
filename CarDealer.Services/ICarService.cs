namespace CarDealer.Services
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Cars;

    public interface ICarService
    {
        IEnumerable<CarModel> AllByMake(string make);

        IEnumerable<CarModel> AllDropdown();

        IEnumerable<CarWithPartsModel> AllWithParts();

        void Create(string make, string model, long travelledDistance, IEnumerable<int> selectedPartIds);

        bool Exists(int id);

        CarEditModel GetById(int id);

        CarWithPrice GetByIdWithPrice(int id);

        void Remove(int id);

        void Update(int id, string make, string model, long travelledDistance, IEnumerable<int> parts);
    }
}
