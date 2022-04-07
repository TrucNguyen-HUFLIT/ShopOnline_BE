using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Core.Models.Enum
{
    public enum ErrorCode
    {
        ErrorCode = 400,

        [Display(GroupName = "Common")]
        [Description("Cannot delete this, please check the items related to this")]
        CannotDelete = ErrorCode + 1,

        #region Account = ErrorCode + 1000
        Account = ErrorCode + 1000,

        //[Display(GroupName = "Account")]
        //[Description("Bad Request")]
        //BadRequest = Account,

        //[Display(GroupName = "Account")]
        //[Description("UnAuthorized")]
        //UnAuthorized = Account + 1,

        //[Display(GroupName = "Account")]
        //[Description("Forbidden")]
        //Forbidden = Account + 3,

        //[Display(GroupName = "Account")]
        //[Description("Not Found")]
        //NotFound = Account + 4,

        [Display(GroupName = "Account")]
        [Description("Email isn't incorrect")]
        WrongEmail = Account + 1,

        [Display(GroupName = "Account")]
        [Description("Password isn't incorrect")]
        WrongPassword = Account + 2,

        [Display(GroupName = "Account")]
        [Description("Phone number doesn't match")]
        PhoneNotMatch = Account + 3,

        [Display(GroupName = "Account")]
        [Description("Email already exists")]
        EmailExisted = Account + 4,

        [Display(GroupName = "Account")]
        [Description("Email doesn't exist")]
        EmailNotExisted = Account + 5,

        [Display(GroupName = "Account")]
        [Description("The server does not respond, please try again later!")]
        NotResponse = Account + 6,

        #endregion

        #region Cart = ErrorCode + 2000
        Cart = ErrorCode + 2000,

        [Display(GroupName = "Cart")]
        [Description("Out of stock. We have reduced your cart.")]
        OutOfStock = Cart + 1,

        [Display(GroupName = "Cart")]
        [Description("Not found this product in your cart")]
        NotFoundInCart = Cart + 2,

        [Display(GroupName = "Cart")]
        [Description("Your cart is empty")]
        EmptyCart = Cart + 3,

        #endregion

        #region User = ErrorCode + 3000
        User = ErrorCode + 3000,

        [Display(GroupName = "User")]
        [Description("Old Password is not correct")]
        OldPasswordNotCorrect = User + 1,

        [Display(GroupName = "User")]
        [Description("Not found user")]
        NotFoundUser = Cart + 1,

        #endregion

        #region Brand = ErrorCode + 4000
        Brand = ErrorCode + 4000,
        [Display(GroupName = "Brand")]
        [Description("Brand already exists")]
        BrandExisted = Brand + 1,

        [Display(GroupName = "Brand")]
        [Description("Brand doesn't exist")]
        BrandNotExisted = Brand + 2,
        #endregion

        #region ProductType = ErrorCode + 5000
        ProductType = ErrorCode + 5000,
        [Display(GroupName = "ProductType")]
        [Description("Product Type already exists")]
        ProductTypeExisted = ProductType + 1,

        [Display(GroupName = "ProductType")]
        [Description("Product Type doesn't exist")]
        ProductTypeNotExisted = ProductType + 2,

        #endregion

        #region ProductDetail = ErrorCode + 6000
        ProductDetail = ErrorCode + 6000,
        [Display(GroupName = "ProductDetail")]
        [Description("Product Detail already exists")]
        ProductDetailExisted = ProductDetail + 1,

        [Display(GroupName = "ProductType")]
        [Description("Product Detail doesn't exist")]
        ProductDetailNotExisted = ProductDetail + 2,

        #endregion

        #region Product = ErrorCode + 6000
        Product = ErrorCode + 6000,
        [Display(GroupName = "Product")]
        [Description("Product already exists")]
        ProductExisted = Product + 1,

        [Display(GroupName = "Product")]
        [Description("Product doesn't exist")]
        ProductNotExisted = Product + 2,

        [Display(GroupName = "Product")]
        [Description("Cannot delete this, please check the orders related to this")]
        CannotDeleteProduct = Product + 3,

        #endregion
    }
}
