using System.ComponentModel.DataAnnotations;
using System.ComponentModel; // Adicionar para DefaultValue

namespace Tria_2025.DTO
{
    /// <summary>
    /// DTO usado para criar e atualizar registros de Setores.
    /// </summary>
    public class SetorDTO
    {
        /// <summary>
        /// Nome do Setor (deve ser único e ter entre 3 e 50 caracteres).
        /// </summary>
        [Required(ErrorMessage = "O nome do setor é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres.")]
        [DefaultValue("Setor de Manutenção")]
        public string Nome { get; set; }
    }
}