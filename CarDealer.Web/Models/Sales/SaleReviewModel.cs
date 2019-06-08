namespace CarDealer.Web.Models.Sales
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SaleReviewModel
    {
        public int CustomerId { get; set; }

        public string Customer { get; set; }

        public int CarId { get; set; }

        public string Car { get; set; }

        public decimal Price { get; set; }

        public double Discount { get; set; }

        [Display(Name = "Additional discount")]
        public double AdditionalDiscount { get; set; }

        [Display(Name = "Total discount")]
        public double TotalDiscount
            => Math.Min(100, this.Discount + this.AdditionalDiscount);

        [Display(Name = "Net price")]
        public decimal NetPrice
            => this.Price * (1 - (decimal)this.TotalDiscount / 100);
    }
}
