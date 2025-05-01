using Client;
using Client.Components;
using Client.Extensions;
using Client.Services;
using Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();

builder.Services.AddClient(builder.Configuration);

builder.Services.AddScoped<CommentsHubService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    })
    .AddInteractiveWebAssemblyComponents()
    ;

builder.Services.AddServerSideBlazor();

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

app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    ;

await app.RunAsync();
