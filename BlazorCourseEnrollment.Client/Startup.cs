using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<StudentService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddOptions();
builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

await builder.Build().RunAsync();