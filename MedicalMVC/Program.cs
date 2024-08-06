using FluentValidation.AspNetCore;
using MedicalMVC.Initializers;
using MedicalMVC.Validation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RequestViewModelValidator>());

builder.Host.UseSerilog();

builder.Services.AddRazorPages();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.Initializ(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "area",
        areaName: "Admin",
        pattern: "admin/{controller=Home}/{action=Index}/{id?}"
    );

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();