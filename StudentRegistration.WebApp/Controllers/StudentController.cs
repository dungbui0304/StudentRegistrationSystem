using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Students;
using StudentRegistration.ViewModel.Students;

namespace StudentRegistration.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult> GetStudentPaging([FromQuery] int pageIndex, int pageSize)
        {
            var students = await _studentService.GetStudentPaging(pageIndex, pageSize);
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(students);
        }

        [HttpGet("all-student")]
        public async Task<ActionResult> GetAll()
        {
            var listStudents = await _studentService.GetAll();
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(listStudents);
        }
        [HttpGet("CourseRegister/{studentId}")]
        public async Task<IActionResult> GetCourseRegister(string studentId)
        {
            var courseRegister = await _studentService.GetCourseRegister(studentId);
            if (courseRegister == null)
                return NotFound();
            return Ok(courseRegister);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var student = await _studentService.GetById(id);
            if (student == null)
                return BadRequest($"Cannot find student with id={id}");
            return Ok(student);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var student = await _studentService.GetByUserId(userId);
            if (student == null)
                return BadRequest($"Cannot find student with userId={userId}");
            return Ok(student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit([FromBody] UpdateStudentRequest updateRequest)
        {
            var result = await _studentService.Edit(updateRequest);
            if (!ModelState.IsValid)
                return BadRequest("Edit Student unsuccessfull");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateStudentRequest request)
        {
            var student = await _studentService.Create(request);
            if (!ModelState.IsValid)
                return BadRequest("Create Student unsuccessfull");
            return Ok(student);
        }

        [HttpPost("register-course")]
        public async Task<IActionResult> RegisterCourse(string courseId, string studentId)
        {
            var result = await _studentService.RegisterCourse(courseId, studentId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _studentService.Delete(id);
            if (!ModelState.IsValid)
                return BadRequest("Delete Student unsuccessfull");
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePassword request)
        {
            var Claimpricipal = User;
            var result = await _studentService.ChangePassword(request, Claimpricipal);
            if (!result)
                return BadRequest("Change password fail!");
            return Ok(result);
        }

        [HttpGet("search-student")]
        public async Task<IActionResult> SearchStudent(string SearchString, int pagedIndex)
        {
            var searchResults = await _studentService.SearchStudent(SearchString, pagedIndex);
            if (searchResults == null)
                return NotFound("No student match keyword!");
            return Ok(searchResults);
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> ImportExcel()
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null)
                return BadRequest("No file found!");

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var result = await _studentService.ImportExcel(memoryStream);

                if (!result)
                {
                    return BadRequest("Import failed");
                }
                return Ok(result);
            }
        }

        [HttpPost("export-file")]
        public async Task<IActionResult> ExportExcel()
        {
            byte[] excelData = await _studentService.ExportExcel();
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}