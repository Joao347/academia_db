using Academia.Domain.Models;
using Dapper;
using System.Data;

namespace Academia.Data.Repositories
{
    public class MembroRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public MembroRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // CREATE - Inserir novo membro
        // SQL equivalente: INSERT INTO Membros (nome, cpf, email, telefone, data_nascimento, data_cadastro, ativo) 
        //                  VALUES (@Nome, @CPF, @Email, @Telefone, @DataNascimento, @DataCadastro, @Ativo)
        public async Task<int> CreateAsync(Membro membro)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = @"
                INSERT INTO Membros (nome, cpf, email, telefone, data_nascimento, data_cadastro, ativo)
                VALUES (@Nome, @CPF, @Email, @Telefone, @DataNascimento, @DataCadastro, @Ativo);
                SELECT LAST_INSERT_ID();";

            return await connection.QuerySingleAsync<int>(sql, new
            {
                membro.Nome,
                membro.CPF,
                membro.Email,
                membro.Telefone,
                DataNascimento = membro.DataNascimento,
                DataCadastro = membro.DataCadastro,
                membro.Ativo
            });
        }

        // READ - Buscar membro por ID
        // SQL equivalente: SELECT * FROM Membros WHERE membro_id = @Id
        public async Task<Membro?> GetByIdAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "SELECT membro_id AS MembroId, nome AS Nome, cpf AS CPF, email AS Email, " +
                     "telefone AS Telefone, data_nascimento AS DataNascimento, " +
                     "data_cadastro AS DataCadastro, ativo AS Ativo " +
                     "FROM Membros WHERE membro_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Membro>(sql, new { Id = id });
        }

        // READ - Listar todos os membros
        // SQL equivalente: SELECT * FROM Membros WHERE ativo = 1 ORDER BY nome
        public async Task<List<Membro>> GetAllAsync()
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "SELECT membro_id AS MembroId, nome AS Nome, cpf AS CPF, email AS Email, " +
                     "telefone AS Telefone, data_nascimento AS DataNascimento, " +
                     "data_cadastro AS DataCadastro, ativo AS Ativo " +
                     "FROM Membros WHERE ativo = 1 ORDER BY nome";
            
            var result = await connection.QueryAsync<Membro>(sql);
            return result.ToList();
        }

        // UPDATE - Atualizar membro
        // SQL equivalente: UPDATE Membros SET nome = @Nome, cpf = @CPF, email = @Email, 
        //                  telefone = @Telefone, data_nascimento = @DataNascimento, ativo = @Ativo
        //                  WHERE membro_id = @MembroId
        public async Task<bool> UpdateAsync(Membro membro)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = @"
                UPDATE Membros 
                SET nome = @Nome, cpf = @CPF, email = @Email, telefone = @Telefone, 
                    data_nascimento = @DataNascimento, ativo = @Ativo
                WHERE membro_id = @MembroId";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                membro.MembroId,
                membro.Nome,
                membro.CPF,
                membro.Email,
                membro.Telefone,
                DataNascimento = membro.DataNascimento,
                membro.Ativo
            });
            
            return rowsAffected > 0;
        }

        // DELETE - Excluir membro (soft delete)
        // SQL equivalente: UPDATE Membros SET ativo = 0 WHERE membro_id = @Id
        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "UPDATE Membros SET ativo = 0 WHERE membro_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}



