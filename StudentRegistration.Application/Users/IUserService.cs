using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Users;

namespace StudentRegistration.Application.Users
{
    public interface IUserService
    {
        public Task<string> Authenticate(LoginRequest request);
        public Task<bool> Register(RegisterRequest request);
        public Task<PagedResult<UserViewModel>> GetUserPaging(int pageIndex, int pageSize);
        public Task<UserViewModel> GetById(Guid id);
        public Task<bool> Update(Guid id, UpdateRequest request);
        public Task<bool> Delete(Guid id);
    }
}
