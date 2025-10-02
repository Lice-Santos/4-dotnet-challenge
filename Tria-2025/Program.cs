using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tria_2025.Connection;
using Tria_2025.Repository;
using Tria_2025.Services;
using Tria_2025.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ⭐️ Configuração Detalhada do Swagger/OpenAPI (Igual ao Exemplo) ⭐️
builder.Services.AddSwaggerGen(configutionSwagger =>
{
    configutionSwagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tria API",
        Version = "v1",
        Description = "API de gerenciamento de frotas e filiais da Tria. \r\nInclui validações de domínio, injeção de dependência e documentação OpenAPI.",

        Contact = new OpenApiContact
        {
            Name = "Alice",
            Email = "alicenuunes05@gmail.com"
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    configutionSwagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


// === Configuração de Banco de Dados (Mantido) ===
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));


// === Injeção de Dependência: Repositórios e Serviços ===

// Repositórios
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<ISetorRepository, SetorRepository>();
builder.Services.AddScoped<IFilialRepository, FilialRepository>();
builder.Services.AddScoped<IMotoSetorRepository, MotoSetorRepository>();
builder.Services.AddScoped<IMotoRepository, MotoRepository>();

// Validações (Classes com dependências)
builder.Services.AddScoped<FuncionarioValidation>();
builder.Services.AddScoped<SetorValidation>();
builder.Services.AddScoped<FilialValidation>();
builder.Services.AddScoped<MotoValidation>();
builder.Services.AddScoped<MotoSetorValidation>();

// Serviços (Camada de Negócio)
builder.Services.AddScoped<FuncionarioService>();
builder.Services.AddScoped<EnderecoService>();
builder.Services.AddScoped<SetorService>();
builder.Services.AddScoped<FilialService>();
builder.Services.AddScoped<MotoService>();
builder.Services.AddScoped<MotoSetorService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // ⭐️ Configuração do Swagger UI (Igual ao Exemplo) ⭐️
    app.UseSwaggerUI(c =>
    {
        // Redireciona a raiz da aplicação para a interface do Swagger
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tria API V1");
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();