using MS.Transferencias.API.Filters;
using MS.Transferencias.Application;
using MS.Transferencias.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureApplication(builder.Configuration);
builder.Services.ConfigureInfrastructure(builder.Configuration, builder.Logging);

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MS.Transferencias.API", Version = "v1" });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MS.Transferencias.API.xml"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Quitar comentarios si se busca utilizar Migrations para generar la base de datos
//if (app.Environment.IsDevelopment())
//    app.AddMigrations();

app.Run();
