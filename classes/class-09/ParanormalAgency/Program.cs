var builder = WebApplication.CreateBuilder(args);

// Registrar servicios MVC como en Class 08.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Mantener el pipeline de Class 08; el examen requiere eliminar Home/Privacy.
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