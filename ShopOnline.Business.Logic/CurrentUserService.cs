using Microsoft.AspNetCore.Http;
using ShopOnline.Core.Helper;
using ShopOnline.Core.Models;
using System;

namespace ShopOnline.Business.Logic
{
    public class CurrentUserService : ICurrentUserService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private CurrentUser _currentUser = null;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUser Current
        {
            get
            {
                if (_currentUser == null)
                {
                    string jwtToken = null;
                    var headerRequestToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

                    // get token from request header
                    if (headerRequestToken != null && headerRequestToken.StartsWith("Bearer" + " "))
                    {
                        jwtToken = headerRequestToken.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                    }

                    // get token from query string
                    if (string.IsNullOrEmpty(jwtToken))
                    {
                        jwtToken = _httpContextAccessor.HttpContext?.Request.Query["access_token"].ToString();
                    }

                    if (string.IsNullOrEmpty(jwtToken) == false)
                    {
                        _currentUser = TokenHelper.GetUserInfo(jwtToken);
                    }
                }

                return _currentUser;
            }
        }
    }
}
