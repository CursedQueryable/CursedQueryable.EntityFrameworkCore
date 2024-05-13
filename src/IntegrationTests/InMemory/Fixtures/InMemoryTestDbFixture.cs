using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.InMemory.Fixtures;

[CollectionDefinition(nameof(InMemoryCollection))]
public class InMemoryCollection : ICollectionFixture<InMemoryTestDbFixture>;

public class InMemoryTestDbFixture() : TestDbFixture(Options)
{
    private static DbContextOptions<TestDbContext> Options =>
        new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("CursedQueryableTest")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
}