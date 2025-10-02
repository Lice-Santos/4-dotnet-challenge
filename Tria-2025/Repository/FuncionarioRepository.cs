using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.Exceptions;
using Tria_2025.Models;
using Tria_2025.Repository;
using static System.Net.Mime.MediaTypeNames;

namespace Tria_2025.Repository
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly AppDbContext _context;

        public FuncionarioRepository(AppDbContext context)
        {
            _context = context;
        }

        // --- MÉTODOS CRUD BÁSICOS ---

        public async Task<Funcionario> AddAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();
            return funcionario;
        }

        public async Task UpdateAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Attach(funcionario);
            _context.Entry(funcionario).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Remove(funcionario);
            await SaveChangesAsync();
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<Funcionario> GetByIdAsync(int id)
        {
            return await _context.Funcionarios.FindAsync(id);
        }

        public async Task<IEnumerable<Funcionario>> GetAllAsync()
        {
            return await _context.Funcionarios.ToListAsync();
        }

        // --- MÉTODO ESPECÍFICO PARA VALIDAÇÃO ---

        public async Task<bool> EmailExistsAsync(string email)
        {

            string emailNormalized = email.ToUpper().Trim();


            return await _context.Funcionarios
                                 .CountAsync(f => f.Email.ToUpper() == emailNormalized) > 0;

        }


        // --- MÉTODO AUXILIAR ---

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<Funcionario> GetByEmailAndSenhaAsync(string email, string senha)
        {
            string emailNormalized = email.ToUpper().Trim();


            return await _context.Funcionarios
                                 .FirstOrDefaultAsync(f => f.Email.ToUpper() == emailNormalized &&
                                                           f.Senha == senha);
        }
    }
}