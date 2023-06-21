using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Data.EF;
using StudentRegistration.Data.Entities;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Students;
using System.Security.Claims;

namespace StudentRegistration.Application.Students
{
    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _context;
        private readonly UserManager<User> _userManager;
        public StudentService(StudentDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<PagedResult<StudentViewModel>> GetStudentPaging(int pageIndex, int pageSize)
        {
            // tính toán tổng số trang và số lượng item trên mỗi trang
            var totalItems = await _context.Students.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // lấy dữ liệu trang hiện tại
            var currentPageData = _context.Students.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var studentViewModel = currentPageData.Select(s => new StudentViewModel
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                PhoneNumber = s.PhoneNumber,
                Email = s.Email
            }).ToList();

            var pagedResult = new PagedResult<StudentViewModel>()
            {
                Items = studentViewModel,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalPage = totalPages
            };
            return pagedResult;
        }
        public async Task<bool> Create(CreateStudentRequest request)
        {
            try
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = request.Id,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstLogin = true
                };
                var result = await _userManager.CreateAsync(user, "12345678");

                //var roleExits = await _roleManager.RoleExistsAsync("student");
                //if (!roleExits)
                //{
                //    // tạo role mới
                //    var role = new Role { Name = "student" };
                //    await _roleManager.CreateAsync(role);
                //}
                if (!result.Succeeded)
                {
                    return false;
                    throw new Exception("Cannot create user");
                }
                await _userManager.AddToRoleAsync(user, "student");

                var student = new Student()
                {
                    Id = request.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    User = user,
                    UserId = user.Id,

                };
                _context.Add(student);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Cannot create student", ex);
            };
        }
        public async Task<bool> Delete(string Id)
        {
            var student = await _context.Students.FindAsync(Id);
            if (student == null)
            {
                return false;
                throw new Exception($"Cannot find student with id={Id}");
            }
            else
            {
                var user = await _userManager.FindByIdAsync(student.UserId.ToString());

                // get role user and delete
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.DeleteAsync(user);
                _context.Remove(student);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> Edit(UpdateStudentRequest request)
        {
            var student = await _context.Students.FindAsync(request.Id);
            if (student == null)
            {
                return false;
                throw new Exception($"Cannot find student with id={request.Id}");
            }
            else
            {
                var user = await _userManager.FindByIdAsync(student.UserId.ToString());

                user.UserName = request.Id;
                user.PhoneNumber = request.PhoneNumber;
                user.Email = request.Email;
                await _userManager.UpdateAsync(user);

                student.Id = request.Id;
                student.FirstName = request.FirstName;
                student.LastName = request.LastName;
                student.PhoneNumber = request.PhoneNumber;
                student.Email = request.Email;

                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<UpdateStudentRequest> GetById(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                throw new Exception($"Cannot find student with id={id}");

            var studentViewModel = new UpdateStudentRequest()
            {
                Id = id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
            };
            return studentViewModel;
        }
        public async Task<StudentViewModel> GetByUserId(Guid userId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null)
                throw new Exception($"Cannot find student with userId={userId}");

            var studentViewModel = new StudentViewModel()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
            };
            return studentViewModel;
        }
        public async Task<string> RegisterCourse(string courseId, string studentId)
        {
            var courseRegistration = new Registration()
            {
                Id = "DK-" + courseId + "-" + studentId,
                CourseId = courseId,
                StudentId = studentId,
                CreateAt = DateTime.Now
            };
            _context.Add(courseRegistration);
            await _context.SaveChangesAsync();
            return courseRegistration.Id;
        }
        public async Task<bool> ChangePassword(ChangePassword request, ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
                throw new Exception($"Cannot find user.");
            var result = await _userManager.ChangePasswordAsync(user, request.Password, request.ComfirmPassword);
            if (result.Succeeded)
            {
                user.FirstLogin = false;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<PagedResult<CourseRegisterViewModel>> GetCourseRegister(string studentId)
        {
            var query = from student in _context.Students
                        join registration in _context.Registrations on student.Id equals registration.StudentId
                        join course in _context.Courses on registration.CourseId equals course.Id
                        where student.Id == studentId
                        select new CourseRegisterViewModel
                        {
                            CourseId = course.Id,
                            CourseName = course.Name,
                        };
            var courseRegisterViewModel = await query.ToListAsync();

            var pageResult = new PagedResult<CourseRegisterViewModel>()
            {
                Items = courseRegisterViewModel
            };
            return pageResult;
        }
    }
}
