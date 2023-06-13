using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.AdminApp.Services.Course;
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
        public async Task<ActionResult> Index()
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
        public async Task<ActionResult> GetListCourse()
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
                return BadRequest(result);
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
