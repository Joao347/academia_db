USE academia_db;

INSERT INTO Planos (nome, descricao, preco, duracao_meses) VALUES
('Plano Mensal', 'Acesso completo à academia por 1 mês', 89.90, 1),
('Plano Trimestral', 'Acesso completo à academia por 3 meses', 249.90, 3),
('Plano Semestral', 'Acesso completo à academia por 6 meses', 449.90, 6),
('Plano Anual', 'Acesso completo à academia por 12 meses', 799.90, 12);

INSERT INTO Membros (nome, cpf, email, telefone, data_nascimento) VALUES
('João Silva', '123.456.789-00', 'joao.silva@email.com', '(48) 99999-8888', '1990-05-15'),
('Maria Santos', '234.567.890-11', 'maria.santos@email.com', '(48) 98888-7777', '1985-08-20'),
('Pedro Oliveira', '345.678.901-22', 'pedro.oliveira@email.com', '(48) 97777-6666', '1992-03-10'),
('Ana Costa', '456.789.012-33', 'ana.costa@email.com', '(48) 96666-5555', '1988-11-25'),
('Carlos Pereira', '567.890.123-44', 'carlos.pereira@email.com', '(48) 95555-4444', '1995-07-30');

INSERT INTO Instrutores (nome, cpf, email, telefone, especialidade) VALUES
('Roberto Alves', '111.222.333-44', 'roberto.alves@academia.com', '(48) 91111-2222', 'Musculação'),
('Fernanda Lima', '222.333.444-55', 'fernanda.lima@academia.com', '(48) 92222-3333', 'Pilates'),
('Marcos Souza', '333.444.555-66', 'marcos.souza@academia.com', '(48) 93333-4444', 'Crossfit');

INSERT INTO Matriculas (membro_id, plano_id, data_inicio, data_fim, valor_pago, matricula_status) VALUES
(1, 1, '2024-01-01', '2024-02-01', 89.90, 'Ativa'),
(2, 2, '2024-01-15', '2024-04-15', 249.90, 'Ativa'),
(3, 1, '2024-02-01', '2024-03-01', 89.90, 'Encerrada'),
(4, 3, '2024-01-10', '2024-07-10', 449.90, 'Ativa'),
(5, 1, '2024-02-15', '2024-03-15', 89.90, 'Ativa');

INSERT INTO Leciona (instrutor_id, membro_id, data_inicio, data_fim, observacao) VALUES
(1, 1, '2024-01-01', NULL, 'Treino de musculação iniciante'),
(1, 2, '2024-01-15', NULL, 'Treino avançado de musculação'),
(2, 3, '2024-02-01', '2024-03-01', 'Aulas de pilates'),
(2, 4, '2024-01-10', NULL, 'Pilates para iniciantes'),
(3, 5, '2024-02-15', NULL, 'Crossfit intensivo'),
(3, 1, '2024-03-01', NULL, 'Crossfit complementar');