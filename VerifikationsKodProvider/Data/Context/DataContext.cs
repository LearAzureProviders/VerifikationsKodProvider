

using Microsoft.EntityFrameworkCore;
using VerifikationsKodProvider.Data.Entities;

namespace VerifikationsKodProvider.Data.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {


        public DbSet<VerificationRequestEntity> VerificationRequests { get; set; }
    }
}
