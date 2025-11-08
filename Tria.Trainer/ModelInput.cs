using Microsoft.ML.Data;

public class ModelInput
{
    // Coluna 0 do nosso conjunto de dados
    [LoadColumn(0)]
    public string Text { get; set; }

    // Coluna 1 do nosso conjunto de dados (o "Rótulo" ou "Label")
    // true = Positivo, false = Negativo
    [LoadColumn(1), ColumnName("Label")]
    public bool Sentiment { get; set; }
}