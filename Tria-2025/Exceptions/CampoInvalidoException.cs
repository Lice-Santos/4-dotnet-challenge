namespace Tria_2025.Exceptions
{
    public class CampoInvalidoException : Exception
    {
        public CampoInvalidoException(string campo)
            : base($"O campo '{campo}' possui um valor inválido.")
        {
        }
    }
}