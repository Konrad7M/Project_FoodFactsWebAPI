using Polly;
using project_actaware.Execptions;
using project_actaware.Models;
using RestSharp;
using System.Text.Json.Serialization;
using System.Text.Json;
using project_actaware.Commands;

namespace project_actaware.Services
{
    public class ProductService: IProductService
    {
        private int pageCount;
        public async Task<ProductDTO> GetByBarcode(string barcode, CancellationToken cancelationToken)
        {
            var client = new RestClient();
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
                throw new BusinessException(ErrorTypeEnum.ResponseContentNull,"response content null");
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
                if (JsonSerializer.Deserialize<int>(statusCode) == 0)
                {
                    throw new BusinessException(ErrorTypeEnum.ProductNotFound,"product not found");
                }
                var productJson = root.GetProperty("product");
                var product = JsonSerializer.Deserialize<ProductDTO>(productJson, options);
                return product ?? throw new Exception("desarialization failed");
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetByName(string productName, CancellationToken cancelationToken)
        {
            var products = await GetProducts(productName, 1, cancelationToken, true);
            for (int i = 2; i <= pageCount; i++)
            {
                products.AddRange(await GetProducts(productName, i, cancelationToken, false));
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
                throw new BusinessException(ErrorTypeEnum.ResponseContentNull,"response content null");
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
