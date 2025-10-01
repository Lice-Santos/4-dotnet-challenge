using System.ComponentModel.DataAnnotations;

namespace Tria_2025.DTO
{
    public class FilialDTO
    {
        // Id informado pelo usuário
        [Required]
        public int Id { get; set; }

        // Nome da filial
        [Required]
        public string Nome { get; set; }

        // Id do endereço relacionado
        [Required]
        public int IdEndereco { get; set; }
    }
}
