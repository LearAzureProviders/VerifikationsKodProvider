using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VerifikationsKodProvider.Data.Context;
using VerifikationsKodProvider.Functions;

namespace VerifikationsKodProvider.Services
{
    public class VerificationCleanerService : IVerificationCleanerService
    {
        private readonly ILogger<VerificationCleaner> _logger;
        private readonly DataContext _context;

        public VerificationCleanerService(ILogger<VerificationCleaner> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task RemoveExpiredRecordsAsync()
        {
            try
            {
                var expired = await _context.VerificationRequests
                    .Where(x => x.ExpiryDate <= DateTime.Now)
                    .ToListAsync();

                _context.RemoveRange(expired);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: VerificationCleanerService.RemoveExpiredRecordsAsync() :: {ex.Message}");
            }
        }
    }
}
