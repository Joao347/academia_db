using Academia.Domain.Models;
using Dapper;
using System.Data;

namespace Academia.Data.Repositories
{
    public class MatriculaRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public MatriculaRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // CREATE - Inserir nova matrícula
        // SQL equivalente: INSERT INTO Matriculas (membro_id, plano_id, data_inicio, data_fim, valor_pago, matricula_status) 
        //                  VALUES (@MembroId, @PlanoId, @DataInicio, @DataFim, @ValorPago, @Status)
        public async Task<int> CreateAsync(Matricula matricula)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = @"
                INSERT INTO Matriculas (membro_id, plano_id, data_inicio, data_fim, valor_pago, matricula_status)
                VALUES (@MembroId, @PlanoId, @DataInicio, @DataFim, @ValorPago, @Status);
                SELECT LAST_INSERT_ID();";

            return await connection.QuerySingleAsync<int>(sql, new
            {
                matricula.MembroId,
                matricula.PlanoId,
                DataInicio = matricula.DataInicio,
                DataFim = matricula.DataFim,
                ValorPago = matricula.ValorPago,
                matricula.Status
            });
        }

        // READ - Buscar matrícula por ID
        // SQL equivalente: SELECT * FROM Matriculas WHERE matricula_id = @Id
        public async Task<Matricula?> GetByIdAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "SELECT matricula_id AS MatriculaId, membro_id AS MembroId, plano_id AS PlanoId, " +
                     "data_inicio AS DataInicio, data_fim AS DataFim, valor_pago AS ValorPago, matricula_status AS Status " +
                     "FROM Matriculas WHERE matricula_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Matricula>(sql, new { Id = id });
        }

        // READ - Listar todas as matrículas
        // SQL equivalente: SELECT * FROM Matriculas ORDER BY data_inicio DESC
        public async Task<List<Matricula>> GetAllAsync()
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "SELECT matricula_id AS MatriculaId, membro_id AS MembroId, plano_id AS PlanoId, " +
                     "data_inicio AS DataInicio, data_fim AS DataFim, valor_pago AS ValorPago, matricula_status AS Status " +
                     "FROM Matriculas ORDER BY data_inicio DESC";
            
            var result = await connection.QueryAsync<Matricula>(sql);
            return result.ToList();
        }

        // UPDATE - Atualizar matrícula
        // SQL equivalente: UPDATE Matriculas SET membro_id = @MembroId, plano_id = @PlanoId, 
        //                  data_inicio = @DataInicio, data_fim = @DataFim, valor_pago = @ValorPago, matricula_status = @Status
        //                  WHERE matricula_id = @MatriculaId
        public async Task<bool> UpdateAsync(Matricula matricula)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = @"
                UPDATE Matriculas 
                SET membro_id = @MembroId, plano_id = @PlanoId, data_inicio = @DataInicio, 
                    data_fim = @DataFim, valor_pago = @ValorPago, matricula_status = @Status
                WHERE matricula_id = @MatriculaId";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                matricula.MatriculaId,
                matricula.MembroId,
                matricula.PlanoId,
                DataInicio = matricula.DataInicio,
                DataFim = matricula.DataFim,
                ValorPago = matricula.ValorPago,
                matricula.Status
            });
            
            return rowsAffected > 0;
        }

        // DELETE - Excluir matrícula
        // SQL equivalente: DELETE FROM Matriculas WHERE matricula_id = @Id
        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            var sql = "DELETE FROM Matriculas WHERE matricula_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}



