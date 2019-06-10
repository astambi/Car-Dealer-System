namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Services.Models.Logs;

    public class LogService : ILogService
    {
        private readonly CarDealerDbContext db;

        public LogService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<LogListingModel> All()
        {
            var logs = this.db
                .Logs
                .Select(l => new LogListingModel
                {
                    User = l.User,
                    ModifiedTable = l.ModifiedTable,
                    Operation = l.Operation,
                    Time = l.Time
                })
                .ToList();

            return logs;
        }

        public void Clear()
        {
            var logs = this.db.Logs
                .Take(1) // todo
                .ToList();

            this.db.Logs.RemoveRange(logs);
            this.db.SaveChanges();
        }

        public int Total(string search)
            => this.db.Logs.Count();
    }
}
