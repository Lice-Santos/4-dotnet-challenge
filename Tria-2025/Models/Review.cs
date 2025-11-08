namespace Tria_2025.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string? PredictedSentiment { get; set; } // "Positivo" ou "Negativo"
        public float? SentimentScore { get; set; }
    }
}
