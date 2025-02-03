using Client;
using Client.Components;
using Services;
using Services.Interfaces;
using Refit;
using Microsoft.Extensions.Options;
using Client.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<TaskTrackerSettings>()
    .BindConfiguration(TaskTrackerSettings.ConfigurationSection)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddApi(builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
