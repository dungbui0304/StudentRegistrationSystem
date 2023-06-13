using Microsoft.EntityFrameworkCore;
using StudentRegistration.Data.EF;
using StudentRegistration.Data.Entities;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Registrations;

namespace StudentRegistration.Application.Registrations
{
    public class RegistrationService : IRegistrationService
    {
        private readonly StudentDbContext _context;
        public RegistrationService(StudentDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<RegistrationViewModel>> GetListRegistration()
        {
            var query = from student in _context.Students
                        join registration in _context.Registrations on student.Id equals registration.StudentId
                        join course in _context.Courses on registration.CourseId equals course.Id
                        select new RegistrationViewModel
                        {
                            Id = registration.Id,
                            StudentId = student.Id,
                            CourseId = course.Id,
                            StudentName = student.LastName + " " + student.FirstName,
                            CourseName = course.Name,
                            CreateAt = registration.CreateAt,
                        };
            var registrationViewModel = await query.ToListAsync();

            var pageResult = new PagedResult<RegistrationViewModel>()
            {
                Items = registrationViewModel
            };
            return pageResult;
        }

        public async Task<bool> Create(CreateRegistrationRequest request)
        {
            try
            {
                var registration = new Registration()
                {
                    Id = "DK-" + request.CourseId + "-" + request.StudentId,
                    StudentId = request.StudentId,
                    CourseId = request.CourseId,
                    CreateAt = DateTime.Now
                };
                _context.Add(registration);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create registration", ex);
            }
        }

        public async Task<bool> Delete(string id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return false;
                throw new Exception($"Cannot find registration with courseId={registration.CourseId} and studentId={registration.StudentId}");
            }
            else
            {
                _context.Remove(registration);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Edit(UpdateRegistrationRequest request)
        {
            try
            {
                var registration = await _context.Registrations.FindAsync(request.Id);
                if (registration == null)
                {
                    return false;
                    throw new Exception($"Cannot find registration with StudentId={request.StudentId} and CourseId={request.CourseId}");
                }
                else
                {
                    _context.Registrations.Remove(registration);
                    await _context.SaveChangesAsync();

                    var newRegistration = new Registration()
                    {
                        Id = "DK-" + request.CourseId + "-" + request.StudentId,
                        CourseId = request.CourseId,
                        StudentId = request.StudentId,
                        CreateAt = request.CreateAt
                    };

                    _context.Registrations.Add(newRegistration);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot update registration", ex);
            }
        }

        public async Task<RegistrationViewModel> GetById(string id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
                throw new Exception($"Cannot find registration with with StudentId={registration.StudentId} and CourseId={registration.CourseId}");

            var registrationViewModel = new RegistrationViewModel()
            {
                Id = registration.Id,
                StudentId = registration.StudentId,
                CourseId = registration.CourseId,
                CreateAt = registration.CreateAt
            };
            return registrationViewModel;
        }
    }
}