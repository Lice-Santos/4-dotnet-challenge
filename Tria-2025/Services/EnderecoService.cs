using Tria_2025.Exceptions;
using Tria_2025.Models;
using Tria_2025.Repository;
using Tria_2025.Validations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tria_2025.DTO;

namespace Tria_2025.Services
{
    public class EnderecoService
    {
        private readonly IEnderecoRepository _repository;

        public EnderecoService(IEnderecoRepository repository)
        {
            _repository = repository;
        }

        // --- MÉTODOS DE ESCRITA ---

        public async Task<Endereco> CreateEnderecoAsync(Endereco endereco)
        {

            EnderecoValidation.ValidarEndereco(endereco);


            return await _repository.AddAsync(endereco);
        }


        public async Task UpdateEnderecoAsync(int id, Endereco endereco)
        {
            var enderecoExistente = await _repository.GetByIdAsync(id);
            if (enderecoExistente == null)
            {
                // Lança 404
                throw new ObjetoNaoEncontradoException(nameof(Endereco), id);
            }


            enderecoExistente.Logradouro = endereco.Logradouro ?? enderecoExistente.Logradouro;
            enderecoExistente.Cidade = endereco.Cidade ?? enderecoExistente.Cidade;
            enderecoExistente.Estado = endereco.Estado ?? enderecoExistente.Estado;
            enderecoExistente.Numero = endereco.Numero ?? enderecoExistente.Numero;
            enderecoExistente.Complemento = endereco.Complemento ?? enderecoExistente.Complemento;
            enderecoExistente.Cep = endereco.Cep ?? enderecoExistente.Cep;

            if (enderecoExistente == null)
                throw new ObjetoNaoEncontradoException(nameof(Endereco), id);

            var enderecoParaValidar = enderecoExistente;
            EnderecoValidation.ValidarEndereco(enderecoParaValidar);


            await _repository.UpdateAsync(enderecoExistente);
        }

        public async Task DeleteEnderecoAsync(int id)
        {
            var endereco = await _repository.GetByIdAsync(id);
            if (endereco == null)
            {
                throw new ObjetoNaoEncontradoException(nameof(Endereco), id);
            }
            await _repository.DeleteAsync(endereco);
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<Endereco> GetEnderecoByIdAsync(int id)
        {
            var endereco = await _repository.GetByIdAsync(id);
            if (endereco == null)
            {
                throw new ObjetoNaoEncontradoException(nameof(Endereco), id);
            }
            return endereco;
        }

        public async Task<Endereco> GetEnderecoByCepAsync(string cep)
        {
            EnderecoValidation.ValidarECorrigirCep(cep);

            var endereco = await _repository.GetByCepAsync(cep);

            if (endereco == null)
            {
                throw new ObjetoNaoEncontradoException($"Endereço com CEP {cep}", "Busca");
            }
            return endereco;
        }

        public async Task<IEnumerable<Endereco>> GetAllEnderecosAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
