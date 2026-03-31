using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Servicios;
using DA;
using DA.Repositorios;
using Flujo;
using Reglas;
using Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Autorizacion.Middleware;
using Abstracciones.Modelos;


var builder = WebApplication.CreateBuilder(args);

var tokenConfig = builder.Configuration.GetSection("Token").Get<TokenConfiguracion>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenConfig.Issuer,
            ValidAudience = tokenConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(tokenConfig.key))
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IProductoFlujo, ProductoFlujo>();
builder.Services.AddScoped<IProductoDA, ProductoDA>();
builder.Services.AddScoped<ICategoriaFlujo, CategoriaFlujo>();
builder.Services.AddScoped<ICategoriaDA, CategoriaDA>();
builder.Services.AddScoped<ISubcategoriaFlujo, SubcategoriaFlujo>();
builder.Services.AddScoped<ISubcategoriaDA, SubcategoriaDA>();

builder.Services.AddScoped<IRepositorioDapper, RepositorioDapper>();
builder.Services.AddScoped<IConfiguracion, Configuracion>();
builder.Services.AddScoped<ITipoCambioServicio, TipoCambioServicio>();
builder.Services.AddScoped<IProductosReglas, ProductoReglas>();

// ★ Registrar servicios del paquete de Autorización
builder.Services.AddTransient<Autorizacion.Abstracciones.Flujo.IAutorizacionFlujo,
                               Autorizacion.Flujo.AutorizacionFlujo>();
builder.Services.AddTransient<Autorizacion.Abstracciones.DA.ISeguridadDA,
                               Autorizacion.DA.SeguridadDA>();
builder.Services.AddTransient<Autorizacion.Abstracciones.DA.IRepositorioDapper,
                               Autorizacion.DA.Repositorios.RepositorioDapper>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AutorizacionClaims();
app.UseAuthorization();

app.MapControllers();

app.Run();
