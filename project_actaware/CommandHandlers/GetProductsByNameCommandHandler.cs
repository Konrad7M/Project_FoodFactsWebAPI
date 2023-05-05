using MediatR;
using project_actaware.Commands;
using project_actaware.Execptions;
using project_actaware.Models;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace project_actaware.CommandHandlers
{
    public class GetProductsByNameCommandHandler : IRequestHandler<GetProductsByNameCommand, IEnumerable<Product>>
    {
        public async Task<IEnumerable<Product>> Handle(GetProductsByNameCommand command, CancellationToken cancelationToken)
        {
            var productName = command.ProductName;
            var client = new RestClient("https://world.openfoodfacts.org");
            var request = new RestRequest($"https://world.openfoodfacts.org/cgi/search.pl?search_terms={productName}&search_simple=1&json=1&page=1", Method.Get);
            request.Timeout = 100000;
            List<Product> products = new List<Product>();
            int pageCount;
            var response = client.Get(request);
            if (response.IsSuccessful)
            {
                using (JsonDocument document = JsonDocument.Parse(response.Content))
                {
                    var options = new JsonSerializerOptions()
                    {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString |
                        JsonNumberHandling.WriteAsString
                    };
                    JsonElement root = document.RootElement;

                    pageCount = JsonSerializer.Deserialize<int>(root.GetProperty("page_count"));
                    var productJson = root.GetProperty("products");
                    products.AddRange(JsonSerializer.Deserialize<IEnumerable<Product>>(productJson, options));
                }
            }
            else
            {
                throw new Exception("response failed");
            }

            for(int i = 2; i <= pageCount; i++)
            {
                var pageRequest = new RestRequest($"https://world.openfoodfacts.org/cgi/search.pl?search_terms={productName}&search_simple=1&json=1&page={i}", Method.Get);
                var pageResponse = client.Get(pageRequest);
                if (pageResponse.IsSuccessful)
                {

                    using (JsonDocument document = JsonDocument.Parse(response.Content))
                    {
                        var options = new JsonSerializerOptions()
                        {
                            NumberHandling = JsonNumberHandling.AllowReadingFromString |
                            JsonNumberHandling.WriteAsString
                        };
                        JsonElement root = document.RootElement;

                        var productJson = root.GetProperty("products");
                        products.AddRange(JsonSerializer.Deserialize<IEnumerable<Product>>(productJson, options));
                    }
                }
                else
                {
                    throw new Exception("response failed");
                }
            }
            return products;
        }
    }
}
