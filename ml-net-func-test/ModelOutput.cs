using Microsoft.ML.Data;

namespace Djkormo.Function
{
    public class ModelOutput : ModelInput
{

    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }

    public float Probability { get; set; }

    public float Score { get; set; }
}
}