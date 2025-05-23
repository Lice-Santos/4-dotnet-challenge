using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tria_2025.Models
{
    public class Setor
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }



    }
}
