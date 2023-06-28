using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentRegistration.AdminApp.Services;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Students;
using System.Security.Claims;

namespace StudentRegistration.AdminApp.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IStudentApiClient _studentApiClient;
        private readonly IConfiguration _configuration;
        public StudentController(IStudentApiClient studentApiClient, IConfiguration configuration)
        {
            _studentApiClient = studentApiClient;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _studentApiClient.ChangePassword(request);
            if (!result)
                return BadRequest("Change Password fail.");
            return RedirectToAction("StudentIndex", "Student");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return NotFound();
            var student = await _studentApiClient.GetByUserId(Guid.Parse(userId));
            if (student == null)
                return NotFound();
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseRegister()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return NotFound();
            var student = await _studentApiClient.GetByUserId(Guid.Parse(userId));
            if (student == null)
                return NotFound();

            var courseRegister = await _studentApiClient.GetCourseRegister(student.Id);
            if (courseRegister == null)
                return NotFound();
            return View(courseRegister);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCourse(string courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return NotFound();
            var student = await _studentApiClient.GetByUserId(Guid.Parse(userId));
            var coureRegistration = _studentApiClient.RegisterCourse(courseId, student.Id);
            if (coureRegistration == null)
                return BadRequest(coureRegistration);
            return RedirectToAction("StudentIndex", "Student");
        }

        [HttpGet]
        public async Task<ActionResult> AdminIndex(string SearchString, string CurrentFilter, int pageIndex, int pageSize = 3)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            else
            {
                SearchString = CurrentFilter;
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (string.IsNullOrEmpty(SearchString))
            {
                var students = await _studentApiClient.GetStudentPaging(pageIndex, pageSize);
                var dataResponse = TempData["ApiResponse"];
                if (dataResponse != null)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<string>>((string)dataResponse);
                    TempData["ApiResponse"] = null;
                    ViewBag.ApiResponse = apiResponse;
                }
                return View(students);
            }
            else
            {
                var studentSearchs = await _studentApiClient.SearchStudent(SearchString, pageIndex);

                // sử dung TempData và ViewBag để truyền dữ liệu từ controller sang view
                if (studentSearchs.Items == null || studentSearchs.Items.Count == 0)
                {
                    var errorResponse = new ApiResponse<StudentViewModel>()
                    {
                        Status = false,
                        Data = null,
                        Message = "Không tìm thấy sinh viên!"
                    };
                    TempData["SearchResults"] = JsonConvert.SerializeObject(errorResponse);

                    var dataSearchErrorResponse = TempData["SearchResults"];
                    if (dataSearchErrorResponse != null)
                    {
                        var apiSearchResponse = JsonConvert.DeserializeObject<ApiResponse<StudentViewModel>>((string)dataSearchErrorResponse);
                        TempData["SearchResults"] = null;
                        ViewBag.SearchResults = apiSearchResponse;
                    }
                    return View(studentSearchs);
                }
                ViewBag.CurrentFilter = SearchString;

                return View(studentSearchs);
            }
        }

        [HttpGet]
        public async Task<IActionResult> StudentIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateStudentRequest request)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _studentApiClient.Create(request);
            if (!result)
            {
                var errorResponse = new ApiResponse<string>()
                {
                    Status = false,
                    Message = $"Sinh viên có mã {request.Id} đã tồn tại. Vui lòng tạo lại!"
                };
                TempData["ApiResponse"] = JsonConvert.SerializeObject(errorResponse);
                return RedirectToAction("AdminIndex", "Student");
            }

            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Tạo thành công sinh viên {request.LastName + " " + request.FirstName}!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
            return RedirectToAction("AdminIndex", "Student");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            var result = await _studentApiClient.Delete(id);
            if (!result)
                return BadRequest(result);
            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = "Xóa sinh viên thành công!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
            return RedirectToAction("AdminIndex", "Student");
        }

        [HttpGet]
        public async Task<ActionResult> Update(string id)
        {
            var student = await _studentApiClient.GetById(id);
            if (student == null)
                return NotFound();
            return View(student);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateStudentRequest request)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _studentApiClient.Update(request);
            if (!result)
                return NotFound();

            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Cập nhật sinh viên {request.LastName + " " + request.FirstName} thành công!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
            return RedirectToAction("AdminIndex", "Student");
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("AdminIndex", "Login");
        }

        [HttpGet]
        public async Task<ActionResult> SearchStudent(string searchString, int pagedIndex)
        {

            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (pagedIndex <= 0)
                pagedIndex = 1;

            var students = await _studentApiClient.SearchStudent(searchString, pagedIndex);


            return RedirectToAction("AdminIndex", "Student");
        }

        [HttpPost]
        public async Task<ActionResult> ImportExcel(IFormFile excelFile)
        {
            var result = await _studentApiClient.ImportExcel(excelFile);
            if (!result)
                return NotFound("Cannot import File Excel");
            var successResponse = new ApiResponse<string>()
            {
                Status = true,
                Message = $"Import dữ liệu thành công!"
            };
            TempData["ApiResponse"] = JsonConvert.SerializeObject(successResponse);
            return RedirectToAction("AdminIndex", "Student");
        }

        [HttpPost]
        public async Task<ActionResult> ExportExcel()
        {
            try
            {
                var excelData = await _studentApiClient.ExportExcel();
                System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Exports", "Export-Data-Student.xlsx"), excelData);
                return RedirectToAction("AdminIndex", "Student");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
