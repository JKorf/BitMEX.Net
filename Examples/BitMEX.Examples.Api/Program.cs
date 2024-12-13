using BitMEX.Net.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the BitMEX services
builder.Services.AddBitMEX();

// OR to provide API credentials for accessing private endpoints, or setting other options:
/*
builder.Services.AddBitMEX(options =>
{
    options.ApiCredentials = new ApiCredentials("<APIKEY>", "<APISECRET>");
    options.Rest.RequestTimeout = TimeSpan.FromSeconds(5);
});
*/

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map the endpoint and inject the rest client
app.MapGet("/{Symbol}", async ([FromServices] IBitMEXRestClient client, string symbol) =>
{
    var result = await client.ExchangeApi.ExchangeData.GetSymbolsAsync(symbol);
    return result.Data.Single().LastPrice;
})
.WithOpenApi();


app.MapGet("/Balances", async ([FromServices] IBitMEXRestClient client) =>
{
    var result = await client.ExchangeApi.Account.GetBalancesAsync();
    return (object)(result.Success ? result.Data : result.Error!);
})
.WithOpenApi();

app.Run();