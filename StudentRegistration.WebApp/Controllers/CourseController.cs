using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Courses;
using StudentRegistration.ViewModel.Courses;

namespace StudentRegistration.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoursePaging(int pageIndex, int pageSize)
        {
            var courses = await _courseService.GetCoursePaging(pageIndex, pageSize);
            if (courses == null)
                return BadRequest("Cannot load list courses");
            return Ok(courses);
        }

        [HttpGet("list-course")]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAll();
            if (courses == null)
                return BadRequest("Cannot load list courses");
            return Ok(courses);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var course = await _courseService.Create(request);
            if (course == null)
                return BadRequest("Cannot create course");
            return Ok(course);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _courseService.Delete(id);
            if (!result)
                return BadRequest($"Cannot delete course with id = {id}");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var course = await _courseService.GetById(id);
            if (course == null)
                return BadRequest($"Cannot find course with id = {id}");
            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateCourseRequest request)
        {
            var result = await _courseService.Edit(request);
            if (!result)
                return BadRequest($"Cannot update course with id = {request.Id}");
            return Ok(result);
        }
    }
}
