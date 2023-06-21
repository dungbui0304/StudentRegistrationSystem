using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentRegistration.AdminApp.Services.Course;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Courses;

namespace StudentRegistration.AdminApp.Controllers
{
    public class CourseController : BaseController
    {
        private readonly ICourseApiClient _courseApiClient;
        private readonly IConfiguration _configuration;
        public CourseController(ICourseApiClient courseApiClient, IConfiguration configuration)
        {
            _courseApiClient = courseApiClient;
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

            var courses = await _courseApiClient.GetCoursePaging(pageIndex, pageSize);
            var responseData = TempData["ApiResponse"];
            if (responseData != null)
            {
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<string>>((string)responseData);
                TempData["ApiResponse"] = null;
                ViewBag.ApiResponse = apiResponse;
            }
            return View(courses);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var courses = await _courseApiClient.GetAll();
            return View(courses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCourseRequest request)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _courseApiClient.Create(request);
            if (!result)
            {
                var errorResponse = new ApiResponse<string>()
                {
                    Status = false,
                    Message = $"Môn học có mã {request.Id} đã tồn tại. Vui lòng tạo lại!"
                };
                TempData["ApiResponse"] = JsonConvert.SerializeObject(errorResponse);
                return RedirectToAction("AdminIndex", "Student");
            }

            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Tạo thành công môn học {request.Name}!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
            return RedirectToAction("Index", "Course");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            var result = await _courseApiClient.Delete(id);
            if (!result)
                return BadRequest(result);
            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Xóa môn học thành công!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        public async Task<ActionResult> Update(string id)
        {
            var course = await _courseApiClient.GetById(id);
            if (course == null)
                return NotFound();
            return View(course);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateCourseRequest request)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _courseApiClient.Update(request);
            if (!result)
                return NotFound();
            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Cập nhật thành công môn học {request.Name}!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
            return RedirectToAction("Index", "Course");
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
