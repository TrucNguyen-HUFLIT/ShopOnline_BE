using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Infrastructure.CurrentUsers
{
    public class CurrentUser
    {
        public int UserId { get; set; }

        public TypeAcc TypeAcc { get; set; }
    }
}
