using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Students;
using System.Security.Claims;

namespace StudentRegistration.Application.Students
{
    public interface IStudentService
    {
        public Task<UpdateStudentRequest> GetById(string id);
        public Task<StudentViewModel> GetByUserId(Guid userId);
        public Task<PagedResult<StudentViewModel>> GetStudentPaging(int pagedIndex, int pageSize);
        public Task<List<StudentViewModel>> GetAll();
        public Task<bool> Create(CreateStudentRequest request);
        public Task<bool> Edit(UpdateStudentRequest updateRequest);
        public Task<bool> Delete(string Id);
        public Task<string> RegisterCourse(string courseId, string studentId);
        public Task<bool> ChangePassword(ChangePassword request, ClaimsPrincipal principal);
        public Task<PagedResult<CourseRegisterViewModel>> GetCourseRegister(string studentId);
        public Task<PagedResult<StudentViewModel>> SearchStudent(string searchString, int pagedIndex);
        public Task<bool> ImportExcel(Stream fileStream);
        public Task<byte[]> ExportExcel();
    }
}
