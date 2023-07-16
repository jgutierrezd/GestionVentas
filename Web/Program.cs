var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient("quotes.api", http =>
{
    // Get API key from appropriate place, e.g. user secrets in dev, environment variable in prod, etc.
    //var apiKey = builder.Configuration["QuotesApi:ApiKey"];
    //http.DefaultRequestHeaders.Add("X-API-Key", apiKey);
    // Add any other required headers
    // Set base address
    http.BaseAddress = new Uri(builder.Configuration["Settings:UrlApiBack"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
