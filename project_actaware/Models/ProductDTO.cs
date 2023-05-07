using System.Text.Json.Serialization;

namespace project_actaware.Models
{
    public class ProductDTO
    {
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
        [JsonPropertyName("serving_size")]
        public string ServingSize {get ; set;}
        [JsonPropertyName("code")]
        public string Barcode { get; set; }
        [JsonPropertyName("nutriments")]
        public NutrimentsDTO Nutriments { get; set; }

    }
}
