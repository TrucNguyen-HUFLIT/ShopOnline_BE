using ShopOnline.Core.Models.Enum;
using System.Collections.Generic;

namespace ShopOnline.Core.Entities
{
    public class ShipperEntity : BaseEntity, IBaseUserEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public AppEnum.TypeAcc TypeAcc { get; set; }
        public int Salary { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; }

    }
}
