using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tria_2025.Connection;
using Tria_2025.Interface;
using Tria_2025.Repository;
using Tria_2025.Services;
using Tria_2025.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
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
    configutionSwagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {token}"
    });

    configutionSwagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    configutionSwagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


// === Configuração de Banco de Dados (Mantido) ===
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<ITokenService, TokenService>();

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

builder.Services.AddHealthChecks();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Define que o emissor ('iss') do token deve ser checado.
            ValidateIssuer = true,

            // Define que a audiência ('aud') do token deve ser checada.
            ValidateAudience = true,

            // Garante que o token não está expirado ('exp') e é válido no tempo atual.
            ValidateLifetime = true,

            // Garante que a assinatura do token é válida, evitando adulteração do token.
            ValidateIssuerSigningKey = true,

            // O valor esperado para o emissor (appsettings).
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            // O valor exato esperado para a audiência (appsettings).
            ValidAudience = builder.Configuration["Jwt:Audience"],

            // A Chave Secreta usada para verificar a assinatura digital do token, vindo da config
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>("BancoDeDados");

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
// Endpoint de Health Check
app.MapHealthChecks("/health");


app.Run();