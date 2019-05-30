using System;
using Microsoft.ML.Data;

namespace Djkormo.Web
{
public class ModelInput
        {
        [ColumnName("SepalLength"), LoadColumn(0)]
        public float SepalLength { get; set; }

        [ColumnName("SepalWidth"), LoadColumn(1)]
        public float SepalWidth { get; set; }

        [ColumnName("PetalLength"), LoadColumn(2)]
        public float PetalLength { get; set; }

        [ColumnName("PetalWidth"), LoadColumn(3)]
        public float PetalWidth { get; set; }

        [ColumnName("Label"), LoadColumn(4)]
        public string Label { get; set; }

        }
}
