﻿namespace StudentRegistration.ViewModel.Common
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public PagedResult<T>? Data { get; set; }
    }
}
