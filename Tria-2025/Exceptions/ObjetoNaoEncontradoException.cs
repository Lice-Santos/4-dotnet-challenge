namespace Tria_2025.Exceptions
{
    public class ObjetoNaoEncontradoException : Exception
    {
        public ObjetoNaoEncontradoException(string entidade, string operacao)
            : base($"{entidade} não encontrado para a operação de {operacao}.")
        {
        }

        // Sobrecarga útil para buscar por ID
        public ObjetoNaoEncontradoException(string entidade, int id)
            : base($"{entidade} com Id {id} não encontrado.")
        {
        }
    }
}