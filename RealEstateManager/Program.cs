using RealEstateManager.Infrastructure;
using RealEstateManager.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("FundaApiClient", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration["FundaApiClient:BaseUrl"] ?? "https://funda.nl";
    
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
    client.DefaultRequestHeaders.Add("User-Agent", "RealEstateManagerApi/1.0");
});

builder.Services.AddSingleton<IFundaApiClient, FundaApiClient>();

builder.Services.AddScoped<IRentalPropertyService, RentalPropertyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();