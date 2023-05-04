using MediatR;
using Microsoft.OpenApi.Models;
using project_actaware.Commands;
using project_actaware.Models;
using RestSharp;
using System.Text.Json;

namespace project_actaware.CommandHandlers;

public class GetProductByBarcodeCommandHandler: IRequestHandler<GetProductByBarcodeCommand,Product>
{
    public async Task<Product> Handle(GetProductByBarcodeCommand command,CancellationToken cancelationToken)
    {
        var barcode = command.Barcode;
        var client = new RestClient("https://world.openfoodfacts.org");
        var request = new RestRequest($"https://world.openfoodfacts.org/api/v0/product/{barcode}.json", Method.Get);

        var response = client.Get(request);
        if (response.IsSuccessful)
        {
            using (JsonDocument document = JsonDocument.Parse(response.Content))
            {
                JsonElement root = document.RootElement;
                var statusCode = root.GetProperty("status");
                if(JsonSerializer.Deserialize<int>(statusCode) == 0)
                {
                    throw new Exception("busines expestion");
                }
                var productJson = root.GetProperty("product");
                var product = JsonSerializer.Deserialize<Product>(productJson);
                return product;
            }
        }
        else
        {
            throw new Exception("response error");
        }
    }
}


