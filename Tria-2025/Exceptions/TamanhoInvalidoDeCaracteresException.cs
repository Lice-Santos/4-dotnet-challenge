namespace Tria_2025.Exceptions
{
    public class TamanhoInvalidoDeCaracteresException : Exception
    {
        public TamanhoInvalidoDeCaracteresException(string nomeCampo, int tamanhoMaximo, int tamanhoMinimo) : base($"{nomeCampo} deve ter no mínimo {tamanhoMinimo} e no máximo {tamanhoMaximo} caracteres.") { }
    }
}
