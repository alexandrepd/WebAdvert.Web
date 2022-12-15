using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Configuration;
using WebAdvert.Web.Configuration;
using WebAdvert.Web.ServiceClients;
using WebAdvert.Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
});

//inject automapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<AWS>(builder.Configuration.GetSection(nameof(AWS)));
builder.Services.AddSingleton(x => x.GetRequiredService<IOptions<AWS>>().Value);

builder.Services.Configure<AdvertApi>(builder.Configuration.GetSection(nameof(AdvertApi)));
builder.Services.AddSingleton(x => x.GetRequiredService<IOptions<AdvertApi>>().Value);

builder.Services.AddTransient<IFileUploader, S3FileUploader>();

builder.Services.AddHttpClient<IAdvertApiClient, AdvertApiClient>()
    .AddPolicyHandler(GetRetryPolice())
    .AddPolicyHandler(GetCircuitBreakerPattern());

IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPattern()
{
    return  HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 3, durationOfBreak: TimeSpan.FromSeconds(30));
}

IAsyncPolicy<HttpResponseMessage> GetRetryPolice()
{
    return HttpPolicyExtensions.HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(retryCount: 5, sleepDurationProvider: retryAttenpy => TimeSpan.FromSeconds(Math.Pow(2, retryAttenpy)));
}

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
