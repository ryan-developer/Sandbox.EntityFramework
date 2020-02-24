using System;
using System.Linq;
using EntityCloning.Console.Persistence;
using EntityCloning.Console.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityCloning.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var inMemoryContextOptions = new DbContextOptionsBuilder<SandboxContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            using(var context = new SandboxContext(inMemoryContextOptions))
            {
                var beach = new BeachEntity();
                beach.Id = Guid.NewGuid();

                for(int i = 0; i < 5; i++)
                {
                    var grain = new SandGrainEntity
                    {
                        CreatedDate = DateTime.UtcNow,
                        Id = Guid.NewGuid()
                    };
                    beach.Grains.Add(grain);
                }

                context.Database.EnsureCreated();
                context.Beaches.Add(beach);
                int savedCount = context.SaveChanges();

                System.Console.WriteLine();
                System.Console.WriteLine($"Created {savedCount} new entries.");
                System.Console.WriteLine();

                WriteBeach(beach);

                BeachEntity clonedBeach = CloneBeach(context, beach);
                context.Beaches.Add(clonedBeach);

                savedCount = context.SaveChanges();
                System.Console.WriteLine();
                System.Console.WriteLine($"Created {savedCount} new entries.");
                System.Console.WriteLine();
                WriteBeach(clonedBeach);
            }
        }

        private static BeachEntity CloneBeach(SandboxContext context, BeachEntity original)
        {
            var cloned = new BeachEntity();
            var cloneValues = context.Entry(original).CurrentValues.Clone();
            context.Entry(cloned).CurrentValues.SetValues(cloneValues);
            cloned.InternalId = 0;
            cloned.Id = Guid.NewGuid();

            cloned.Grains = original.Grains.Select(d => CloneGrain(context, d)).ToList();
            return cloned;
        }

        private static SandGrainEntity CloneGrain(SandboxContext context, SandGrainEntity original)
        {
            var clonedGrain = new SandGrainEntity();
            var cloneValues = context.Entry(original).CurrentValues.Clone();
            context.Entry(clonedGrain).CurrentValues.SetValues(cloneValues);
            clonedGrain.InternalId = 0;
            clonedGrain.Id = Guid.NewGuid();

            return clonedGrain;
        }

        private static void WriteBeach(BeachEntity beach)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($" BEACH ADDED");
            System.Console.WriteLine($" Internal ID:  {beach.InternalId}");
            System.Console.WriteLine($" Primary Key:  {beach.Id}");
            System.Console.WriteLine($" Created Date: {beach.CreatedDate.ToString("s")}");
            foreach(var grain in beach.Grains)
            {
                WriteSandGrain(grain);
            }
            System.Console.WriteLine();
        }

        private static void WriteSandGrain(SandGrainEntity grain)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"    GRAIN ADDED");
            System.Console.WriteLine($"    Internal ID:  {grain.InternalId}");
            System.Console.WriteLine($"    Primary Key:  {grain.Id}");
            System.Console.WriteLine($"    Created Date: {grain.CreatedDate.ToString("s")}");
            System.Console.WriteLine();
        }
    }
}
