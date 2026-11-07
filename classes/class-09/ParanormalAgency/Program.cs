var builder = WebApplication.CreateBuilder(args);

// Registra los controladores y las vistas MVC.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura la página de error y HTTPS cuando no se ejecuta en desarrollo.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Investigation/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Investigation}/{action=Index}/{id?}");

app.Run();