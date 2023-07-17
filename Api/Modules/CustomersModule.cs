using ApiGestionVentas.Contracts;
using ApiGestionVentas.Data;
using ApiGestionVentas.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionVentas.Modules
{
    public class CustomersModule : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/customers", async (GestionVentasDbContext dbContext) =>
            {
                var items = await dbContext.Cliente.ToListAsync();
                if (items == null)
                {
                    return Results.NoContent();
                }
                return Results.Ok(items);

            }).RequireAuthorization();

            endpoints.MapGet("/customers/{id}", async (int id, GestionVentasDbContext dbContext) =>
            {
                var itemModel = await dbContext.Cliente.FindAsync(id);
                if (itemModel == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(itemModel);

            }).RequireAuthorization();

            endpoints.MapPost("/customers", async ([FromBody] ClienteDto item, GestionVentasDbContext dbContext) =>
            {
                var itemModel = new ApiGestionVentas.Models.Cliente { TipoDoc = item.TipoDoc, NumDoc = item.NumDoc, Nombres = item.Nombres, Apellidos = item.Apellidos, Celular = item.Celular };

                var result = dbContext.Cliente.Add(itemModel);

                await dbContext.SaveChangesAsync();

                return Results.Ok(result.Entity);

            }).RequireAuthorization();

            endpoints.MapPut("/customers", async (int id, ClienteDto item, GestionVentasDbContext dbContext) =>
            {
                var itemModel = await dbContext.Cliente.FindAsync(id);
                if (itemModel == null)
                {
                    return Results.NotFound();
                }

                itemModel.Nombres = item.Nombres;
                itemModel.Apellidos = item.Apellidos;
                itemModel.Celular = item.Celular;

                await dbContext.SaveChangesAsync();

                return Results.Ok(itemModel);

            }).RequireAuthorization();

            return endpoints;
        }

        public IServiceCollection RegisterModule(IServiceCollection services)
        {
            return services;
        }
    }
}
