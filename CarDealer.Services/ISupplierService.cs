namespace CarDealer.Services
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Suppliers;

    public interface ISupplierService
    {
        IEnumerable<SupplierListingModel> All();

        IEnumerable<SupplierListingModel> AllByType(bool isImporter);

        IEnumerable<SupplierModel> AllDropDown();

        void Create(string name, bool isImporter);

        bool Exists(int id);

        SupplierWithTypeModel GetById(int id);

        void Remove(int id);

        void Update(int id, string name, bool isImporter);
    }
}
