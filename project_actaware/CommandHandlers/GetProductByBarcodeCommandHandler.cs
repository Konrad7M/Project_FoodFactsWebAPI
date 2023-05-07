using MediatR;
using Polly;
using project_actaware.Commands;
using project_actaware.Execptions;
using project_actaware.Models;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace project_actaware.CommandHandlers;

public class GetProductByBarcodeCommandHandler: IRequestHandler<GetProductByBarcodeCommand,ProductDTO>
{
    public async Task<ProductDTO> Handle(GetProductByBarcodeCommand command,CancellationToken cancelationToken)
    {
        var barcode = command.Barcode;
        var client = new RestClient("https://world.openfoodfacts.org");
        var request = new RestRequest($"https://world.openfoodfacts.org/api/v0/product/{barcode}.json", Method.Get);
        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i * 5));
        var response = await retryPolicy.ExecuteAsync(async () =>
        {
            return await client.GetAsync(request, cancelationToken);
        });
        if (!response.IsSuccessful)
        {
            throw new Exception("response failed");
        }
        if (response.Content == null)
        {
            throw new Exception("response content null");
        }
        using (JsonDocument document = JsonDocument.Parse(response.Content))
        {
            var options = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString |
                   JsonNumberHandling.WriteAsString
            };
            JsonElement root = document.RootElement;
            var statusCode = root.GetProperty("status");
            if(JsonSerializer.Deserialize<int>(statusCode) == 0)
            {
                throw new BusinessException("product not found");
            }
            var productJson = root.GetProperty("product");
            var product = JsonSerializer.Deserialize<ProductDTO>(productJson, options);
            return product ?? throw new Exception("desarialization failed");
        }
    }
}


