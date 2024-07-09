
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VerifikationsKodProvider.Services;
namespace VerifikationsKodProvider.Functions
{

   
    public class GenerateVerificationCode
    {
        private readonly ILogger<GenerateVerificationCode> _logger;
        private readonly IVerificationService _service;

        public GenerateVerificationCode(ILogger<GenerateVerificationCode> logger, IVerificationService verificationService)
        {
            _logger = logger;
            _service = verificationService;
        }

        [Function(nameof(GenerateVerificationCode))]
        [ServiceBusOutput("email_request", Connection = "ServiceBusConnection")]
        public async Task<string> Run([ServiceBusTrigger("verification_request", Connection = "ServiceBusConnection")]ServiceBusReceivedMessage message,ServiceBusMessageActions messageActions)
        {
            try
            {
                var verificationRequest = _service.UnpackVerificationRequest(message);
                if (verificationRequest != null)
                {
                 


                    var code = _service.GenerateCode();
                    if (!string.IsNullOrEmpty(code))
                    {

                        var result = await _service.SaveVerificationRequest(verificationRequest, code);
                        if (result)
                        {
                            var emailRequest = _service.GenerateEmailRequest(verificationRequest, code);
                            if (emailRequest != null)
                            {
                                var payload = _service.GenerateServiceBusEmailRequest(emailRequest);
                                if (!string.IsNullOrEmpty(payload))
                                {
                                    await messageActions.CompleteMessageAsync(message);
                                    return payload;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: GenerateVerificationCode.Run() :: {ex.Message}");
            }
            return null!;
        }


    }
}
