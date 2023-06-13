using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Users;

namespace StudentRegistration.AdminApp.Services
{
    public interface IUserApiClient
    {
        public Task<string> Authenticate(LoginRequest request);
        public Task<bool> Register(RegisterRequest request);
        public Task<PagedResult<UserViewModel>> GetAll();
        public Task<UserViewModel> GetById(Guid id);
        public Task<bool> Update(UpdateRequest request);
        public Task<bool> Delete(Guid id);
    }
}
