using Microsoft.ML.Data;

namespace ModelPrediction

{

    public class ModelOutput
    {
        [ColumnName("Score")]
        public float FareAmount;
    }
}