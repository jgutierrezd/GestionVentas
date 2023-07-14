using ApiGestionVentas.Data;
using ApiGestionVentas.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services DB to container.
builder.Services.AddDbContext<GestionVentasDbContext>();

// Add services Authentication(JWT) to container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

// Add services Authorization to container.
builder.Services.AddAuthorization();

// Add services Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Map access token
app.MapPost("/token", async (User user) =>
{
    if (user.UserName == "Admin" && user.Password == "123456")
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
        (builder.Configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);

        return Results.Ok(stringToken);
    }
    return Results.Unauthorized();
});


app.MapGet("/", () => "");


// Productos
/////////////////////////////////////////////////////////////
app.MapGet("/products", async (GestionVentasDbContext dbContext) =>
{
    var items = await dbContext.Producto.ToListAsync();
    if (items == null)
    {
        return Results.NoContent();
    }
    return Results.Ok(items);

}).RequireAuthorization();

app.MapGet("/products/{id}", async (int id, GestionVentasDbContext dbContext) =>
{
    var itemModel = await dbContext.Producto.FindAsync(id);
    if (itemModel == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(itemModel);
}).RequireAuthorization();


//Asesores
/////////////////////////////////////////////////////////////
app.MapGet("/advisors", async (GestionVentasDbContext dbContext) =>
{
    var items = await dbContext.Asesor.ToListAsync();
    if (items == null)
    {
        return Results.NoContent();
    }
    return Results.Ok(items);

}).RequireAuthorization();

app.MapGet("/advisors/{id}", async (int id, GestionVentasDbContext dbContext) =>
{
    var itemModel = await dbContext.Asesor.FindAsync(id);
    if (itemModel == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(itemModel);

}).RequireAuthorization();

//Clientes
/////////////////////////////////////////////////////////////
app.MapGet("/customers", async (GestionVentasDbContext dbContext) =>
{
    var items = await dbContext.Cliente.ToListAsync();
    if (items == null)
    {
        return Results.NoContent();
    }
    return Results.Ok(items);

}).RequireAuthorization();

app.MapGet("/customers/{id}", async (int id, GestionVentasDbContext dbContext) =>
{
    var itemModel = await dbContext.Cliente.FindAsync(id);
    if (itemModel == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(itemModel);

}).RequireAuthorization();

app.MapPost("/customers", async ([FromBody] Cliente item, GestionVentasDbContext dbContext) =>
{
    var itemModel = new ApiGestionVentas.Models.Cliente { TipoDoc = item.TipoDoc, NumDoc = item.NumDoc, Nombres = item.Nombres, Apellidos = item.Apellidos, Celular = item.Celular };

    var result = dbContext.Cliente.Add(itemModel);

    await dbContext.SaveChangesAsync();

    return Results.Ok(result.Entity);

}).RequireAuthorization();

app.MapPut("/customers", async (int id, Cliente item, GestionVentasDbContext dbContext) =>
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


//Ventas
/////////////////////////////////////////////////////////////
app.MapGet("/sales", async (GestionVentasDbContext dbContext) =>
{
    var items = await dbContext.Venta.ToListAsync();
    if (items == null)
    {
        return Results.NoContent();
    }
    return Results.Ok(items);

}).RequireAuthorization();

app.MapGet("/sales/{id}", async (int id, GestionVentasDbContext dbContext) =>
{
    var itemModel = await dbContext.Venta.FindAsync(id);
    if (itemModel == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(itemModel);

}).RequireAuthorization();

app.MapPost("/sales", async ([FromBody] Venta item, GestionVentasDbContext dbContext) =>
{
    var itemModel = new ApiGestionVentas.Models.Venta { Periodo = item.Periodo, Monto = item.Monto, FechaOperacion = DateTime.Now };

    var result = dbContext.Venta.Add(itemModel);

    await dbContext.SaveChangesAsync();

    return Results.Ok(result.Entity);

}).RequireAuthorization();

/////////////////////////////////////////////////////////////
app.Run();