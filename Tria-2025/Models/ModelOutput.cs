using Microsoft.ML.Data;
namespace Tria_2025.Models
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
        public float Score { get; set; }
    }
}