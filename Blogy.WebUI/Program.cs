using Blogy.Business.Extensions;
using Blogy.DataAccess.Extensions;
using Blogy.WebUI.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 

// HttpClientFactory servisini ekleyin
builder.Services.AddHttpClient();

// İsteğe bağlı: Özel HttpClient yapılandırması
builder.Services.AddHttpClient("ToxicityClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "BlogyApp");
});

builder.Services.AddHttpClient("AIClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(60);
    client.DefaultRequestHeaders.Add("User-Agent", "BlogyApp");
});

builder.Services.AddServicesExt();
builder.Services.AddRepositoriesExt(builder.Configuration);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ValidationExceptionFilter>();
});

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Login/Index";
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
app.UseAuthentication();
app.UseAuthorization();

//https:localhost:7000/Admin/Category/Index
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();