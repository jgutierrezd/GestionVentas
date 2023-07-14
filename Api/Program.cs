using ApiGestionVentas.Data;
using ApiGestionVentas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<GestionVentasDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/products", async (GestionVentasDbContext db) =>
 await db.Producto.ToListAsync()
);

app.Run();