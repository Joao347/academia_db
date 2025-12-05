using Academia.Domain.Models;
using Dapper;
using System.Data;

namespace Academia.Data.Repositories
{
    public class LecionaRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public LecionaRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // CREATE - Criar relação instrutor-membro
        // SQL equivalente: INSERT INTO Leciona (instrutor_id, membro_id, data_inicio, data_fim, observacao) 
        //                  VALUES (@InstrutorId, @MembroId, @DataInicio, @DataFim, @Observacao)
        public async Task<bool> CreateAsync(Leciona leciona)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = @"
                INSERT INTO Leciona (instrutor_id, membro_id, data_inicio, data_fim, observacao)
                VALUES (@InstrutorId, @MembroId, @DataInicio, @DataFim, @Observacao)";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                leciona.InstrutorId,
                leciona.MembroId,
                DataInicio = leciona.DataInicio,
                DataFim = leciona.DataFim,
                leciona.Observacao
            });

            return rowsAffected > 0;
        }

        // READ - Buscar relação específica
        // SQL equivalente: SELECT * FROM Leciona WHERE instrutor_id = @InstrutorId 
        //                  AND membro_id = @MembroId AND data_inicio = @DataInicio
        public async Task<Leciona?> GetByIdAsync(int instrutorId, int membroId, DateTime dataInicio)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "SELECT instrutor_id AS InstrutorId, membro_id AS MembroId, " +
                     "data_inicio AS DataInicio, data_fim AS DataFim, observacao AS Observacao " +
                     "FROM Leciona WHERE instrutor_id = @InstrutorId AND membro_id = @MembroId " +
                     "AND data_inicio = @DataInicio";

            return await connection.QueryFirstOrDefaultAsync<Leciona>(sql, new
            {
                InstrutorId = instrutorId,
                MembroId = membroId,
                DataInicio = dataInicio
            });
        }

        // READ - Listar todos os relacionamentos
        // SQL equivalente: SELECT * FROM Leciona ORDER BY data_inicio DESC
        public async Task<List<Leciona>> GetAllAsync()
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "SELECT instrutor_id AS InstrutorId, membro_id AS MembroId, " +
                     "data_inicio AS DataInicio, data_fim AS DataFim, observacao AS Observacao " +
                     "FROM Leciona ORDER BY data_inicio DESC";

            var result = await connection.QueryAsync<Leciona>(sql);
            return result.ToList();
        }

        // READ - Buscar alunos de um instrutor específico (com informações do membro)
        // SQL equivalente: SELECT l.*, m.* FROM Leciona l
        //                  INNER JOIN Membros m ON l.membro_id = m.membro_id
        //                  WHERE l.instrutor_id = @InstrutorId AND (l.data_fim IS NULL OR l.data_fim >= CURRENT_DATE)
        public async Task<List<Leciona>> GetAlunosPorInstrutorAsync(int instrutorId)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = @"
                SELECT l.instrutor_id AS InstrutorId, l.membro_id AS MembroId, 
                       l.data_inicio AS DataInicio, l.data_fim AS DataFim, 
                       l.observacao AS Observacao,
                       m.membro_id AS MembroId, m.nome AS Nome, m.email AS Email, 
                       m.cpf AS CPF, m.telefone AS Telefone, m.data_nascimento AS DataNascimento,
                       m.data_cadastro AS DataCadastro, m.ativo AS Ativo
                FROM Leciona l
                INNER JOIN Membros m ON l.membro_id = m.membro_id
                WHERE l.instrutor_id = @InstrutorId 
                  AND (l.data_fim IS NULL OR l.data_fim >= CURRENT_DATE)
                ORDER BY l.data_inicio DESC";

            var result = await connection.QueryAsync<Leciona, Membro, Leciona>(
                sql,
                (leciona, membro) =>
                {
                    leciona.Membro = membro;
                    return leciona;
                },
                new { InstrutorId = instrutorId },
                splitOn: "MembroId"
            );

            return result.ToList();
        }

        // READ - Buscar instrutores de um membro específico
        // SQL equivalente: SELECT l.*, i.* FROM Leciona l
        //                  INNER JOIN Instrutores i ON l.instrutor_id = i.instrutor_id
        //                  WHERE l.membro_id = @MembroId AND (l.data_fim IS NULL OR l.data_fim >= CURRENT_DATE)
        public async Task<List<Leciona>> GetInstrutoresPorMembroAsync(int membroId)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = @"
                SELECT l.instrutor_id AS InstrutorId, l.membro_id AS MembroId, 
                       l.data_inicio AS DataInicio, l.data_fim AS DataFim, 
                       l.observacao AS Observacao,
                       i.instrutor_id AS InstrutorId, i.nome AS Nome, i.cpf AS CPF,
                       i.email AS Email, i.telefone AS Telefone, i.especialidade AS Especialidade,
                       i.data_contratacao AS DataContratacao, i.ativo AS Ativo
                FROM Leciona l
                INNER JOIN Instrutores i ON l.instrutor_id = i.instrutor_id
                WHERE l.membro_id = @MembroId 
                  AND (l.data_fim IS NULL OR l.data_fim >= CURRENT_DATE)
                ORDER BY l.data_inicio DESC";

            var result = await connection.QueryAsync<Leciona, Instrutor, Leciona>(
                sql,
                (leciona, instrutor) =>
                {
                    leciona.Instrutor = instrutor;
                    return leciona;
                },
                new { MembroId = membroId },
                splitOn: "InstrutorId"
            );

            return result.ToList();
        }

        // UPDATE - Atualizar relação
        // SQL equivalente: UPDATE Leciona SET data_fim = @DataFim, observacao = @Observacao
        //                  WHERE instrutor_id = @InstrutorId AND membro_id = @MembroId 
        //                  AND data_inicio = @DataInicio
        public async Task<bool> UpdateAsync(Leciona leciona)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = @"
                UPDATE Leciona 
                SET data_fim = @DataFim, observacao = @Observacao
                WHERE instrutor_id = @InstrutorId AND membro_id = @MembroId 
                  AND data_inicio = @DataInicio";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                leciona.InstrutorId,
                leciona.MembroId,
                DataInicio = leciona.DataInicio,
                DataFim = leciona.DataFim,
                leciona.Observacao
            });

            return rowsAffected > 0;
        }

        // DELETE - Excluir relação
        // SQL equivalente: DELETE FROM Leciona WHERE instrutor_id = @InstrutorId 
        //                  AND membro_id = @MembroId AND data_inicio = @DataInicio
        public async Task<bool> DeleteAsync(int instrutorId, int membroId, DateTime dataInicio)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "DELETE FROM Leciona WHERE instrutor_id = @InstrutorId " +
                     "AND membro_id = @MembroId AND data_inicio = @DataInicio";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                InstrutorId = instrutorId,
                MembroId = membroId,
                DataInicio = dataInicio
            });

            return rowsAffected > 0;
        }
    }
}

