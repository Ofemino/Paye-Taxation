using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Paye.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:53320/") });

// Authentication & Authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<Paye.Client.Services.ILocalStorageService, Paye.Client.Services.LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, Paye.Client.Services.CustomAuthStateProvider>();
builder.Services.AddScoped<Paye.Client.Services.ApiClient>();

await builder.Build().RunAsync();
