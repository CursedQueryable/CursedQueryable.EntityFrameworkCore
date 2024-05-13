[![CI](https://github.com/CursedQueryable/CursedQueryable.EntityFrameworkCore/actions/workflows/ci.yml/badge.svg)](https://github.com/CursedQueryable/CursedQueryable/actions/workflows/ci.yml)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/FrontierFox/c5a4a1966f7d7aecbf95ff42f233a47d/raw/cq-efcore-coverage.json)
[![NuGet](http://img.shields.io/nuget/vpre/CursedQueryable.EntityFrameworkCore.svg?label=NuGet)](https://www.nuget.org/packages/CursedQueryable.EntityFrameworkCore/)
[![License](https://img.shields.io/github/license/CursedQueryable/CursedQueryable.EntityFrameworkCore)](https://github.com/CursedQueryable/CursedQueryable.EntityFrameworkCore/blob/main/LICENSE.md)

### What is CursedQueryable.EntityFrameworkCore?

CursedQueryable is a library that aims to implement cursor-based keyset pagination (aka seek pagination) for IQueryable
with as close to zero boilerplate code as possible. This is achieved via examining the underlying expression tree for
any `IQueryable` instance and rewriting it as needed prior to any database calls being made.

This repository contains the implementation for using CursedQueryable with Entity Framework Core, and is the primary
expected use case for CursedQueryable. If you're wanting to integrate with a different `IQueryable` provider than
EFCore, you should grab the core library from the [CursedQueryable repository](https://github.com/CursedQueryable/CursedQueryable) instead.

### Compatability

CursedQueryable.EntityFrameworkCore is compatible with Entity Framework Core versions 5 and above, and should work with
any provider that plugs into it. It has been verified as working against the following specific providers:

| Provider                                                                                                          | Databases      | Notes                                                                                                                                |
|-------------------------------------------------------------------------------------------------------------------|----------------|--------------------------------------------------------------------------------------------------------------------------------------|
| [Pomelo.EntityFrameworkCore.MySql](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql)               | MariaDB, MySQL |                                                                                                                                      |
| [MySql.EntityFrameworkCore](https://www.nuget.org/packages/MySql.EntityFrameworkCore)                             | MySql          | <small><span style="color:#FC0">⚠</span> EFCore 5 only: primary key(s) and ordering columns cannot be of type `System.Guid`.</small> | 
| [Oracle.EntityFrameworkCore](https://www.nuget.org/packages/Oracle.EntityFrameworkCore)                           | Oracle         | <small>Ensure `NullBehaviour.LargerThanNonNullable` is set.</small>                                                                  |
| [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL)     | Postgres       | <small>Ensure `NullBehaviour.LargerThanNonNullable` is set.</small>                                                                  |
| [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer) | SQL Server     |                                                                                                                                      |

### Getting started

Reference the `CursedQueryable.EntityFrameworkCore` package, make sure it's [configured](https://github.com/CursedQueryable/CursedQueryable?tab=readme-ov-file#configuration) correctly, then
just call the CursedQueryable extension methods `.ToPage()`/`.ToPageAsync()` instead of `.ToList()`/`.ToListAsync()`:

```csharp
using CursedQueryable.Extensions;

public async Task<Page<Cat>> GetPageOfCats(string? cursor)
{
    return await dbContext
        .Cats
        .Take(10)
        .ToPageAsync(cursor);
}

// This will get an initial page of (up to) 10 cats
var pageOfCats = await GetPageOfCats(null);
```

Additional documentation is available in the [CursedQueryable repository](https://github.com/CursedQueryable/CursedQueryable).