var builder = WebApplication.CreateBuilder(args);

// Habilita los controladores y las vistas Razor.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Muestra una página de error y habilita HSTS fuera del entorno de desarrollo.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Investigation/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Abre la lista de investigaciones como página principal.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Investigation}/{action=Index}/{id?}");

app.Run();