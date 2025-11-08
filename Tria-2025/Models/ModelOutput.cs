using Microsoft.ML.Data;
namespace SmartApi.Web.Models
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
        public float Score { get; set; }
    }
}