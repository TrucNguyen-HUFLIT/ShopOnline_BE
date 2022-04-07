using EnumsNET;
using ShopOnline.Core.Models.Enum;
using System;

namespace ShopOnline.Core.Exceptions
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(ErrorCode errorCode)
            : base(errorCode.AsString(EnumFormat.Description))
        {

        }

        public UserFriendlyException(string mess)
            : base(mess)
        {

        }
    }
}
