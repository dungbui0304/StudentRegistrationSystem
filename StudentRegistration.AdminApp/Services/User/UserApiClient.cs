using Newtonsoft.Json;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Users;
using System.Net.Http.Headers;
using System.Text;

namespace StudentRegistration.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserApiClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            // convert data request to json
            var json = JsonConvert.SerializeObject(request);
            // tạo chuỗi httpContent để gửi đi
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            // thực hiện gọi api
            var response = await _httpClient.PostAsync("/api/User/authenticate", httpContent);
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<bool> Delete(Guid id)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.DeleteAsync($"/api/User/{id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            // convert result to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }

        public async Task<PagedResult<UserViewModel>> GetAll()
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync("/api/User");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var users = JsonConvert.DeserializeObject<PagedResult<UserViewModel>>(result);
            return users;
        }

        public async Task<UserViewModel> GetById(Guid id)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync($"/api/User/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var user = JsonConvert.DeserializeObject<UserViewModel>(result);
            return user;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // convert data request to json
            var json = JsonConvert.SerializeObject(request);
            // tạo chuỗi httpContent để gửi đi
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            // thực hiện gọi api
            var response = await _httpClient.PostAsync("/api/User", httpContent);
            var data = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }

        public async Task<bool> Update(UpdateRequest request)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // convert data request to json
            var json = JsonConvert.SerializeObject(request);
            // tạo chuỗi httpContent để gửi đi
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            // thực hiện gọi api
            var response = await _httpClient.PutAsync($"/api/User/{request.Id}", httpContent);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            // convert data to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
    }
}
