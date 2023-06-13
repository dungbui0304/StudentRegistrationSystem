using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentRegistration.Data.EF;
using StudentRegistration.Data.Entities;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentRegistration.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly StudentDbContext _context;

        public UserService(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, StudentDbContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return null;
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);
            if (!result.Succeeded) return null;

            // khởi tạo symmetric
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // khởi tạo signing credenticials
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(user);

            //khởi tạo ds claims để thêm vào token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,string.Join(";",roles))
            };
            // tạo jwt token với các thông tin issuer,audience, ds claim, thời gian hết hạn credentials
            var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Issuer"],
                            claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: credentials
                        );
            // chuyển đổi token thành string và trả về
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return false;
                throw new Exception($"Cannot find user with id = {id}");
            }
            else
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == id);
                if (student == null)
                    throw new Exception($"Cannot find student with id = {id}");

                // get role user and delete
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                _context.Remove(student);
                await _context.SaveChangesAsync();
                await _userManager.DeleteAsync(user);
                return true;
            }
        }

        public async Task<PagedResult<UserViewModel>> GetAll()
        {
            var users = _userManager.Users;
            var userViewModel = users.Select(x => new UserViewModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
            }).ToList();
            var pagedResult = new PagedResult<UserViewModel>()
            {
                Items = userViewModel,
            };
            return pagedResult;
        }

        public async Task<UserViewModel> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new Exception($"Cannot find user with id = {id}");
            var userViewModel = new UserViewModel()
            {
                Id = id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                FirstLogin = user.FirstLogin
            };
            return userViewModel;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                throw new Exception("Tài khoản đã tồn tại");
            }
            var email = await _userManager.FindByEmailAsync(request.Email);
            if (email != null)
            {
                throw new Exception("Emai đã tồn tại");
            }

            user = new User()
            {
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, "admin");

            if (result.Succeeded)
                return true;
            return false;
        }

        public async Task<bool> Update(Guid id, UpdateRequest request)
        {
            var checkEmail = await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id);
            if (checkEmail)
            {
                throw new Exception("Email đã tồn tại");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new Exception($"Cannot find user with id = {id}");

            user.UserName = request.UserName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return true;
            return false;
        }
    }
}
