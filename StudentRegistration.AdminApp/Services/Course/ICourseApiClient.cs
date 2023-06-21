using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Courses;

namespace StudentRegistration.AdminApp.Services.Course
{
    public interface ICourseApiClient
    {
        public Task<bool> Create(CreateCourseRequest request);
        public Task<PagedResult<CourseViewModel>> GetCoursePaging(int pageIndex, int pageSize);
        public Task<PagedResult<CourseViewModel>> GetAll();
        public Task<CourseViewModel> GetById(string id);
        public Task<bool> Update(UpdateCourseRequest request);
        public Task<bool> Delete(string id);
    }
}
