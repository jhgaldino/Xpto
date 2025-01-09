using Microsoft.EntityFrameworkCore;
using XptoAPI.src.Data;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Services;
using FluentValidation;
using XptoAPI.src.Validators;
using XptoAPI.src.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<XptoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IValidator<Pedido>, PedidoValidator>();
builder.Services.AddScoped<IValidator<MenuItem>, MenuItemValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "XptoAPI",
        Version = "v1",
        Description = "API para o restaurante Xpto",
    });
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));

});

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
