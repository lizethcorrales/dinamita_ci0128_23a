using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models.Dependency_Injection;
using JunquillalUserSystem.Models.Patron_Bridge;

var builder = WebApplication.CreateBuilder(args);


// Registrar las dependencias y la inyecci�n de dependencias
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<IMensajeConfirmacionImplementacion,MensajeConfirmacionImplementacionHTML>();
builder.Services.AddScoped<ReportesHandler>();
builder.Services.AddScoped<LoginHandler>(); 
builder.Services.AddScoped<TarifasHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Login}/{action=Login}/{id?}"
    );

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});



app.Run();
