using EntityCloning.Console.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityCloning.Console.Persistence
{
    public class SandboxContext : DbContext
    {
        public SandboxContext()
        { }

        public SandboxContext(DbContextOptions<SandboxContext> options)
            : base(options)
        { }

        public virtual DbSet<SandGrainEntity> Grains { get; set; }
    }
}
