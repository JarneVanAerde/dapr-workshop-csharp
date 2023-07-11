// create web-app
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient(builder => builder
    .UseHttpEndpoint($"http://localhost:3600")
    .UseGrpcEndpoint($"http://localhost:60000"));

builder.Services.AddSingleton<ISpeedingViolationCalculator>(
    new DefaultSpeedingViolationCalculator("A12", 10, 100, 5));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IVehicleStateRepository, InMemoryVehicleStateRepository>();
builder.Services.AddControllers();

var app = builder.Build();

// configure web-app
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// configure routing
app.MapControllers();

// let's go!
app.Run("http://localhost:6000");
