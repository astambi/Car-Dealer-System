namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Services.Models.Logs;

    public class LogService : ILogService
    {
        private readonly CarDealerDbContext db;
        private readonly IMapper mapper;

        public LogService(CarDealerDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public IEnumerable<LogListingModel> All(string search, int page, int pageSize)
            => this.GetLogsAsQuerable(search)
            .OrderBy(l => l.Time)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => this.mapper.Map<LogListingModel>(l))           
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
