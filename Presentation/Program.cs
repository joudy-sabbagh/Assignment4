// Program.cs
using System.Globalization;
using Application.Mapping;
using Application.UseCases.Attendees;
using Application.UseCases.Tickets;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Bootstrap Serilog from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();

// 2. Culture
var enCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = enCulture;
CultureInfo.DefaultThreadCurrentUICulture = enCulture;

// 3. MVC
builder.Services.AddControllersWithViews();

// 4. AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 5. MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateAttendeeHandler).Assembly)
);
// 6. EF Core
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 7. DI
builder.Services.AddScoped<ITicketPricingService, TicketPricingService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();

var app = builder.Build();

// 8. Localization
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(enCulture),
    SupportedCultures = new[] { enCulture },
    SupportedUICultures = new[] { enCulture }
});

// 9. Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 10. Endpoints
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// 11. Run
try
{
    Log.Information("Application starting up");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
