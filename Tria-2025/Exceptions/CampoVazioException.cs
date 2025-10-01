using static System.Net.Mime.MediaTypeNames;
namespace Tria_2025.Exceptions
{
    public class CampoVazioException : Exception
    {
        public CampoVazioException(string nome) : base($"{nome} não pode estar vazio.")
        { 
            
        }
    }
}
