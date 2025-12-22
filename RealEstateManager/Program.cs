using RealEstateManager;
using RealEstateManager.Infrastructure;
using RealEstateManager.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ApiRateLimiter>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var maxRequests = configuration.GetValue<int>("FundaApiCLient:MaxRequestsPerMinute", 90);
    
    return new ApiRateLimiter(permitLimit: maxRequests, windowSeconds: 60);
});

builder.Services.AddHttpClient("FundaApiClient", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration["FundaApiClient:BaseUrl"] ?? "https://funda.nl";
    
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "RealEstateManagerApi/1.0");
});

builder.Services.AddSingleton<IFundaApiClient>(serviceProvider =>
{
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    var rateLimiter = serviceProvider.GetRequiredService<ApiRateLimiter>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    
    return new FundaApiClient(httpClientFactory, rateLimiter, configuration); 
});

builder.Services.AddScoped<IRealEstateUnitService, RealEstateUnitService>();

var app = builder.Build();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();