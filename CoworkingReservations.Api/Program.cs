using System.Reflection;
using AutoMapper;
using CoworkingReservations.Api.Middleware;
using CoworkingReservations.Core.Interfaces;
using CoworkingReservations.Infrastructure.Data;
using CoworkingReservations.Infrastructure.Persistence;
using CoworkingReservations.Infrastructure.Repositories;
using CoworkingReservations.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

// AJUSTA el using al namespace real de tu TaskManagerContext:
// según tu doc TaskManagerContext está en "CoworkingReservations.Infrastructure.Repositories"
// pero lo correcto semánticamente sería "CoworkingReservations.Infrastructure" o ".Persistence".
// Si TaskManagerContext está en Infrastructure.Repositories, usa ese; si lo moviste, cambia aquí.
//using CoworkingReservations.Infrastructure.Persistence; // <- si TaskManagerContext está aquí

var builder = WebApplication.CreateBuilder(args);

// --- Services
builder.Services.AddControllers()
    .AddNewtonsoftJson(); // si usas NewtonSoft (tienes referencia), ayuda con fechas/serialización

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext: usa TaskManagerContext scaffolded (lee DefaultConnection)
builder.Services.AddDbContext<TaskManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 1. Registros de Dapper
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IDapperContext, DapperContext>();

// 2. Registro del Patrón Unit of Work
//    Transient es común para UoW si se usa por request, 
//    Scoped también es válido si se maneja cuidadosamente.
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// 3. Registro del Repositorio Genérico (BaseRepository)
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
// 4. Registro de Repositorios Específicos (los que tienen lógica extra)
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
// (Si UserRepository y SpaceRepository no tienen lógica extra,
// no necesitas registrarlos, se usarán vía IUnitOfWork)

// 5. Registro de tus Services (que ahora inyectan IUnitOfWork)
builder.Services.AddScoped<IReservationService, ReservationService>();


// AutoMapper: busca Profiles en todos los assemblies (Core/Api)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// FluentValidation: registra validadores del proyecto Api (o cambia assembly si están en otro)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Repos (implementaciones reales en Infrastructure)
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IReservationRepository, ReservationRepository>(); // <-- Esta se queda


// Service (implementación en Infrastructure)
builder.Services.AddScoped<IReservationService, ReservationService>();

var app = builder.Build();

// Middleware (manejo global de excepciones)
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
