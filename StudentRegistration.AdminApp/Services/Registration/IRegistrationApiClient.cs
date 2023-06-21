using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Registrations;

namespace StudentRegistration.AdminApp.Services.Registration
{
    public interface IRegistrationApiClient
    {
        public Task<bool> Create(CreateRegistrationRequest request);
        public Task<PagedResult<RegistrationViewModel>> GetRegistrationPaging(int pageIndex, int pageSize);
        public Task<RegistrationViewModel> GetById(string id);
        public Task<bool> Update(UpdateRegistrationRequest request);
        public Task<bool> Delete(string id);
    }
}
