using Altinn.ApiClients.Dan.Extensions;
using Altinn.ApiClients.Maskinporten.Extensions;
using Altinn.ApiClients.Maskinporten.Services;
using bransjekartlegging.Services;
using bransjekartlegging.Services.Interfaces;
using eduediligence.Services;
using eduediligence.Services.Interfaces;
using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Authorization;
using Polly;
using System.Security.Claims;
using eduediligence.Config;using Microsoft.AspNetCore.HttpsPolicy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

var configurationRoot = builder.Configuration;
builder.Services.Configure<AuthSettings>(configurationRoot);

builder.Services.AddSingleton<ISearchService, SearchService>();
builder.Services.AddSingleton<IDanDatasetService, DanDatasetService>();
builder.Services.AddSingleton<IValidationService, ValidationService>();

builder.Services.RegisterMaskinportenClientDefinition<SettingsJwkClientDefinition>("eduediligenceDan",
    builder.Configuration.GetSection("MaskinportenSettingsForDanClient"));

builder.Services
    .AddDanClient(builder.Configuration.GetSection("DanSettings"))
    .AddMaskinportenHttpMessageHandler<SettingsJwkClientDefinition>("eduediligenceDan");

builder.Services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
    .AddBasic(options =>
    {
        options.Realm = "Basic";
        //options.SuppressWWWAuthenticateHeader = true;
        options.AllowInsecureProtocol = true;
        options.Events = new BasicAuthenticationEvents
        {
            OnValidateCredentials = context =>
            {

                var validationService = context.HttpContext.RequestServices.GetService<IValidationService>();

                if (validationService.Validate(context.Username, context.Password))
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, context.Username, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                        new Claim(ClaimTypes.Name, context.Username, ClaimValueTypes.String, context.Options.ClaimsIssuer)
                    };

                    context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                    context.Success();
                }
                else
                {
                    context.Fail("Authentication failed");
                }

                return Task.CompletedTask;
            }
        };
    });
//builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/authenticate", [Authorize] (ClaimsPrincipal user) => Results.Redirect("/"));
app.MapGet("/unauthorized", () => Results.Unauthorized());

app.MapRazorPages();

app.Run();
