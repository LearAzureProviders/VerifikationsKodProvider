
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VerifikationsKodProvider.Data.Context;
using VerifikationsKodProvider.Models;

namespace VerifikationsKodProvider.Services;

public class ValidateVerificationCodeService : IValidateVerificationCodeService
{
    private readonly ILogger<ValidateVerificationCodeService> _logger;
    private readonly DataContext _context;

    public ValidateVerificationCodeService(ILogger<ValidateVerificationCodeService> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<bool> ValidateCodeAsync(ValidateRequest validateRequest)
    {
        try
        {

            var entity = await _context.VerificationRequests.FirstOrDefaultAsync(x => x.Email == validateRequest.Email && x.Code == validateRequest.Code);
            if (entity != null)
            {
                _context.VerificationRequests.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : ValidateVerificationCode.ValidateCodeAsync :: {ex.Message}");
        }
        return false;
    }

    public async Task<ValidateRequest> UnpackValidateRequestAsync(HttpRequest req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(body))
            {
                var validatRequest = JsonConvert.DeserializeObject<ValidateRequest>(body);

                if (validatRequest != null)
                {
                    return validatRequest;
                }
            }
        }

        catch (Exception ex)
        {
            _logger.LogError($"ERROR : ValidateVerificationCode.Ru :: {ex.Message}");
        }

        return null!;
    }

}
