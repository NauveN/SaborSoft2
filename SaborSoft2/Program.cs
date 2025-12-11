using Microsoft.EntityFrameworkCore;
using SaborSoft2;
using SaborSoft2.Models;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<SaborCriolloContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SaborCriolloConnection")));

// Razor Pages
builder.Services.AddRazorPages();



var app = builder.Build();

SeedData.Initialize(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// IMPORTANTE para Razor Pages
app.MapRazorPages();

app.Run();
