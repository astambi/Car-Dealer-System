namespace CarDealer.Web.Controllers
{
    using CarDealer.Services;
    using CarDealer.Web.Models.Cars;
    using Microsoft.AspNetCore.Mvc;

    public class CarsController : Controller
    {
        private const string All = "all";

        private readonly ICarService carService;

        public CarsController(ICarService carService)
        {
            this.carService = carService;
        }

        [Route("cars/{make?}")]
        public IActionResult AllByMake(string make)
        {
            var cars = this.carService.AllByMake(make);

            var model = new CarsByMakeModel
            {
                Make = make ?? All,
                Cars = cars
            };

            return this.View(model);
        }

        public IActionResult Parts()
        {
            var cars = this.carService.AllWithParts();
            return this.View(cars);
        }
    }
}
