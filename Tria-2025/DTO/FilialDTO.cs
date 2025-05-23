using System.ComponentModel.DataAnnotations;

namespace Tria_2025.DTO
{
    public class FilialDTO
    {
        //Classe DTO da entidade Filial para manipulação
        [Required]
        public string Nome { get; set; }

        [Required]
        public int IdEndereco { get; set; }
    }

}
