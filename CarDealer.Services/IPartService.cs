namespace CarDealer.Services
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Parts;

    public interface IPartService
    {
        IEnumerable<PartListingModel> All(int page = 1, int pageSize = ServicesConstants.PageSize);

        //IEnumerable<PartBasicModel> AllBasic();

        IEnumerable<PartBasicModel> AllDropdown();

        void Create(string name, decimal price, int quantity, int supplierId);

        bool Exists(int id);

        PartEditDeleteModel GetById(int id);

        void Remove(int id);

        int Total();

        void Update(int id, decimal price, int quantity);
    }
}
