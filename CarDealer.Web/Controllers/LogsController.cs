namespace CarDealer.Web.Controllers
{
    using CarDealer.Services;
    using CarDealer.Web.Infrastructure.Helpers;
    using CarDealer.Web.Models;
    using CarDealer.Web.Models.Logs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class LogsController : Controller
    {
        private readonly ILogService logService;

        public LogsController(ILogService logService)
        {
            this.logService = logService;
        }

        public IActionResult All(string search = null, int currentPage = 1)
        {
            var logsTotal = this.logService.Total(search);

            var totalPages = PaginationHelpers.GetTotalPages(logsTotal);
            currentPage = PaginationHelpers.GetValidCurrentPage(currentPage, totalPages);

            var logsCurrentPage = this.logService.All(search, currentPage, WebConstants.PageSize);

            var model = new LogPageListingModel
            {
                Logs = logsCurrentPage,
                Pagination = new PaginationModel
                {
                    Controller = WebConstants.LogsControllerName,
                    Action = nameof(All),
                    SearchTerm = search,
                    CurrentPage = currentPage,
                    TotalPages = totalPages
                }
            };

            return this.View(model);
        }

        public IActionResult Clear()
        {
            this.logService.Clear();

            return this.RedirectToAction(nameof(All));
        }
    }
}