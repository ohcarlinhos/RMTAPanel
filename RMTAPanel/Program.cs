using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Blazored.LocalStorage;
using App;
using App.Providers;
using App.Services;
using App.Utils;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<AppBase>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!)
});

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton(builder.HostEnvironment);
builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddScoped<CustomAuthStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>((provider) =>
    provider.GetRequiredService<CustomAuthStateProvider>()
);

builder.Services.AddScoped<AuthenticateService>();

builder.Services.AddScoped<HttpErrorSnackbarHandle>();
builder.Services.AddScoped<UrlQueriesHandle>();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();


await builder.Build().RunAsync();