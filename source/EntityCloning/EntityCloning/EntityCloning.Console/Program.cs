using System;
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
                var grain = new SandGrainEntity
                {
                    CreatedDate = DateTime.UtcNow,
                    Id = Guid.NewGuid()
                };

                context.Database.EnsureCreated();
                context.Grains.Add(grain);
                context.SaveChanges();

                WriteSandGrain(grain);

                var clonedGrain = new SandGrainEntity();

                var cloneValues = context.Entry(grain).CurrentValues.Clone();
                context.Entry(clonedGrain).CurrentValues.SetValues(cloneValues);
                clonedGrain.InternalId = 0;
                clonedGrain.Id = Guid.NewGuid();

                context.Grains.Add(clonedGrain);

                context.SaveChanges();
                WriteSandGrain(clonedGrain);
            }
        }

        private static void WriteSandGrain(SandGrainEntity grain)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"Internal ID:  {grain.InternalId}");
            System.Console.WriteLine($"Primary Key:  {grain.Id}");
            System.Console.WriteLine($"Created Date: {grain.CreatedDate.ToString("s")}");
            System.Console.WriteLine();
        }
    }
}
