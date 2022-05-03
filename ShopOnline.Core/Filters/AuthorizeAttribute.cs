using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ShopOnline.Core.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeFilterAttribute : AuthorizeAttribute
    {
        public AuthorizeFilterAttribute()
        {
        }

        public AuthorizeFilterAttribute(params object[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("roles");

            Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
        }
    }
}
