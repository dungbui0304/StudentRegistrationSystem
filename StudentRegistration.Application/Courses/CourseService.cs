using StudentRegistration.Data.EF;
using StudentRegistration.Data.Entities;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Courses;

namespace StudentRegistration.Application.Courses
{
    public class CourseService : ICourseService
    {
        private readonly StudentDbContext _context;
        public CourseService(StudentDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<CourseViewModel>> GetListCourse()
        {
            var courses = _context.Courses.ToList();
            var courseViewModel = courses.Select(x => new CourseViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToList();
            var pagedResult = new PagedResult<CourseViewModel>()
            {
                Items = courseViewModel,
            };
            return pagedResult;
        }

        public async Task<string> Create(CreateCourseRequest request)
        {
            var course = new Course()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description
            };

            _context.Add(course);
            await _context.SaveChangesAsync();
            return course.Id;
        }

        public async Task<bool> Delete(string Id)
        {
            var course = await _context.Courses.FindAsync(Id);
            if (course == null)
            {
                return false;
                throw new Exception($"Cannot find course with id={Id}");
            }
            else
            {
                _context.Remove(course);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Edit(UpdateCourseRequest request)
        {
            var course = await _context.Courses.FindAsync(request.Id);
            if (course == null)
            {
                return false;
                throw new Exception($"Cannot find course with id={request.Id}");
            }
            else
            {
                course.Id = request.Id;
                course.Name = request.Name;
                course.Description = request.Description;

                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<CourseViewModel> GetById(string id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                throw new Exception($"Cannot find course with id={id}");

            var courseViewModel = new CourseViewModel()
            {
                Id = id,
                Name = course.Name,
                Description = course.Description
            };
            return courseViewModel;
        }
    }
}
