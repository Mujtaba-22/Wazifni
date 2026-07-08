using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wazifni.Data;
using Wazifni.Models;
using Wazifni;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

//Language
builder.Services.AddLocalization(options => options.ResourcesPath =
"Resources");


builder.Services.AddControllersWithViews()
.AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
.AddDataAnnotationsLocalization(options =>
{
options.DataAnnotationLocalizerProvider = (type, factory) =>
factory.Create(typeof(SharedResource));
});

var supportedCultures = new[] { "ar", "en-US" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
options.SetDefaultCulture(supportedCultures[0]);
options.AddSupportedCultures(supportedCultures);
options.AddSupportedUICultures(supportedCultures);
// المتصفح لغة مزود إزالة
var browserLanguageProvider = options.RequestCultureProviders
.OfType<Microsoft.AspNetCore.Localization.AcceptLanguageHeaderRequestCultureProvider>()
.FirstOrDefault();
if (browserLanguageProvider != null)
{
options.RequestCultureProviders.Remove(browserLanguageProvider);
}
});


// Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    await DbSeeder.SeedAsync(dbContext);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//Language
app.UseRequestLocalization();

// Identity
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();