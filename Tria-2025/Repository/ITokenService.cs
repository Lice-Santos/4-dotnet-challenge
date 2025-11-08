namespace Tria_2025.Interface
{
    public interface ITokenService
    {
        /// <summary>
        /// Faz login do usuário e retorna um token JWT se as credenciais forem válidas.
        /// </summary>
        Task<string> LoginAndGenerateTokenAsync(string email, string senha);

        /// <summary>
        /// Registra um novo usuário no sistema.
        /// </summary>
        Task<bool> RegisterUserAsync(string nome, string email, string senha, string cargo);

        /// <summary>
        /// Gera um token JWT válido para o usuário autenticado.
        /// </summary>
        string GenerateToken(int userId, string nome, string cargo);
    }
}
