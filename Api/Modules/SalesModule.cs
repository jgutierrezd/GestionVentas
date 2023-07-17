using ApiGestionVentas.Contracts;
using ApiGestionVentas.Data;
using ApiGestionVentas.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionVentas.Modules
{
    public class SalesModule : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/sales", async (GestionVentasDbContext dbContext) =>
            {
                var items = await dbContext.Venta.ToListAsync();
                if (items == null)
                {
                    return Results.NoContent();
                }
                return Results.Ok(items);

            }).RequireAuthorization();

            endpoints.MapGet("/sales/{id}", async (int id, GestionVentasDbContext dbContext) =>
            {
                var itemModel = await dbContext.Venta.FindAsync(id);
                if (itemModel == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(itemModel);

            }).RequireAuthorization();

            endpoints.MapPost("/sales", async ([FromBody] VentaDto item, GestionVentasDbContext dbContext) =>
            {
                var itemModel = new ApiGestionVentas.Models.Venta { Periodo = item.Periodo, Monto = item.Monto, FechaOperacion = DateTime.Now };

                var result = dbContext.Venta.Add(itemModel);

                await dbContext.SaveChangesAsync();

                return Results.Ok(result.Entity);

            }).RequireAuthorization();


            return endpoints;
        }

        public IServiceCollection RegisterModule(IServiceCollection services)
        {
            return services;
        }
    }
}
