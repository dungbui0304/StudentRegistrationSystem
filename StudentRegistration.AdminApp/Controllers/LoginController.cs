using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentRegistration.AdminApp.Services;
using StudentRegistration.ViewModel.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentRegistration.AdminApp.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public LoginController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var token = await _userApiClient.Authenticate(request);
            HttpContext.Session.SetString("Token", token);

            ClaimsPrincipal principal = ValidateToken(token);

            //get info user
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return NotFound();
            var user = await _userApiClient.GetById(Guid.Parse(userId));

            // check role
            bool isAdmin = principal.IsInRole("admin");
            bool isStudent = principal.IsInRole("student");

            // thêm các thuộc tính cho authentication cookie
            AuthenticationProperties authenticationProperties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

            if (isAdmin)
                return RedirectToAction("Index", "Home");
            else if (isStudent)
            {
                if (user == null)
                    return NotFound();
                if (user.FirstLogin)
                    return RedirectToAction("ChangePassword", "Student");
                else
                    return RedirectToAction("StudentIndex", "Student");
            }
            else
                return RedirectToAction("AccessDenied", "Error");
        }

        [HttpPost]
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Issuer"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            SecurityToken validatedToken;
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            return claimsPrincipal;
        }
    }
}
