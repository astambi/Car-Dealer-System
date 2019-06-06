namespace CarDealer.Web.Infrastructure.Helpers
{
    public static class CssHelpers
    {
        public static string GetColor(string actionName)
        {
            switch (actionName.ToLower())
            {
                case "create": return WebConstants.CssColorCreate;
                case "edit": return WebConstants.CssColorEdit;
                case "delete": return WebConstants.CssColorDelete;
                case "details": return WebConstants.CssColorDetails;
                default: return WebConstants.CssColorDefault;
            }
        }
    }
}
