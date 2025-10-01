using System.ComponentModel.DataAnnotations;

namespace Tria_2025.DTOs.Moto
{
    public class MotoDTO
    {
        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(10, MinimumLength = 7, ErrorMessage = "A placa deve ter entre 7 e 10 caracteres.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O modelo deve ter entre 2 e 50 caracteres.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O ano é obrigatório.")]
        [Range(2000, 2030, ErrorMessage = "O ano deve estar entre 2000 e 2030.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "O tipo de combustível é obrigatório.")]
        public string TipoCombustivel { get; set; }

        [Required(ErrorMessage = "O Id da Filial é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id da Filial deve ser um valor positivo.")]
        public int IdFilial { get; set; }
    }
}