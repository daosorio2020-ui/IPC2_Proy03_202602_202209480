var builder = WebApplication.CreateBuilder(args);

// Habilitar los controladores (Endpoints)
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

// Mapear las rutas de ProcesarController y ConsultasController
app.MapControllers();

app.Run();

