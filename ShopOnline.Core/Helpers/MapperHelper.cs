using AutoMapper;
using ShopOnline.Core.Mappers;


namespace ShopOnline.Infrastructure.Helper
{
    public static class MapperHelper
    {
        public static MapperConfiguration MapperConfiguration = Init();

        public static MapperConfiguration Init()
        {
            var myAssembly = typeof(IAutoMapperProfile).Assembly;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(myAssembly);
            });
            var configuration = new MapperConfiguration(cfg => cfg.AddMaps(myAssembly));
            return configuration;
        }
    }
}
