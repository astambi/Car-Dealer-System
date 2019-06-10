namespace CarDealer.Services
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Logs;

    public interface ILogService
    {
        IEnumerable<LogListingModel> All();

        //IEnumerable<LogListingModel> All(string search, int page, int pageSize);

        void Clear();

        int Total(string search);
    }
}
