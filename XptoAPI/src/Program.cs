using Microsoft.EntityFrameworkCore;
using XptoAPI.src.Data;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Services;
using FluentValidation;
using XptoAPI.src.Validators;
using XptoAPI.src.Models;
using XptoAPI.src.Filters;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.FileProviders;
using System.Threading.RateLimiting;
using XptoAPI.src.Filters;
using XptoAPI.src.Middleware;


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
builder.Services.AddSwaggerGen(c =>
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

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "localhost",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .WithMethods("GET", "POST", "PUT", "DELETE")
              .WithExposedHeaders("X-Pagination");
    });
});

builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateFilter>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRateLimiter();
app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Append("Content-Security-Policy",
        "default-src 'self'; img-src 'self' https: data:; style-src 'self' 'unsafe-inline';");
    await next();
});
app.UseMiddleware<RequestLoggingMiddleware>();
var staticFilesPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
if (!Directory.Exists(staticFilesPath))
{
    Directory.CreateDirectory(staticFilesPath);
}
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/static"
});

app.Run();
await app.RunAsync();
