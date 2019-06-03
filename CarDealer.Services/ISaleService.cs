namespace CarDealer.Services
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Sales;

    public interface ISaleService
    {
        IEnumerable<SaleModel> All();

        IEnumerable<SaleModel> Discounted(int? discount);

        SaleModel ById(int id);

        //SaleReviewModel SaleReview(int carId, int customerId, double discount);

        void Create(int customerId, int carId, double discount);
    }
}
