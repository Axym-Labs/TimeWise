using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TimeWise.Data;
using MudBlazor;
using MudBlazor.Services;
using TimeWise.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using TimeWise.Modules.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<Constants>();
builder.Services.AddSingleton<ContentStorage>();
builder.Services.AddSingleton<InputsExporter>();
builder.Services.AddSingleton<ScheduleExporter>();
builder.Services.AddSingleton<InputsImporter>();
builder.Services.AddSingleton<SchedulingAPIService>();
builder.Services.AddMudServices();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 8000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

var app = builder.Build();


#if DEBUG
Postgres.ConnectionString = builder.Configuration["Database:ConnectionStringTesting"]!;
#else
Postgres.ConnectionString = builder.Configuration["Database:ConnectionStringProduction"]!;
#endif

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Fundamental/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "static")),
        RequestPath = "/static"
    });

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/Fundamental/_Host");

app.Run();
