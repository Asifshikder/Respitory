using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Respitory
{
    public interface IUserContext<T> where T : IdentityUser
    {
        Task<T> GetCurrentUserAsync();
    }
    public class UserContext<T> : IUserContext<T> where T : IdentityUser
    {
        private T _currentUser;

        private UserManager<T> _userManager;
        private HttpContext _httpContext;
        public UserContext(UserManager<T> userManager,
                           IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _httpContext = contextAccessor.HttpContext;
        }
        public async Task<T> GetCurrentUserAsync()
        {
            var contextUser = _httpContext.User;
            _currentUser = await _userManager.GetUserAsync(contextUser);

            if (_currentUser != null)
            {
                return _currentUser;
            }
            return _currentUser;
        }
    }
}
