using System.Globalization;
using System.Reflection;
using Application.UseCases.Tickets;
using Application.Mapping;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1) Configure Serilog before building the host
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// 2) Enforce en-US culture
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

// 3) Add MVC
builder.Services.AddControllersWithViews();

// 4) AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 5) MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetAllTicketsHandler).Assembly);
});

// 6) EF Core / SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 7) DI for your Repositories
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();

var app = builder.Build();

// 8) Localization
var supportedCultures = new[] { new CultureInfo("en-US") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// 9) Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 10) Endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// 11) Start & flush logs
app.Run();
Log.CloseAndFlush();
