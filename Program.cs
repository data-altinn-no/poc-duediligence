using bransjekartlegging.Services;
using bransjekartlegging.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IMunicipalitiesService, MunicipalitiesService>();
builder.Services.AddSingleton<IIndustryCodesService, IndustryCodesService>();
builder.Services.AddSingleton<IEnhetsregisterService, EnhetsregisterService>();
builder.Services.AddSingleton<IRegnskapsregisterService, RegnskapsregisterService>();
builder.Services.AddSingleton<ISearchService, SearchService>();

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

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/api/municipalities", async (IMunicipalitiesService service) 
    => Results.Ok(await service.GetMunicipalities()));

app.MapGet("/api/municipalities/search/{query}", async (string query, IMunicipalitiesService service) 
    => Results.Ok(await service.SearchMunicipalities(query)));

app.MapGet("/api/industrycodes", async (IIndustryCodesService service)
    => Results.Ok(await service.GetIndustryCodes()));

app.MapGet("/api/industrycodes/search/{query}", async (string query, IIndustryCodesService service)
    => Results.Ok(await service.SearchIndustryCodes(query)));


app.Run();
