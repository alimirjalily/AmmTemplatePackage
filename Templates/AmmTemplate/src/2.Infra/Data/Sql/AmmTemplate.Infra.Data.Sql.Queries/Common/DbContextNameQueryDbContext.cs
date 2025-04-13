using AmmFramework.Infra.Data.Sql.Queries;
using Microsoft.EntityFrameworkCore;

namespace AmmTemplate.Infra.Data.Sql.Queries.Common;

public class DbContextNameQueryDbContext : BaseQueryDbContext
{
    public DbContextNameQueryDbContext(DbContextOptions<DbContextNameQueryDbContext> options) : base(options)
    {
    }
}