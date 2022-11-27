using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCognitoIdentity();

//builder.Services.AddScoped<SignInManager<CognitoUser>>();
//builder.Services.AddScoped<UserManager<CognitoUser>>();
//builder.Services.AddScoped<CognitoUserPool>();



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
