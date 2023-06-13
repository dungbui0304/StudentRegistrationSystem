﻿using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.ViewModel.Registrations
{
    public class UpdateRegistrationRequest
    {
        [Display(Name = "Mã đăng ký học")]
        public string? Id { get; set; }

        [Display(Name = "Mã sinh viên")]
        public string? StudentId { get; set; }

        [Display(Name = "Mã môn học")]
        public string? CourseId { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
