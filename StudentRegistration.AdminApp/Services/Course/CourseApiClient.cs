using Newtonsoft.Json;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Courses;
using System.Net.Http.Headers;
using System.Text;

namespace StudentRegistration.AdminApp.Services.Course
{
    public class CourseApiClient : ICourseApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CourseApiClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Delete(string id)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.DeleteAsync($"/api/Course/{id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            // convert result to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }

        public async Task<PagedResult<CourseViewModel>> GetAll()
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync("api/Course");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var courses = JsonConvert.DeserializeObject<PagedResult<CourseViewModel>>(result);
            return courses;
        }

        public async Task<CourseViewModel> GetById(string id)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync($"/api/Course/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var course = JsonConvert.DeserializeObject<CourseViewModel>(result);
            return course;
        }

        public async Task<bool> Create(CreateCourseRequest request)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // convert data request to json
            var json = JsonConvert.SerializeObject(request);
            // tạo chuỗi httpContent để gửi đi
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            // thực hiện gọi api
            var response = await _httpClient.PostAsync("/api/Course", httpContent);
            var data = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
        public async Task<bool> Update(UpdateCourseRequest request)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // convert data request to json
            var json = JsonConvert.SerializeObject(request);
            // tạo chuỗi httpContent để gửi đi
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            // thực hiện gọi api
            var response = await _httpClient.PutAsync($"/api/Course/{request.Id}", httpContent);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            // convert data to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
    }
}
