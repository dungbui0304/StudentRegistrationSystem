using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Registrations;
using StudentRegistration.ViewModel.Registrations;

namespace StudentRegistration.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegistrationController : Controller
    {
        private readonly IRegistrationService _registrationService;
        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRegistrationPaging(int pageIndex, int pageSize)
        {
            var registrations = await _registrationService.GetRegistrationPaging(pageIndex, pageSize);
            if (registrations == null)
                return BadRequest("Cannot load list registrations");
            return Ok(registrations);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var registration = await _registrationService.Create(request);
            if (registration == null)
                return BadRequest("Cannot create registration");
            return Ok(registration);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _registrationService.Delete(id);
            if (!result)
                return BadRequest($"Cannot delete registration with Id = {id}");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var registration = await _registrationService.GetById(id);
            if (registration == null)
                return BadRequest($"Cannot find registration with courseId = {registration.CourseId} and studentId = {registration.StudentId}");
            return Ok(registration);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateRegistrationRequest request)
        {
            var result = await _registrationService.Edit(request);
            if (!result)
                return BadRequest($"Cannot update registration with id = {request.Id}");
            return Ok(result);
        }
    }
}
