using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

namespace Tria_2025.Models
{
    public class Filial
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        //chave estrangeira mapeada no mapping
        public int IdEndereco { get; set; }
        [JsonIgnore]
        public virtual Endereco Endereco { get; set; }

    }
}
