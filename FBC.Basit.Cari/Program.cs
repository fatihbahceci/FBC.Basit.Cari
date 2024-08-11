//using FBC.Basit.Cari.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using FBC.Basit.Cari.Auth;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Radzen;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
FBC.Basit.Cari.DBModels.DB.MigrateDB();
// Add services to the container.
//builder.Services.AddCookiePolicy
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, FBCSessionedAuthenticationStateProvider>();
builder.Services.AddSingleton<CircuitHandler, FBCCircuitHandlerService>();
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("tr-TR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("tr-TR");
//builder.Services.AddLocalization();

var app = builder.Build();



//https://stackoverflow.com/questions/63038712/httpcontext-connection-remoteipaddress-returns-127-0-0-1-on-the-server
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

//https://www.gencayyildiz.com/blog/asp-net-core-httpshypertext-transfer-protocol-secure-ve-hstshttp-strict-transport-security-nedir/
app.UseHsts();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
