using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VerifikationsKodProvider.Data.Context;
using VerifikationsKodProvider.Services;

namespace VerifikationsKodProvider.Functions
{
    public class VerificationCleaner
    {
        private readonly ILogger<VerificationCleaner> _logger;
        private readonly IVerificationCleanerService _cleanerService;

        public VerificationCleaner(ILogger<VerificationCleaner> logger, IVerificationCleanerService cleanerService)
        {
            _logger = logger;
            _cleanerService = cleanerService;
        }

        [Function("verificationCleaner")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            try
            {
                await _cleanerService.RemoveExpiredRecordsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: verificationCleaner.Run() :: {ex.Message}");
            }
        }
    }
}
