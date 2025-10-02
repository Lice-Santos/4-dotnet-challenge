using System.ComponentModel.DataAnnotations;
using System.ComponentModel; // Adicionar para DefaultValue

namespace Tria_2025.DTOs.Moto
{
    /// <summary>
    /// DTO usado para criar e atualizar registros de Motos.
    /// </summary>
    public class MotoDTO
    {
        /// <summary>
        /// Placa da Moto (Formato padrão, 7 ou 8 caracteres).
        /// </summary>
        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(10, MinimumLength = 7, ErrorMessage = "A placa deve ter entre 7 e 10 caracteres.")]
        [DefaultValue("ABC1234")]
        public string Placa { get; set; }

        /// <summary>
        /// Modelo e nome da moto (Ex: Honda Titan 160).
        /// </summary>
        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O modelo deve ter entre 2 e 50 caracteres.")]
        [DefaultValue("Honda CB 500F")]
        public string Modelo { get; set; }

        /// <summary>
        /// Ano de fabricação da moto.
        /// </summary>
        [Required(ErrorMessage = "O ano é obrigatório.")]
        [Range(2000, 2030, ErrorMessage = "O ano deve estar entre 2000 e 2030.")]
        [DefaultValue(2023)]
        public int Ano { get; set; }

        /// <summary>
        /// Tipo de combustível da moto (Ex: Gasolina, Flex, Elétrica).
        /// </summary>
        [Required(ErrorMessage = "O tipo de combustível é obrigatório.")]
        [DefaultValue("Flex")]
        public string TipoCombustivel { get; set; }

        /// <summary>
        /// Chave estrangeira que referencia a Filial onde a moto está alocada.
        /// </summary>
        [Required(ErrorMessage = "O Id da Filial é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id da Filial deve ser um valor positivo.")]
        [DefaultValue(1)]
        public int IdFilial { get; set; }
    }
}