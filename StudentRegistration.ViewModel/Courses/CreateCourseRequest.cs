using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.ViewModel.Courses
{
    public class CreateCourseRequest
    {
        [Display(Name = "Mã môn học")]
        public string? Id { get; set; }

        [Display(Name = "Tên môn học")]
        public string? Name { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
    }
}
