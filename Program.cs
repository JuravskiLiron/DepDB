using DepDB.Data;
using DepDB.Models;
using DepDB.Services;

var builder = WebApplication.CreateBuilder(args);

// Mongo configuration
builder.Services.Configure<MongoOptions>(
    builder.Configuration.GetSection("Mongo"));

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

// Services
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.AddSingleton<IRepository<UserAccount>, MongoRepository<UserAccount>>();
builder.Services.AddSingleton<IRepository<Shift>, MongoRepository<Shift>>();
builder.Services.AddSingleton<IRepository<IncidentReport>, MongoRepository<IncidentReport>>();
builder.Services.AddSingleton<PasswordHasher>();

// MVC
builder.Services.AddControllersWithViews();

// Authentication (COOKIE)
builder.Services.AddAuthentication("LspdCookie")
    .AddCookie("LspdCookie", opt =>
    {
        opt.LoginPath = "/Account/Login";
        opt.AccessDeniedPath = "/Account/AccessDenied";

        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

        opt.ExpireTimeSpan = TimeSpan.FromDays(7);      // <— COOKIE ЖИВЁТ 7 ДНЕЙ
        opt.SlidingExpiration = true;                   // <— ОБНОВЛЯЕТСЯ ПРИ КАЖДОМ ВИЗИТЕ
    });


builder.Services.AddAuthorization();

var app = builder.Build();

// Error handler for Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// DEFAULT ROUTING
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Officer}/{action=Dashboard}/{id?}");



app.Run();



app.Run();
