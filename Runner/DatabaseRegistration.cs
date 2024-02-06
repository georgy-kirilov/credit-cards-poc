using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Runner;

public static class DatabaseRegistration
{
    public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services,
        string? connectionString,
        string schema)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString, npgsqlOptions =>
            {
                npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema)
                    .MigrationsAssembly(typeof(TContext).Assembly.FullName);
            });
        });

        return services;
    }
}
