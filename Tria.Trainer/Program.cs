using Microsoft.ML;

Console.WriteLine("Iniciando Treinamento do Modelo de Sentimento...");

// Dados de treino
var sampleData = new List<ModelInput>
{
    new ModelInput { Text = "The motorcycle is in great condition, engine running smoothly.", Sentiment = true },
    new ModelInput { Text = "Brakes and tires are in good shape.", Sentiment = true },
    new ModelInput { Text = "Everything has been serviced and there are no engine noises.", Sentiment = true },
    new ModelInput { Text = "New battery, starts without any issues.", Sentiment = true },

    new ModelInput { Text = "Engine is misfiring and releasing smoke.", Sentiment = false },
    new ModelInput { Text = "Tires are worn out and the brakes make noise.", Sentiment = false },
    new ModelInput { Text = "Battery keeps draining and the lights are weak.", Sentiment = false },
    new ModelInput { Text = "Loud noise from the chain and oil leaking.", Sentiment = false }
};


var mlContext = new MLContext();

var dataView = mlContext.Data.LoadFromEnumerable(sampleData);

// 4. Definir o Pipeline de Treino
var pipeline = mlContext.Transforms.Text
    .FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(ModelInput.Text))

    .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
        labelColumnName: "Label",
        featureColumnName: "Features"));

Console.WriteLine("Treinando o modelo...");
var model = pipeline.Fit(dataView);
Console.WriteLine("Modelo treinado!");

// Tria.Trainer/bin/Debug/netX.X/model.zip
mlContext.Model.Save(model, dataView.Schema, "model.zip");

Console.WriteLine("Modelo salvo como 'model.zip'.");
Console.WriteLine("Copie este 'model.zip' para o projeto 'TRIA'");

// Atenção
// Após executar, vá à pasta (ex: bin/Debug/net8.0) do projeto Trainer,
// copie o 'model.zip' e cole-o na raiz do projeto 'TRIA.Web'.