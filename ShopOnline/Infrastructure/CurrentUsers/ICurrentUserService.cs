using ShopOnline.Infrastructure.CurrentUsers;

namespace ShopOnline.Infrastructure
{
    public interface ICurrentUserService
    {
        CurrentUser Current { get; }
    }
}
