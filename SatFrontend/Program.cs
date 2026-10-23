var builder = WebApplication.CreateBuilder(args);

// Habilitar las páginas de interfaz gráfica (Razor Pages)
builder.Services.AddRazorPages();

// Configurar la conexión hacia tu API en el puerto 5115
builder.Services.AddHttpClient("SatApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5115");
});

var app = builder.Build();

// Configuración estándar de la interfaz web
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();