using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StudentRegistration.AdminApp.Services;
using StudentRegistration.ViewModel.Common;
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
        public async Task<ActionResult> Index(int pageIndex, int pageSize = 3)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pageIndex == null || pageIndex <= 0)
                pageIndex = 1;

            var users = await _userApiClient.GetUserPaging(pageIndex, pageSize);
            // lấy token lưu trong session
            var token = HttpContext.Session.GetString("Token");

            var responseData = TempData["ApiResponse"];
            if (responseData != null)
            {
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<string>>((string)responseData);
                TempData["ApiResponse"] = null;
                ViewBag.ApiResponse = apiResponse;
            }
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
            {
                var errorResponse = new ApiResponse<string>()
                {
                    Status = false,
                    Message = $"Người dùng {request.UserName} đã tồn tại. Vui lòng tạo lại!"
                };
                TempData["ApiResponse"] = JsonConvert.SerializeObject(errorResponse);
                return RedirectToAction("Index", "User");
            }

            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Tạo thành công người dùng {request.UserName}!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
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
            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Xóa người dùng thành công!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
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
            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Cập nhật người dùng {request.UserName} thành công!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
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
