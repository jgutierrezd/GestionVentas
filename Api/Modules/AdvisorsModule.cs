using ApiGestionVentas.Contracts;
using ApiGestionVentas.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionVentas.Modules
{
    public class AdvisorsModule : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/advisors", async (GestionVentasDbContext dbContext) =>
            {
                var items = await dbContext.Asesor.ToListAsync();
                if (items == null)
                {
                    return Results.NoContent();
                }
                return Results.Ok(items);

            }).RequireAuthorization();

            endpoints.MapGet("/advisors/{id}", async (int id, GestionVentasDbContext dbContext) =>
            {
                var itemModel = await dbContext.Asesor.FindAsync(id);
                if (itemModel == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(itemModel);

            }).RequireAuthorization();

            return endpoints;   
        }

        public IServiceCollection RegisterModule(IServiceCollection services)
        {
            return  services;
        }
    }
}
