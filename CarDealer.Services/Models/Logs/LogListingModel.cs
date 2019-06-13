namespace CarDealer.Services.Models.Logs
{
    using System;
    using CarDealer.Common.Mapping;
    using CarDealer.Data.Models;

    public class LogListingModel : IMapFrom<Log>
    {
        public string User { get; set; }

        public string Operation { get; set; }

        public string ModifiedTable { get; set; }

        public DateTime Time { get; set; }
    }
}
