using Academia.Domain.Models;
using Dapper;
using System.Data;

namespace Academia.Data.Repositories
{
    public class PlanoRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public PlanoRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // CREATE - Inserir novo plano
        // SQL equivalente: INSERT INTO Planos (nome, descricao, preco, duracao_meses, ativo) 
        //                  VALUES (@Nome, @Descricao, @Preco, @DuracaoMeses, @Ativo)
        public async Task<int> CreateAsync(Plano plano)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = @"
                INSERT INTO Planos (nome, descricao, preco, duracao_meses, ativo)
                VALUES (@Nome, @Descricao, @Preco, @DuracaoMeses, @Ativo);
                SELECT LAST_INSERT_ID();";

            return await connection.QuerySingleAsync<int>(sql, plano);
        }

        // READ - Buscar plano por ID
        // SQL equivalente: SELECT * FROM Planos WHERE plano_id = @Id
        public async Task<Plano?> GetByIdAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "SELECT plano_id AS PlanoId, nome AS Nome, descricao AS Descricao, " +
                     "preco AS Preco, duracao_meses AS DuracaoMeses, ativo AS Ativo " +
                     "FROM Planos WHERE plano_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Plano>(sql, new { Id = id });
        }

        // READ - Listar todos os planos
        // SQL equivalente: SELECT * FROM Planos WHERE ativo = 1 ORDER BY nome
        public async Task<List<Plano>> GetAllAsync()
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "SELECT plano_id AS PlanoId, nome AS Nome, descricao AS Descricao, " +
                     "preco AS Preco, duracao_meses AS DuracaoMeses, ativo AS Ativo " +
                     "FROM Planos WHERE ativo = 1 ORDER BY nome";
            
            var result = await connection.QueryAsync<Plano>(sql);
            return result.ToList();
        }

        // UPDATE - Atualizar plano
        // SQL equivalente: UPDATE Planos SET nome = @Nome, descricao = @Descricao, 
        //                  preco = @Preco, duracao_meses = @DuracaoMeses, ativo = @Ativo
        //                  WHERE plano_id = @PlanoId
        public async Task<bool> UpdateAsync(Plano plano)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = @"
                UPDATE Planos 
                SET nome = @Nome, descricao = @Descricao, preco = @Preco, 
                    duracao_meses = @DuracaoMeses, ativo = @Ativo
                WHERE plano_id = @PlanoId";

            var rowsAffected = await connection.ExecuteAsync(sql, plano);
            return rowsAffected > 0;
        }

        // DELETE - Excluir plano (soft delete)
        // SQL equivalente: UPDATE Planos SET ativo = 0 WHERE plano_id = @Id
        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "UPDATE Planos SET ativo = 0 WHERE plano_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}



