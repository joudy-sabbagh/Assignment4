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

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// 1. Serilog
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
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateTicketHandler).Assembly)
);

// 6. EF Core
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services
    .AddScoped<Application.Interfaces.IEmailService, Infrastructure.Services.SendGridEmailService>();

// 7. DI
// Removed ITicketPricingService registration
builder.Services.AddScoped<IEventValidationService, EventValidationService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();

builder.Services.AddMemoryCache();

var app = builder.Build();

// In dev, show the detailed page; otherwise use our “/Error” handler
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // any exception goes to /Error
    app.UseExceptionHandler("/Error");
    // HSTS in production
    app.UseHsts();
}

// Turn 404/403/etc into our ErrorController too
app.UseStatusCodePagesWithReExecute("/Error/{0}");

// The rest of your pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();