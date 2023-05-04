using System.Text.Json.Serialization;

namespace project_actaware.Models
{
    public class Nutriments
    {
        [JsonPropertyName("carbohydrates_100g")]
        public double CarbohydratesPer100g { get; set; }

        [JsonPropertyName("energy-kcal_100g")]
        public double EnergyPer100g { get; set; }

        [JsonPropertyName("fat_100g")]
        public double FatPer100g { get; set; }

        [JsonPropertyName("proteins_100g")]
        public double ProteinsPer100g { get; set; }

        [JsonPropertyName("salt_100g")]
        public double SaltPer100g { get; set; }
    }
}
