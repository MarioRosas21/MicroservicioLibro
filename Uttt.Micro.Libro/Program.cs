using Microsoft.EntityFrameworkCore;
using Uttt.Micro.Libro.Extenciones;
using Uttt.Micro.Libro.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Configurar Swagger y OpenAPI
builder.Services.AddOpenApi();     // Extensión personalizada
builder.Services.AddSwaggerGen();  // Swagger de Swashbuckle

// ?? Cadena de conexión directamente en el código
builder.Services.AddDbContext<ContextoLibreria>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirReact", policy =>
    {
        policy.WithOrigins(
    "http://localhost:3001",
    "https://localhost:3001"
) // URL de tu aplicación React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Servicios personalizados (como validadores, etc.)
builder.Services.AddCustonServices(builder.Configuration);

var app = builder.Build();

// Swagger solo en entorno de desarrollo
//if (app.Environment.IsDevelopment())
//{

//}

app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi(); // OpenAPI personalizado

// Habilitar CORS
app.UseCors("PermitirReact");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var hostname = System.Net.Dns.GetHostName();
Console.WriteLine($"Iniciando microservicio desde instancia: {hostname}");

app.Run("http://0.0.0.0:8080");
