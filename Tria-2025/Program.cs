
using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.Repository;
using Tria_2025.Services;
using Tria_2025.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<ISetorRepository, SetorRepository>();
builder.Services.AddScoped<IFilialRepository, FilialRepository>();
builder.Services.AddScoped<IMotoSetorRepository, MotoSetorRepository>();
builder.Services.AddScoped<IMotoRepository, MotoRepository>();

// Valida��es que usam Reposit�rios (s�o classes normais, n�o est�ticas)
builder.Services.AddScoped<FuncionarioValidation>();
builder.Services.AddScoped<SetorValidation>();
builder.Services.AddScoped<FilialValidation>();
builder.Services.AddScoped<MotoValidation>();
builder.Services.AddScoped<MotoSetorValidation>();
// A valida��o de EnderecoValidation � est�tica, ent�o n�o precisa ser registrada.

// === REGISTRO DOS SERVICES (Adicionado) ===
// Os Services injetam Reposit�rios e Valida��es
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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
