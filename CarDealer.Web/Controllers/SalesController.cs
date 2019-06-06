namespace CarDealer.Web.Controllers
{
    using CarDealer.Services;
    using CarDealer.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    [Route(WebConstants.SalesControllerName)]
    public class SalesController : Controller
    {
        private const string AllView = "all";

        private readonly ISaleService saleService;

        public SalesController(ISaleService saleService)
        {
            this.saleService = saleService;
        }

        [Route("")]
        public IActionResult All()
        {
            var sales = this.saleService.All();
            return this.View(sales);
        }

        [Route("{id}")]
        public IActionResult Details(int id)
        {
            var sales = this.saleService.ById(id);
            return this.ViewOrRedirect(sales);
        }

        [Route("discounted/{discount?}")]
        public IActionResult Discounted(int? discount) // in percentage
        {
            var sales = this.saleService.Discounted(discount);
            return this.View(AllView, sales);
        }
    }
}
