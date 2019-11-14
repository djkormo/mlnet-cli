using Microsoft.ML.Data;

namespace SentimentAnalysis
{
    public class ModelInput
    {
[LoadColumn(0)]
    public string SentimentText;

    [LoadColumn(1), ColumnName("Label")]
    public bool Sentiment;
    }
}