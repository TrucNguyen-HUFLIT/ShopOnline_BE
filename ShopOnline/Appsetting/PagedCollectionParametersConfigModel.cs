namespace ShopOnline.Appsetting
{
    public class PagedCollectionParametersConfigModel
    {
        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 10;

        public int MaxTake { get; set; } = 10000;

        public string Terms { get; set; } = string.Empty;
    }
}
