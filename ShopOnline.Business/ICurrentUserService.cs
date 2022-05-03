using ShopOnline.Core.Models;

namespace ShopOnline.Business
{
    public interface ICurrentUserService
    {
        CurrentUser Current { get; }
    }
}
