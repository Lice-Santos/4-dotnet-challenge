using Tria_2025.Models;
using Tria_2025.Exceptions;
using Tria_2025.DTO;

namespace Tria_2025.Validations
{
    public static class EnderecoValidation
    {
        public static void ValidarECorrigirCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
            {
                // Garante que o nome do campo é claro para a exceção.
                throw new CampoVazioException("CEP");
            }


            // Verifica se tem 8 dígitos E se todos são numéricos
            if (cep.Length != 8 || !cep.All(char.IsDigit) || cep.Contains("-") || cep.Contains("/"))
            {
                // A condição de erro estava incompleta, corrigi os parênteses.
                throw new CepForaFormatacao();
            }

            // Retorna o CEP limpo (os 8 dígitos)
        }
        

        // Método principal que orquestra todas as validações
        public static void ValidarEndereco(Endereco endereco)
        {
            ValidarECorrigirCep(endereco.Cep);
            ValidarLogradouro(endereco.Logradouro);
            ValidarCidade(endereco.Cidade);
            ValidarNumero(endereco.Numero);
            ValidarEstado(endereco.Estado);
            
        }

        public static void ValidarLogradouro(string logradouro)
        {
            if (string.IsNullOrWhiteSpace(logradouro))
            {
                throw new CampoVazioException("Logradouro");
            }

            if (logradouro.Length < 3 || logradouro.Length > 100)
            {
                throw new TamanhoInvalidoDeCaracteresException("Logradouro", 100, 3);
            }
        }

        public static void ValidarCidade(string cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade))
            {
                throw new CampoVazioException("Cidade");
            }

            if (cidade.Length < 2 || cidade.Length > 80)
            {
                throw new TamanhoInvalidoDeCaracteresException("Cidade", 80, 2);
            }
        }

        public static void ValidarNumero(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
            {
                throw new CampoVazioException("Número");
            }

            // O número não pode ser menor que 1 caractere ou maior que 10 (excluindo 's/n')
            if (numero.Length < 1 || numero.Length > 10)
            {
                throw new TamanhoInvalidoDeCaracteresException("Número", 10, 1);
            }
        }

        public static void ValidarEstado(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
            {
                throw new CampoVazioException("Estado");
            }

            // Validação para o código da Unidade Federativa (UF)
            if (estado.Length != 2 || !estado.All(char.IsLetter))
            {
                throw new CampoInvalidoException("Estado");
            }
        }
    }
}