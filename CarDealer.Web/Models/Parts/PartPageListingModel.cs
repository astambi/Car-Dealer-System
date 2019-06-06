namespace CarDealer.Web.Models.Parts
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Parts;

    public class PartPageListingModel
    {
        public IEnumerable<PartListingModel> Parts { get; set; }


        public PaginationModel Pagination { get; set; }
    }
}
