using ApiGestionVentas.Contracts;
using ApiGestionVentas.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionVentas.Modules
{
    public class ProductsModule : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/products", async (GestionVentasDbContext dbContext) =>
            {
                var items = await dbContext.Producto.ToListAsync();
                if (items == null)
                {
                    return Results.NoContent();
                }
                return Results.Ok(items);

            }).RequireAuthorization();

            endpoints.MapGet("/products/{id}", async (int id, GestionVentasDbContext dbContext) =>
            {
                var itemModel = await dbContext.Producto.FindAsync(id);
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
            //services.AddSingleton(new OrderConfig());
            //services.AddScoped<IOrdersRepository, OrdersRepository>();
            //services.AddScoped<IPayment, PaymentService>();
            return services;
        }
    }
}
