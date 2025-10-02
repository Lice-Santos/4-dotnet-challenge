using System.ComponentModel.DataAnnotations;
using System.ComponentModel; // Adicionar para DefaultValue

namespace Tria_2025.DTO.Funcionario
{
    /// <summary>
    /// DTO usado para criar e atualizar registros de Funcionários.
    /// </summary>
    public class FuncionarioDTO
    {
        /// <summary>
        /// Nome completo do funcionário.
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        [DefaultValue("Pedro Álvares")]
        public string Nome { get; set; }

        /// <summary>
        /// Cargo do funcionário (Ex: Mecânico, Gerente, Vendedor).
        /// </summary>
        [Required(ErrorMessage = "O cargo é obrigatório.")]
        [DefaultValue("Mecânico")]
        public string Cargo { get; set; }

        /// <summary>
        /// Endereço de e-mail (deve ser único no sistema).
        /// </summary>
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres.")]
        [DefaultValue("pedro.alvares@tria.com")]
        public string Email { get; set; }

        /// <summary>
        /// Senha de acesso do funcionário.
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        [MaxLength(50, ErrorMessage = "A senha deve ter no máximo 50 caracteres.")]
        [DefaultValue("Senha123")]
        public string Senha { get; set; }
    }
}