using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SchedulerApp.Data;
using MudBlazor;
using MudBlazor.Services;
using SchedulerApp.Modules;
using SchedulerApp.Pages.Fundamental;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<Constants>();
builder.Services.AddSingleton<ContentStorage>();
builder.Services.AddSingleton<Exporter>();
builder.Services.AddSingleton<Importer>();
builder.Services.AddSingleton<SchedulingAPIService>();
builder.Services.AddMudServices();

//builder.Services.AddMudServices(config =>
//{
//    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

//    config.SnackbarConfiguration.PreventDuplicates = false;
//    config.SnackbarConfiguration.NewestOnTop = false;
//    config.SnackbarConfiguration.ShowCloseIcon = true;
//    config.SnackbarConfiguration.VisibleStateDuration = 10000;
//    config.SnackbarConfiguration.HideTransitionDuration = 500;
//    config.SnackbarConfiguration.ShowTransitionDuration = 500;
//    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Fundamental/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/Fundamental/_Host");

app.Run();
