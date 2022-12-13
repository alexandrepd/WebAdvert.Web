using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using WebAdvert.Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
});

builder.Services.AddScoped<IFileUploader, S3FileUploader>();


// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddCognitoIdentity();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}   
app.UseStaticFiles();

app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
