using MediatR;
using Polly;
using project_actaware.Commands;
using project_actaware.Models;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace project_actaware.CommandHandlers
{
    public class GetProductsByNameCommandHandler : IRequestHandler<GetProductsByNameCommand, IEnumerable<ProductDTO>>
    {
        private int pageCount;
        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByNameCommand command, CancellationToken cancelationToken)
        {
            var products = await GetProducts(command.ProductName, 1, cancelationToken, true);
            for(int i = 2; i <= pageCount; i++)
            {
                products.AddRange(await GetProducts(command.ProductName, i, cancelationToken, false));
            }
            return products;
        }

        private async Task<List<ProductDTO>> GetProducts(string productName, int pageNumber, CancellationToken cancelationToken, bool checkPageCount = false)
        {
            var client = new RestClient();
            var request = new RestRequest($"https://world.openfoodfacts.org/cgi/search.pl?search_terms={productName}&search_simple=1&json=1&page={pageNumber}", Method.Get);
            var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i * 5));
            var response = await retryPolicy.ExecuteAsync(async () =>
            {
                return await client.GetAsync(request, cancelationToken);
            });
            
            if (!response.IsSuccessful)
            {
                throw new Exception("connection failed");
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
                if (checkPageCount)
                {
                    pageCount = JsonSerializer.Deserialize<int>(root.GetProperty("page_count"));
                }
                var productJson = root.GetProperty("products");
                return JsonSerializer.Deserialize<List<ProductDTO>>(productJson, options) ?? throw new Exception("deserialization failed");
            }
        }
    }
}
