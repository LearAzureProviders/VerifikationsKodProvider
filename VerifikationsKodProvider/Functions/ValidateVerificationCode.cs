using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VerifikationsKodProvider.Data.Context;
using VerifikationsKodProvider.Models;
using VerifikationsKodProvider.Services;

namespace VerifikationsKodProvider.Functions
{
    public class ValidateVerificationCode
    {
        private readonly ILogger<ValidateVerificationCode> _logger;
        private readonly IValidateVerificationCodeService _validate;

        public ValidateVerificationCode(ILogger<ValidateVerificationCode> logger, IValidateVerificationCodeService validate)
        {
            _logger = logger;
            _validate = validate;
        }

        [Function("ValidateVerificationCode")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route ="Lear")] HttpRequest req)
        {


            try
            {
                var validateRequest = await _validate.UnpackValidateRequestAsync(req);
               if (validateRequest != null)
                {
                    var validateResult = await _validate.ValidateCodeAsync(validateRequest);
                    if (validateResult)
                    {
                        return new OkResult();
                    }
                }
            }
            
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : ValidateVerificationCode.Ru :: {ex.Message}");
            }
           
            return new UnauthorizedResult();
        }

      
    }
}
