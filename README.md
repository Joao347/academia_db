# Sistema de Domínio de Academia
## Trabalho M3 - Banco de Dados I

Sistema completo de gestão de academia desenvolvido em MySQL e C# (.NET 8.0).

## Estrutura do Projeto

```
m3 bd academia/
├── Scripts/
│   ├── Academia_CriarEsquema.sql    # Criação do banco e tabelas
│   └── Academia_InserirDados.sql    # Dados de exemplo
├── Academia.Domain/              # Camada de domínio (modelos)
│   └── Models/
│       ├── Membro.cs
│       ├── Plano.cs
│       ├── Matricula.cs
│       ├── Instrutor.cs
│       └── Leciona.cs
├── Academia.Data/                   # Camada de acesso a dados
│   ├── DatabaseConnection.cs
│   └── Repositories/
│       ├── MembroRepository.cs
│       ├── PlanoRepository.cs
│       ├── MatriculaRepository.cs
│       ├── InstrutorRepository.cs
│       └── LecionaRepository.cs
├── Academia.Console/                # Aplicação console
│   └── Program.cs
└── Academia.sln                     # Solução do projeto
```

## Modelo de Dados

### Tabelas Principais:

- **Membros**: Clientes da academia
  - Campos: `membro_id`, `nome`, `cpf`, `email`, `telefone`, `data_nascimento`, `data_cadastro`, `ativo`

- **Planos**: Planos de assinatura disponíveis
  - Campos: `plano_id`, `nome`, `descricao`, `preco`, `duracao_meses`, `ativo`

- **Matriculas**: Relacionamento entre membros e planos
  - Campos: `matricula_id`, `membro_id`, `plano_id`, `data_inicio`, `data_fim`, `valor_pago`, `matricula_status`
  - Foreign Keys: `membro_id` → `Membros(membro_id)`, `plano_id` → `Planos(plano_id)`

- **Instrutores**: Funcionários que orientam os treinos
  - Campos: `instrutor_id`, `nome`, `cpf`, `email`, `telefone`, `especialidade`, `data_contratacao`, `ativo`

- **Leciona**: Relacionamento N:N entre instrutores e membros
  - Campos: `instrutor_id`, `membro_id`, `data_inicio`, `data_fim`, `observacao`
  - Primary Key composta: `(instrutor_id, membro_id, data_inicio)`
  - Foreign Keys: `instrutor_id` → `Instrutores(instrutor_id)`, `membro_id` → `Membros(membro_id)`

## Como Usar

### Pré-requisitos
- MySQL Server instalado
- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code

### Passo 1: Criar o Banco de Dados

1. Abra o MySQL Workbench ou cliente MySQL
2. Execute o script `Scripts/Academia_CriarEsquema.sql`
3. Execute o script `Scripts/Academia_InserirDados.sql` para popular com dados de exemplo

### Passo 2: Configurar a String de Conexão

Edite o arquivo `Academia.Data/DatabaseConnection.cs`:

```csharp
public static string GetDefaultConnectionString()
{
    return "Server=localhost;Database=academia_db;Uid=root;Pwd=SUA_SENHA;";
}
```

### Passo 3: Restaurar Pacotes NuGet

No diretório raiz do projeto, execute:

```bash
dotnet restore
```

### Passo 4: Executar a Aplicação

```bash
dotnet run --project Academia.Console
```

Ou abra a solução `Academia.sln` no Visual Studio e execute o projeto `Academia.Console`.

## Menu da Aplicação

A aplicação apresenta um menu interativo com as seguintes opções:

1. **Gerenciar Membros**
   - Listar, cadastrar, atualizar e excluir membros

2. **Gerenciar Planos**
   - Listar, cadastrar, atualizar e excluir planos de assinatura

3. **Gerenciar Matrículas**
   - Listar, cadastrar, atualizar e excluir matrículas (relacionamento membro-plano)

4. **Gerenciar Leciona (Instrutor-Membro)**
   - Listar todas as relações
   - Cadastrar nova relação instrutor-membro
   - Buscar alunos de um instrutor específico
   - Buscar instrutores de um membro específico
   - Atualizar relação existente
   - Excluir relação

## 🔧 Funcionalidades Implementadas

### CRUDs Disponíveis:

1. **Membros** (Create, Read, Update, Delete)
2. **Planos** (Create, Read, Update, Delete)
3. **Matrículas** (Create, Read, Update, Delete)

4. **Instrutores** (Create, Read, Update, Delete)
   - Cadastro com especialidade
   - Validação de CPF único
   - Soft delete através do campo `ativo`

5. **Leciona** (Create, Read, Update, Delete)
   - Relacionamento N:N entre instrutores e membros
   - Consultas específicas: alunos por instrutor e instrutores por membro
   - Controle de datas de início e fim do relacionamento

### Características Técnicas:

- **ORM**: Dapper (micro-ORM leve e performático)
- **Banco de Dados**: MySQL
- **Padrão de Arquitetura**: Camadas (Domain, Data, Console)
- **Soft Delete**: Implementado para Membros, Planos e Instrutores
- **Documentação**: Comentários SQL nos repositórios explicando as queries executadas

## Observações

- Todos os métodos CRUD estão implementados e testados
- Os comandos SQL estão documentados nos comentários dos repositórios
- O sistema utiliza transações implícitas através do Dapper
- Validações de integridade referencial são mantidas pelo MySQL através de Foreign Keys
- A aplicação console oferece interface amigável para todas as operações

## Dependências

- **Dapper**: Micro-ORM para acesso a dados
- **MySql.Data**: Driver MySQL para .NET
- **.NET 8.0**: Framework base

## Licença

Este projeto é um trabalho acadêmico desenvolvido para a disciplina de Banco de Dados I da Universidade do Vale do Itajaí.


