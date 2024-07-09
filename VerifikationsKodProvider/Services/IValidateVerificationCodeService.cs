using Microsoft.AspNetCore.Http;
using VerifikationsKodProvider.Models;

namespace VerifikationsKodProvider.Services
{
    public interface IValidateVerificationCodeService
    {
        Task<ValidateRequest> UnpackValidateRequestAsync(HttpRequest req);
        Task<bool> ValidateCodeAsync(ValidateRequest validateRequest);
    }
}