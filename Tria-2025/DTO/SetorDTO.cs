using System.ComponentModel.DataAnnotations;

namespace Tria_2025.DTO
{
    public class SetorDTO
    {
        [Required(ErrorMessage = "O nome do setor é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres.")]
        public string Nome { get; set; }
    }
}
