using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Tria_2025.DTO
{
    /// <summary>
    /// DTO usado para criação e atualização de Endereços.
    /// </summary>
    public class EnderecoDTO
    {
        /// <summary>
        /// Nome da rua, avenida ou via pública.
        /// </summary>
        [Required(ErrorMessage = "O Logradouro é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Logradouro deve ter até 100 caracteres.")]
        [DefaultValue("Rua dos Andradas")]
        public string Logradouro { get; set; }

        /// <summary>
        /// Nome da Cidade.
        /// </summary>
        [Required(ErrorMessage = "A Cidade é obrigatória.")]
        [StringLength(80, ErrorMessage = "A Cidade deve ter até 80 caracteres.")]
        [DefaultValue("Porto Alegre")]
        public string Cidade { get; set; }

        /// <summary>
        /// Sigla da Unidade Federativa (UF), com 2 caracteres.
        /// </summary>
        [Required(ErrorMessage = "O Estado é obrigatório.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O Estado deve ter 2 caracteres.")]
        [DefaultValue("RS")]
        public string Estado { get; set; }

        /// <summary>
        /// Número do imóvel ou "S/N" (máximo 10 caracteres).
        /// </summary>
        [Required(ErrorMessage = "O Número é obrigatório.")]
        [StringLength(10, ErrorMessage = "O Número deve ter no máximo 10 caracteres.")]
        [DefaultValue("1234")]
        public string Numero { get; set; }

        /// <summary>
        /// Informação adicional, como "Apto 301" (Opcional).
        /// </summary>
        [StringLength(50, ErrorMessage = "O Complemento deve ter no máximo 50 caracteres.")]
        public string Complemento { get; set; }

        /// <summary>
        /// Código de Endereçamento Postal (CEP), 8 dígitos (pode conter formatação - Ex: 90020-010).
        /// </summary>
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(8, ErrorMessage = "O CEP deve ter no máximo 8 caracteres.")]
        [DefaultValue("90020010")]
        public string Cep { get; set; }
    }
}