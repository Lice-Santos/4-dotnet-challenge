using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tria_2025.Models
{
    public class MotoSetor
    {
        [Key]
        public int Id { get; set; }
        public DateTime Data {  get; set; }
        public string Fonte { get; set; }

        public int IdMoto { get; set; }
        public int IdSetor { get; set; }
        //chaves estrangeiras mapeadas no mapping
        [JsonIgnore]
        public Moto Moto { get; set; }

        [JsonIgnore]
        public Setor Setor { get; set; }


    }
}
