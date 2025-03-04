using AntDesign;
using Client;
using Client.Components;
using Client.Extensions;
using Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();

builder.Services.AddClient(builder.Configuration);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
;

LocaleProvider.DefaultLanguage = "en-US";

var app = builder.Build();

app.UseMiddleware<CookieMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.UseStaticFiles();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

await app.RunAsync();
