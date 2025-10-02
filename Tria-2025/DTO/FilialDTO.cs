using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Tria_2025.DTO
{
    /// <summary>
    /// DTO usado para criação e atualização de Filiais.
    /// </summary>
    public class FilialDTO
    {

        /// <summary>
        /// Nome da Filial (Ex: Filial Sul, Filial Nordeste).
        /// </summary>
        [Required(ErrorMessage = "O Nome da Filial é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
        [DefaultValue("Filial Centro-Oeste")]
        public string Nome { get; set; }

        /// <summary>
        /// Chave estrangeira que referencia o Endereço da Filial.
        /// </summary>
        [Required(ErrorMessage = "O ID do Endereço é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do Endereço deve ser um valor válido.")]
        [DefaultValue(5)]
        public int IdEndereco { get; set; }
    }
}