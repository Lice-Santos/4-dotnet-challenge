using System.ComponentModel.DataAnnotations;
using System.ComponentModel; 

namespace Tria_2025.DTO.MotoSetor
{
    /// <summary>
    /// DTO usado para criar e atualizar a alocação de uma Moto a um Setor.
    /// </summary>
    public class MotoSetorDTO
    {
        /// <summary>
        /// Data em que a alocação ocorreu.
        /// </summary>
        [Required(ErrorMessage = "A data é obrigatória.")]
        [DefaultValue("2025-10-01T10:00:00")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Fonte ou responsável pela mudança de alocação (Ex: Sistema, Gerente XPTO).
        /// </summary>
        [Required(ErrorMessage = "A fonte é obrigatória.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A fonte deve ter entre 2 e 100 caracteres.")]
        [DefaultValue("Ordem de Serviço 456")]
        public string Fonte { get; set; }

        /// <summary>
        /// ID da Moto que está sendo alocada. Deve ser um ID existente na tabela Moto.
        /// </summary>
        [Required(ErrorMessage = "O Id da Moto é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id da Moto deve ser um valor positivo.")]
        [DefaultValue(101)]
        public int IdMoto { get; set; }

        /// <summary>
        /// ID do Setor de destino da alocação. Deve ser um ID existente na tabela Setor.
        /// </summary>
        [Required(ErrorMessage = "O Id do Setor é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id do Setor deve ser um valor positivo.")]
        [DefaultValue(2)]
        public int IdSetor { get; set; }
    }
}