using Domain_layer.Models;

public class Sentiment
{
    public int SentimentId { get; set; }

   
    public string MessageId { get; set; }
    public Message Message { get; set; }

    public string Score { get; set; }
    public string Label { get; set; }
}
