using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentRegistration.AdminApp.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected bool IsLoggedIn()
        {
            var token = HttpContext.Session.GetString("Token");
            return !string.IsNullOrEmpty(token);
        }
    }
}
