using Newtonsoft.Json;
using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Registrations;
using System.Net.Http.Headers;
using System.Text;

namespace StudentRegistration.AdminApp.Services.Registration
{
    public class RegistrationApiClient : IRegistrationApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RegistrationApiClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
            var response = await _httpClient.DeleteAsync($"/api/Registration/{id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            // convert result to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }

        public async Task<PagedResult<RegistrationViewModel>> GetAll()
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync("/api/Registration");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var registrations = JsonConvert.DeserializeObject<PagedResult<RegistrationViewModel>>(result);
            return registrations;
        }

        public async Task<RegistrationViewModel> GetById(string id)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // thực hiện gọi api
            var response = await _httpClient.GetAsync($"/api/Registration/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            // convert result to json
            var registration = JsonConvert.DeserializeObject<RegistrationViewModel>(result);
            return registration;
        }

        public async Task<bool> Create(CreateRegistrationRequest request)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // convert data request to json
            var json = JsonConvert.SerializeObject(request);
            // tạo chuỗi httpContent để gửi đi
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            // thực hiện gọi api
            var response = await _httpClient.PostAsync("/api/Registration", httpContent);
            var data = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
        public async Task<bool> Update(UpdateRegistrationRequest request)
        {
            // get token gán vào header khi gọi api
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // convert data request to json
            var json = JsonConvert.SerializeObject(request);
            // tạo chuỗi httpContent để gửi đi
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            // thực hiện gọi api
            var response = await _httpClient.PutAsync($"/api/Registration/{request.Id}", httpContent);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            // convert data to json
            var result = JsonConvert.DeserializeObject<bool>(data);
            return result;
        }
    }
}
