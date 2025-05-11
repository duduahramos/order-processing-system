using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OrderAPI.Data;
using OrderAPI.Repositories;
using OrderAPI.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var mySqlServerVersion = new MySqlServerVersion(new Version(8, 4, 5));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(mySqlConnection, mySqlServerVersion);
});


var serviceName = "to-segurado-back";
var serviceVersion = typeof(Program).Assembly.GetName().Version!.ToString();

builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing =>
        {
            tracing
                // The rest of your setup code goes here
                .AddSource(serviceName)
                .ConfigureResource(resourceBuilder =>
                {
                    resourceBuilder.AddService(serviceName: serviceName, serviceVersion: serviceVersion);
                })
                .AddHttpClientInstrumentation(instrumentationOptions =>
                {
                    instrumentationOptions.RecordException = true;
                })
                .AddAspNetCoreInstrumentation(instrumentationOptions =>
                {
                    instrumentationOptions.RecordException = true;
                })
                .AddSqlClientInstrumentation(instrumentationOptions =>
                {
                    instrumentationOptions.RecordException = true;
                    instrumentationOptions.SetDbStatementForText = true;
                })
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri("http://localhost:4317");
                    options.Protocol = OtlpExportProtocol.Grpc;
                })
                .AddConsoleExporter();
        }
    )
    .WithMetrics(options =>
    {
        options
            .ConfigureResource(resourceBuilder =>
            {
                resourceBuilder.AddService(serviceName: serviceName, serviceVersion: serviceVersion);
            })
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            //.AddEventCountersInstrumentation()
            .AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel", "System.Net.Http")
            //.AddCustomMeter(builder.Environment.ApplicationName)
            //.AddPrometheusExporter() // exporting metrics to prometheus via prometheus-net exporter.
            .AddOtlpExporter(opt =>
            {
                opt.Protocol = OtlpExportProtocol.Grpc;
                opt.Endpoint = new Uri("http://localhost:4317");
            })
            .AddConsoleExporter();
    });

builder.Logging.AddOpenTelemetry(logging => {
    // The rest of your setup code goes here
    logging.AddOtlpExporter(options =>
    {
        options.Endpoint = new Uri("http://localhost:4317");
        options.Protocol = OtlpExportProtocol.Grpc;
    })
    .AddConsoleExporter();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
