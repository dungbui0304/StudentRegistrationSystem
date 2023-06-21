using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentRegistration.AdminApp.Services.Registration;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Registrations;

namespace StudentRegistration.AdminApp.Controllers
{
    public class RegistrationController : BaseController
    {
        private readonly IRegistrationApiClient _registrationApiClient;
        private readonly IConfiguration _configuration;
        public RegistrationController(IRegistrationApiClient registrationApiClient, IConfiguration configuration)
        {
            _registrationApiClient = registrationApiClient;
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

            var registrations = await _registrationApiClient.GetRegistrationPaging(pageIndex, pageSize);
            var responseData = TempData["ApiResponse"];
            if (responseData != null)
            {
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<string>>((string)responseData);
                TempData["ApiResponse"] = null;
                ViewBag.ApiResponse = apiResponse;
            }
            return View(registrations);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateRegistrationRequest request)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _registrationApiClient.Create(request);
            if (!result)
            {
                var errorResponse = new ApiResponse<string>()
                {
                    Status = false,
                    Message = $"Đăng kí học có mã sinh viên {request.StudentId} và mã môn học {request.CourseId} đã tồn tại. Vui lòng tạo lại!"
                };
                TempData["ApiResponse"] = JsonConvert.SerializeObject(errorResponse);
                return RedirectToAction("AdminIndex", "Student");
            }

            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Tạo thành công đăng kí học {request.Id}!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
            return RedirectToAction("Index", "Registration");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            var result = await _registrationApiClient.Delete(id);
            if (!result)
                return BadRequest(result);
            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Xóa đăng kí học thành công!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
            return RedirectToAction("Index", "Registration");
        }

        [HttpGet]
        public async Task<ActionResult> Update(string id)
        {
            var registration = await _registrationApiClient.GetById(id);
            if (registration == null)
                return NotFound();
            return View(registration);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateRegistrationRequest request)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _registrationApiClient.Update(request);
            if (!result)
                return NotFound();
            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Cập nhật đăng kí học thành công !"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
            return RedirectToAction("Index", "Registration");
        }
    }
}
