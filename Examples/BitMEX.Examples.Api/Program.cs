using BitMEX.Net;
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
    options.ApiCredentials = new BitMEXCredentials("<APIKEY>", "<APISECRET>");
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
    return result.Success
        ? Results.Ok(result.Data.Single().LastPrice)
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();


app.MapGet("/Balances", async ([FromServices] IBitMEXRestClient client) =>
{
    var result = await client.ExchangeApi.Account.GetBalancesAsync();
    return result.Success
        ? Results.Ok(result.Data)
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();

app.Run();
