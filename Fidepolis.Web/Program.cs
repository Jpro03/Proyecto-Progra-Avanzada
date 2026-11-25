using Fidepolis.Data;
using Fidepolis.Business;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "AppData");
builder.Services.AddSingleton<ICinemaRepository>(_ => new JsonCinemaRepository(dataDirectory));
builder.Services.AddSingleton<ICinemaService, CinemaService>();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Movies}/{action=Index}/{id?}");
app.Run();
