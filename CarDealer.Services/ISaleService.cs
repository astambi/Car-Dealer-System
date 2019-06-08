namespace CarDealer.Services
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Sales;

    public interface ISaleService
    {
        IEnumerable<SaleModel> All();

        IEnumerable<SaleModel> AllDiscounted(int? discount);

        SaleModel ById(int id);

        void Create(int customerId, int carId, double discount); // discount [0, 100]
    }
}
