using Microsoft.ML.Data;

public class ModelOutput
{
    // A previsão (true para Positivo, false para Negativo)
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }

    // A pontuação "raw" da previsão
    public float Score { get; set; }
}