
using Microsoft.EntityFrameworkCore;
using PublicCMS_API.Data;
using PublicCMS_API.Repositories;
using PublicCMS_API.Services;

namespace PublicCMS_API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContextFactory<ApiDbContext>(options =>
            options.UseSqlServer(connectionString,
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "api")
        ));

        builder.Services.AddScoped<GalleryRepository>();

        builder.Services.AddScoped<GalleryService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.UseStaticFiles();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
            db.Database.Migrate();
        }

        app.Run();
    }
}
