using Microsoft.AspNetCore.Mvc;
using StudentRegistration.AdminApp.Services.Registration;
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
        public async Task<ActionResult> Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var registrations = await _registrationApiClient.GetAll();
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
                return BadRequest(result);
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
            return RedirectToAction("Index", "Registration");
        }
    }
}
