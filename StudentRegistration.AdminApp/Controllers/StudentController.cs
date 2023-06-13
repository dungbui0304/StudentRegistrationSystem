using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.AdminApp.Services;
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
        public async Task<ActionResult> AdminIndex()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var students = await _studentApiClient.GetAll();
            return View(students);
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
                return BadRequest(result);
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
            return RedirectToAction("AdminIndex", "Student");
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("AdminIndex", "Login");
        }
    }
}
