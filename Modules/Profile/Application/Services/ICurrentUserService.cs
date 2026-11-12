namespace Modules.Profile.Application.Services
{
    public interface ICurrentUserService
    {
        string GetCurrentIdentityProviderId();
        string GetUserEmail();
    }
}