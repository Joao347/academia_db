using Academia.Domain.Models;
using Dapper;
using System.Data;

namespace Academia.Data.Repositories
{
    public class InstrutorRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public InstrutorRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // READ - Listar todos os instrutores
        // SQL equivalente: SELECT * FROM Instrutores WHERE ativo = 1 ORDER BY nome
        public async Task<List<Instrutor>> GetAllAsync()
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "SELECT instrutor_id AS InstrutorId, nome AS Nome, cpf AS CPF, " +
                     "email AS Email, telefone AS Telefone, especialidade AS Especialidade, " +
                     "data_contratacao AS DataContratacao, ativo AS Ativo " +
                     "FROM Instrutores WHERE ativo = 1 ORDER BY nome";

            var result = await connection.QueryAsync<Instrutor>(sql);
            return result.ToList();
        }

        // READ - Buscar instrutor por ID
        // SQL equivalente: SELECT * FROM Instrutores WHERE instrutor_id = @Id
        public async Task<Instrutor?> GetByIdAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "SELECT instrutor_id AS InstrutorId, nome AS Nome, cpf AS CPF, " +
                     "email AS Email, telefone AS Telefone, especialidade AS Especialidade, " +
                     "data_contratacao AS DataContratacao, ativo AS Ativo " +
                     "FROM Instrutores WHERE instrutor_id = @Id";

            return await connection.QueryFirstOrDefaultAsync<Instrutor>(sql, new { Id = id });
        }
    }
}

