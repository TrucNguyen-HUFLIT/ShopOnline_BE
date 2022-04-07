
namespace ShopOnline.Core.Models.Enum
{
    public class AppEnum
    {
        public enum TypeAcc
        {
            Admin = 1,
            Staff = 2,
            Customer = 3,
            Shipper = 4,
            Manager = 5,
        }

        public enum StatusOrder
        {
            Processing = 1,
            Accepted = 2,
            Delivering = 3,
            Completed = 4,
            Cancelled = 5,
        }

        public enum ProductSize
        {
            Size_35 = 35,
            Size_36 = 36,
            Size_37 = 37,
            Size_38 = 38,
            Size_39 = 39,
            Size_40 = 40,
            Size_41 = 41,
            Size_42 = 42,
            Size_43 = 43,
            Size_44 = 44,
            Size_45 = 45,
            Size_46 = 46,
        }

        public enum ProductStatus
        {
            Available = 1,
            Unavailable = 2,
        }

        public enum ReviewStatus
        {
            Waiting = 1,
            Approved = 2,
            Rejected = 3,
        }

        public enum PaymentMethod
        {
            ShipCod = 1,
            EWallet = 2,
            BankTransfer = 3,
        }
    }
}
