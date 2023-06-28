using Newtonsoft.Json;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Students;
using System.Net.Http.Headers;
using System.Text;

namespace StudentRegistration.AdminApp.Services
{
    public class StudentApiClient : IStudentApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StudentApiClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<PagedResult<StudentViewModel>> GetStudentPaging(int pageIndex, int pageSize)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync($"/api/Student?pageIndex={pageIndex}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var students = JsonConvert.DeserializeObject<PagedResult<StudentViewModel>>(result);
            return students;
        }
        public async Task<List<StudentViewModel>> GetAll()
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync($"/api/Student/all-student");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var listStudents = JsonConvert.DeserializeObject<List<StudentViewModel>>(result);
            return listStudents;
        }
        public async Task<bool> Delete(string id)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.DeleteAsync($"/api/Student/{id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            // convert result to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
        public async Task<UpdateStudentRequest> GetById(string id)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync($"/api/Student/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var student = JsonConvert.DeserializeObject<UpdateStudentRequest>(result);
            return student;
        }
        public async Task<StudentViewModel> GetByUserId(Guid userId)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync($"/api/Student/user/{userId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var student = JsonConvert.DeserializeObject<StudentViewModel>(result);
            return student;
        }
        public async Task<bool> Create(CreateStudentRequest request)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // convert data request to json
            var json = JsonConvert.SerializeObject(request);
            // tạo chuỗi httpContent để gửi đi
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            // thực hiện gọi api
            var response = await _httpClient.PostAsync("/api/Student", httpContent);
            var data = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
        public async Task<bool> Update(UpdateStudentRequest request)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // convert data request to json
            var json = JsonConvert.SerializeObject(request);
            // tạo chuỗi httpContent để gửi đi
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            // thực hiện gọi api
            var response = await _httpClient.PutAsync($"/api/Student/{request.Id}", httpContent);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            // convert data to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
        public async Task<bool> RegisterCourse(string courseId, string studentId)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.PostAsync($"/api/Student/register-course?courseId={courseId}&studentId={studentId}", null);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            // convert result to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
        public async Task<bool> ChangePassword(ChangePassword request)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // convert data request to json
            var json = JsonConvert.SerializeObject(request);
            // tạo chuỗi httpContent để gửi đi
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            // thực hiện gọi api
            var response = await _httpClient.PostAsync($"/api/Student/change-password", httpContent);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            // convert result to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
        public async Task<PagedResult<CourseRegisterViewModel>> GetCourseRegister(string studentId)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync($"/api/Student/CourseRegister/{studentId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var courseRegister = JsonConvert.DeserializeObject<PagedResult<CourseRegisterViewModel>>(result);
            return courseRegister;
        }
        public async Task<PagedResult<StudentViewModel>> SearchStudent(string SearchString, int pagedIndex)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync($"/api/Student/search-student?SearchString={SearchString}&pagedIndex={pagedIndex}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var students = JsonConvert.DeserializeObject<PagedResult<StudentViewModel>>(result);
            return students;
        }
        public async Task<bool> ImportExcel(IFormFile excelFile)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // convert data excelFile to MultipartFormDataContent
            var multipartContent = new MultipartFormDataContent();
            var fileContent = new StreamContent(excelFile.OpenReadStream());
            multipartContent.Add(fileContent, "UploadFile", excelFile.FileName);

            // thực hiện gọi api
            var response = await _httpClient.PostAsync($"/api/Student/upload-file", multipartContent);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            // convert data to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
        public async Task<byte[]> ExportExcel()
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // thực hiện gọi api
            var response = await _httpClient.PostAsync($"/api/Student/export-file", null);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsByteArrayAsync();
            return data;
        }
    }
}
