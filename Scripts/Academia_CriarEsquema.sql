CREATE SCHEMA IF NOT EXISTS academia_db;
#DROP SCHEMA academia_db;

USE academia_db;

CREATE TABLE Membros (
    membro_id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(255) NOT NULL,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL,
    telefone VARCHAR(20) NOT NULL,
    data_nascimento DATE,
    data_cadastro DATE DEFAULT (CURRENT_DATE),
    ativo BOOLEAN DEFAULT TRUE
);

CREATE TABLE Planos (
	plano_id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(255) NOT NULL,
    descricao TEXT,
    preco DECIMAL(10,2) NOT NULL,
    duracao_meses INT NOT NULL,
    ativo BOOLEAN DEFAULT TRUE
);

CREATE TABLE Matriculas (
    matricula_id INT PRIMARY KEY AUTO_INCREMENT,
    membro_id INT NOT NULL,
    plano_id INT NOT NULL,
    data_inicio DATE NOT NULL,
    data_fim DATE,
    valor_pago DECIMAL(10,2) NOT NULL,
    matricula_status VARCHAR(50) DEFAULT 'Ativa',
    FOREIGN KEY (membro_id) REFERENCES Membros(membro_id) ON DELETE CASCADE,
    FOREIGN KEY (plano_id) REFERENCES Planos(plano_id) ON DELETE CASCADE
);

CREATE TABLE Instrutores (
    instrutor_id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(255) NOT NULL,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL,
    telefone VARCHAR(20) NOT NULL,
    especialidade VARCHAR(255),
    data_contratacao DATE DEFAULT (CURRENT_DATE),
    ativo BOOLEAN DEFAULT TRUE
);

CREATE TABLE Leciona (
    instrutor_id INT NOT NULL,
    membro_id INT NOT NULL,
    data_inicio DATE NOT NULL,
    data_fim DATE,
    observacao VARCHAR(255),

    PRIMARY KEY (instrutor_id, membro_id, data_inicio),
    FOREIGN KEY (instrutor_id) REFERENCES Instrutores(instrutor_id) ON DELETE CASCADE,
    FOREIGN KEY (membro_id) REFERENCES Membros(membro_id) ON DELETE CASCADE
);