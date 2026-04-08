using CoilTrackingLocationManagementSystem.Models;
using CoilTrackingLocationManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<DataAccessOptions>(builder.Configuration.GetSection(DataAccessOptions.SectionName));
builder.Services.AddSingleton<ITrackingRepository>(_ =>
{
    var provider = builder.Configuration.GetSection(DataAccessOptions.SectionName).Get<DataAccessOptions>()?.Provider ?? "InMemory";
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

    return provider.Equals("MySql", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(connectionString)
        ? new MySqlTrackingRepository(connectionString)
        : new InMemoryTrackingRepository();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
