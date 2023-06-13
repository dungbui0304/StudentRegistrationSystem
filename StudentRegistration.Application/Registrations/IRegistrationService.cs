using StudentRegistration.ViewModel.Common;
using StudentRegistration.ViewModel.Registrations;

namespace StudentRegistration.Application.Registrations
{
    public interface IRegistrationService
    {
        public Task<RegistrationViewModel> GetById(string id);
        public Task<PagedResult<RegistrationViewModel>> GetListRegistration();
        public Task<bool> Create(CreateRegistrationRequest request);
        public Task<bool> Edit(UpdateRegistrationRequest updateRequest);
        public Task<bool> Delete(string id);
    }
}
