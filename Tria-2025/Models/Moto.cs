using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tria_2025.Models
{
    public class Moto
    {
        [Key]
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public string TipoCombustivel { get; set; }
        public int IdFilial { get; set; }
        //chave estrangeira mapeada no mapping
        [JsonIgnore]
        public virtual Filial Filial { get; set; }

    }
}
