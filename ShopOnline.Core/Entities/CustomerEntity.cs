using System.Collections.Generic;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Entities
{
    public class CustomerEntity : BaseEntity, IBaseUserEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public TypeAcc TypeAcc { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; }
        public virtual ICollection<ReviewDetailEntity> ReviewDetails { get; set; }
        public virtual ICollection<FavoriteProductEntity> FavoriteProducts { get; set; }

    }
}
