namespace CarDealer.Services
{
    using System;
    using System.Collections.Generic;
    using CarDealer.Services.Models;
    using CarDealer.Services.Models.Customers;

    public interface ICustomerService
    {
        //IEnumerable<CustomerBasicModel> AllBasic();

        IEnumerable<CustomerModel> AllOrdered(OrderDirection order);

        void Create(string name, DateTime birthDate, bool isYoungDriver);

        bool Exists(int id);

        CustomerModel GetById(int id);

        void Remove(int id);

        CustomerTotalSalesModel TotalSalesById(int id);

        void Update(int id, string name, DateTime birthDate);
    }
}
