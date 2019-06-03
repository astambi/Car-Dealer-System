﻿namespace CarDealer.Services
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Suppliers;

    public interface ISupplierService
    {
        //IEnumerable<SupplierWithTypeModel> All();

        IEnumerable<SupplierModel> AllByType(bool isImporter);

        IEnumerable<SupplierModel> AllDropDown();

        //bool Exists(int id);

        //void Create(string name, bool isImporter);

        //void Delete(int id);

        //SupplierWithTypeModel GetById(int id);

        //void Update(int id, string name, bool isImporter);
    }
}
