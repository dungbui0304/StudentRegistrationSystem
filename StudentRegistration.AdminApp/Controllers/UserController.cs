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
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var users = await _userApiClient.GetAll();
            var token = HttpContext.Session.GetString("Token");
            return View(users);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _userApiClient.Register(request);
            if (!result)
                return BadRequest(result);
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            var result = await _userApiClient.Delete(id);
            if (!result)
                return BadRequest(result);
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public async Task<ActionResult> Update(Guid id)
        {
            var user = await _userApiClient.GetById(id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateRequest request)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _userApiClient.Update(request);
            var userNameUpdate = request.UserName;
            UpdateClaim(userNameUpdate);
            if (!result)
                return BadRequest(result);
            return RedirectToAction("Index", "User");
        }

        public void UpdateClaim(string userName)
        {
            // lấy token từ session
            var token = HttpContext.Session.GetString("Token");
            // xác thực claim dựa vào token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Issuer"],
                ValidateLifetime = true,
                IssuerSigningKey = securityKey
            };
            SecurityToken validatedToken;
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            var claimIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            if (claimsPrincipal != null)
            {
                var existingNameClaim = claimIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (existingNameClaim != null)
                    claimIdentity.RemoveClaim(existingNameClaim);

                claimIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
            }
            // tạo token mới
            // khởi tạo signing credenticials
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // tạo jwt token với các thông tin issuer,audience, ds claim, thời gian hết hạn credentials
            var newToken = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Issuer"],
                            claimIdentity.Claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: credentials
                        );
            var stringNewToken = tokenHandler.WriteToken(newToken);
            // cập nhật lại token
            HttpContext.Session.SetString("Token", stringNewToken);

            // thêm các thuộc tính cho authentication cookie
            AuthenticationProperties authenticationProperties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10)
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }
    }
}
