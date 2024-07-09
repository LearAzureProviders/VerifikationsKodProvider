
namespace VerifikationsKodProvider.Services
{
    public interface IVerificationCleanerService
    {
        Task RemoveExpiredRecordsAsync();
    }
}