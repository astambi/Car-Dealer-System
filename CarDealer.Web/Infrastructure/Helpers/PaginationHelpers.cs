namespace CarDealer.Web.Infrastructure.Helpers
{
    using System;

    public static class PaginationHelpers
    {
        public static int GetTotalPages(int itemsCount, int pageSize = WebConstants.PageSize)
            => Math.Max(1, (int)Math.Ceiling(itemsCount / (double)pageSize));
    }
}
