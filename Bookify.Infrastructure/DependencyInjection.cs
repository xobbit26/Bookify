using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.Repositories;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        AddPersistence(services, configuration);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = GetConnectionString(configuration);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IApartmentRepository, ApartmentRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }

    /// <summary>
    /// Retrieves the connection string from the configuration file and replaces placeholders with environmental variables.
    /// </summary>
    /// <param name="configuration">The configuration instance that provides access to the configuration settings.</param>
    /// <returns>The formatted connection string with the placeholders replaced by actual values.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the connection string is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when any of the required environment variables (DB_NAME, DB_USER, DB_PASSWORD) are not set.</exception>
    private static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
                               ?? throw new ArgumentNullException(nameof(configuration));

        var dbName = Environment.GetEnvironmentVariable("DB_NAME") ??
                     throw new InvalidOperationException("DB_NAME is not set.");

        var dbUser = Environment.GetEnvironmentVariable("DB_USER") ??
                     throw new InvalidOperationException("DB_USER is not set.");

        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ??
                         throw new InvalidOperationException("DB_PASSWORD is not set.");

        connectionString = connectionString
            .Replace("${DB_NAME}", dbName)
            .Replace("${DB_USER}", dbUser)
            .Replace("${DB_PASSWORD}", dbPassword);

        return connectionString;
    }
}