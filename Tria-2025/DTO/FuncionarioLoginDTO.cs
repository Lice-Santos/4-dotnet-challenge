using System.ComponentModel.DataAnnotations;

namespace Tria_2025.Models
{
    public class FuncionarioLoginDTO
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; }
    }
}
