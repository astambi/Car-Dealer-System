namespace CarDealer.Web.Models.Logs
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Logs;

    public class LogPageListingModel
    {
        public IEnumerable<LogListingModel> Logs { get; set; }

        public PaginationModel Pagination { get; set; }
    }
}
