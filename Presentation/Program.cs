// Presentation/Program.cs
using System;
using System.Globalization;
using System.Text;
using Application.Mapping;
using Application.UseCases.Attendees;
using Application.UseCases.Tickets;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Application.Interfaces;
using Infrastructure.Services;
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

// 7. Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opts =>
{
    // password policy (tweak as needed)
    opts.Password.RequireDigit = true;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 8. JWT Authentication
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// 9. DI for your services/repositories
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddScoped<IEventValidationService, EventValidationService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();

builder.Services.AddMemoryCache();

var app = builder.Build();

// error pages
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// **Enable auth middleware**
app.UseAuthentication();
app.UseAuthorization();

// your MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // 1) ensure roles exist
    foreach (var roleName in new[] { "Admin", "User" })
        if (!await roleMgr.RoleExistsAsync(roleName))
            await roleMgr.CreateAsync(new ApplicationRole { Name = roleName });

    // 2) ensure an admin user exists
    const string adminEmail = "Joudy.f.sabbagh@gmail.com";
    var admin = await userMgr.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "System",
            LastName = "Admin",
            DateOfBirth = DateTime.UtcNow
        };
        var result = await userMgr.CreateAsync(admin, "Admin123!");
        if (result.Succeeded)
            await userMgr.AddToRoleAsync(admin, "Admin");
    }
}

app.Run();