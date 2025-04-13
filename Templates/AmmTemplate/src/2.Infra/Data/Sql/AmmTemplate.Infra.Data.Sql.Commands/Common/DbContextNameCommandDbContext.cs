using AmmFramework.Extensions.Events.Outbox.Dal.EF;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AmmTemplate.Infra.Data.Sql.Commands.Common;

public class DbContextNameCommandDbContext : BaseOutboxCommandDbContext
{
    public DbContextNameCommandDbContext(DbContextOptions<DbContextNameCommandDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}