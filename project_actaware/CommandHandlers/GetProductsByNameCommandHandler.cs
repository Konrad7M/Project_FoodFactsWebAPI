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
        private int pageCount;
        public async Task<IEnumerable<Product>> Handle(GetProductsByNameCommand command, CancellationToken cancelationToken)
        {
            var products = GetProducts(command.ProductName, 1, true);
            for(int i = 2; i <= pageCount; i++)
            {
                products.AddRange(GetProducts(command.ProductName, i, false));
            }
            return products;
        }

        private List<Product> GetProducts(string productName, int pageNumber, bool checkPageCount = false)
        {
            var client = new RestClient("https://world.openfoodfacts.org");
            var request = new RestRequest($"https://world.openfoodfacts.org/cgi/search.pl?search_terms={productName}&search_simple=1&json=1&page={pageNumber}", Method.Get);
            request.Timeout = 100000;
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
                    if (checkPageCount)
                    {
                        pageCount = JsonSerializer.Deserialize<int>(root.GetProperty("page_count"));
                    }
                    var productJson = root.GetProperty("products");
                    return JsonSerializer.Deserialize<List<Product>>(productJson, options);
                }
            }
            else
            {
                throw new Exception("connection failed");
            }
        }
    }
}
