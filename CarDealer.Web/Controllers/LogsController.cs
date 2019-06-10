namespace CarDealer.Web.Controllers
{
    using System;
    using CarDealer.Services;
    using CarDealer.Web.Infrastructure.Helpers;
    using CarDealer.Web.Models;
    using CarDealer.Web.Models.Logs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class LogsController : Controller
    {
        private readonly ILogService logService;

        public LogsController(ILogService logService)
        {
            this.logService = logService;
        }

        [Authorize]
        public IActionResult All(string search = null, int currentPage = 1)
        {
            var logsCurrentPage = this.logService.All(search, currentPage, WebConstants.PageSize);
            var logsTotal = this.logService.Total(search);

            var model = new LogPageListingModel
            {
                Logs = logsCurrentPage,
                Pagination = new PaginationModel
                {
                    Controller = WebConstants.LogsControllerName,
                    Action = nameof(All),
                    SearchTerm = search,
                    CurrentPage = currentPage,
                    TotalPages = PaginationHelpers.GetTotalPages(logsTotal)
                }
            };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Clear()
        {
            this.logService.Clear();

            return this.RedirectToAction(nameof(All));
        }
    }
}