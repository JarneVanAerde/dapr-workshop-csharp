// create web-app
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IFineCalculator, HardCodedFineCalculator>();

builder.Services.AddSingleton<VehicleRegistrationService>(_ => 
    new VehicleRegistrationService(DaprClient.CreateInvokeHttpClient(
        "vehicleregistrationservice", "http://localhost:3601")));

builder.Services.AddDaprClient(builder => builder
    .UseHttpEndpoint($"http://localhost:3601")
    .UseGrpcEndpoint($"http://localhost:60001"));

builder.Services.AddControllers().AddDapr();

var app = builder.Build();

// configure web-app
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// configure routing
app.MapControllers();
app.UseCloudEvents();
app.MapSubscribeHandler();

// let's go!
app.Run("http://localhost:6001");
