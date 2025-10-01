using System.ComponentModel.DataAnnotations;

namespace Tria_2025.DTO.MotoSetor
{
    // Classe DTO genérica para MotoSetor
    public class MotoSetorDTO
    {
        [Required(ErrorMessage = "A data é obrigatória.")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "A fonte é obrigatória.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A fonte deve ter entre 2 e 100 caracteres.")]
        public string Fonte { get; set; }

        [Required(ErrorMessage = "O Id da Moto é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id da Moto deve ser um valor positivo.")]
        public int IdMoto { get; set; }

        [Required(ErrorMessage = "O Id do Setor é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id do Setor deve ser um valor positivo.")]
        public int IdSetor { get; set; }
    }
}
