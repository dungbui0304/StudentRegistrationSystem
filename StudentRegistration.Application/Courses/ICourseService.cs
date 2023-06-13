using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Courses;

namespace StudentRegistration.Application.Courses
{
    public interface ICourseService
    {
        public Task<CourseViewModel> GetById(string id);
        public Task<PagedResult<CourseViewModel>> GetListCourse();
        public Task<string> Create(CreateCourseRequest request);
        public Task<bool> Edit(UpdateCourseRequest updateRequest);
        public Task<bool> Delete(string Id);
    }
}
