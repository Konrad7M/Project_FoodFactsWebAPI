using MediatR;
using Microsoft.OpenApi.Models;
using project_actaware.Commands;
using project_actaware.Models;

namespace project_actaware.CommandHandlers;

public class GetProductByBarcodeCommandHandler: IRequestHandler<GetProductByBarcodeCommand,Product>
{
    public async Task<Product> Handle(GetProductByBarcodeCommand command,CancellationToken cancelationToken)
    {
        //var client = new RestClient("https://world.openfoodfacts.org");
        //var request = new RestRequest($"api/v0/product/{barcode}.json?fields=generic_name,quantity,image_url", DataFormat.Json);
        //request.AddParameter("user_id", _apiKey, ParameterType.QueryString);

        //var response = client.Get(request);
        //if (response.IsSuccessful)
        //{
        //    var content = response.Content;
        //    // parsowanie JSONa i tworzenie obiektu Product
        //    return Ok(product);
        //}
        //else
        //{
        //    return NotFound();
        //}
        return new Product();
    }
}


