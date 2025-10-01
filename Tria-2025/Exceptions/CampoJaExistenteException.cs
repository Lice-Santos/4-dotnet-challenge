namespace Tria_2025.Exceptions
{
    public class CampoJaExistenteException : Exception
    {
        public CampoJaExistenteException(string campo, string valor)
            : base($"O valor '{valor}' para o campo '{campo}' já está cadastrado.")
        {
        }
    }
}