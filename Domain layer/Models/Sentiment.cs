using Domain_layer.Models;

public class Sentiment
{
    public int SentimentId { get; set; }
    public string SourceText { get; set; } = string.Empty;
    public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;

    // Link to message (required)
    public int MessageId { get; set; }
    public Message Message { get; set; }

    // Results
    public double Score { get; set; }     // numeric score if you have one
    public string Label { get; set; } = string.Empty; // e.g. Positive/Neutral/Negative
}
