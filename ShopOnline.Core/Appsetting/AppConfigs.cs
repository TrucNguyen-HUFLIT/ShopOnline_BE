namespace ShopOnline.Core.Appsetting
{
    public static class AppConfigs
    {
        public static ConnectionStrings ConnectionStrings { get; set; }
        public static BearerToken BearerToken { get; set; }
        public static PagedCollectionParametersConfigModel PagedCollectionParameters { get; set; } = new PagedCollectionParametersConfigModel();
    }
}
