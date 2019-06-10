namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Services.Models.Logs;

    public class LogService : ILogService
    {
        private readonly CarDealerDbContext db;

        public LogService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<LogListingModel> All(string search, int page, int pageSize)
            => this.GetLogsAsQuerable(search)
            .OrderBy(l => l.Time)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new LogListingModel
            {
                User = l.User,
                ModifiedTable = l.ModifiedTable,
                Operation = l.Operation,
                Time = l.Time
            })
            .ToList();

        public void Clear()
        {
            var logs = this.db.Logs.ToList();

            this.db.Logs.RemoveRange(logs);
            this.db.SaveChanges();
        }

        public int Total(string search)
            => this.GetLogsAsQuerable(search).Count();

        private IQueryable<Log> GetLogsAsQuerable(string search)
        {
            var logsAsQuerable = this.db.Logs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                logsAsQuerable = logsAsQuerable.Where(l => l.User.ToLower().Contains(search.Trim().ToLower()));
            }

            return logsAsQuerable;
        }
    }
}
