using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Courses;

namespace StudentRegistration.Application.Courses
{
    public interface ICourseService
    {
        public Task<CourseViewModel> GetById(string id);
        public Task<PagedResult<CourseViewModel>> GetCoursePaging(int pageIndex, int pageSize);
        public Task<PagedResult<CourseViewModel>> GetAll();
        public Task<string> Create(CreateCourseRequest request);
        public Task<bool> Edit(UpdateCourseRequest updateRequest);
        public Task<bool> Delete(string Id);
    }
}
