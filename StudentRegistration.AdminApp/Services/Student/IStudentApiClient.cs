using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Students;

namespace StudentRegistration.AdminApp.Services
{
    public interface IStudentApiClient
    {
        public Task<bool> Create(CreateStudentRequest request);
        public Task<PagedResult<StudentViewModel>> GetStudentPaging(int pageIndex, int pageSize);
        public Task<List<StudentViewModel>> GetAll();
        public Task<UpdateStudentRequest> GetById(string id);
        public Task<StudentViewModel> GetByUserId(Guid id);
        public Task<bool> Update(UpdateStudentRequest request);
        public Task<bool> Delete(string id);
        public Task<bool> RegisterCourse(string courseId, string studentId);
        public Task<bool> ChangePassword(ChangePassword request);
        public Task<PagedResult<CourseRegisterViewModel>> GetCourseRegister(string studentId);
        public Task<PagedResult<StudentViewModel>> SearchStudent(string searchString, int pagedIndex);
        public Task<bool> ImportExcel(IFormFile excelFile);
        public Task<byte[]> ExportExcel();
    }
}
