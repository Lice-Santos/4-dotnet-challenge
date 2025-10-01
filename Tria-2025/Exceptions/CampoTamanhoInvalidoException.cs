namespace Tria_2025.Exceptions
{
    public class CampoTamanhoInvalidoException : Exception
    {
        public CampoTamanhoInvalidoException(string campo, int quantidadeCaracteres) : base($"{campo} deve ter exatamente {quantidadeCaracteres} caracteres.") { }
    }
}
