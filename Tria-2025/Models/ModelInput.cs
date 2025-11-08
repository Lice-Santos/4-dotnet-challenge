namespace Tria_2025.Models
{
    public class ModelInput
    {
        public string Text { get; set; }

        // O sentimento (Label) não é necessário aqui, 
        // pois só queremos prever, não treinar.
        // Mas podemos mantê-lo para consistência, 
        // o motor de previsão irá ignorá-lo se não for usado.
    }
}