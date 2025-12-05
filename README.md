# Sistema de Domínio de Academia
## Trabalho M3 - Banco de Dados I

Sistema completo de gestão de academia desenvolvido em MySQL e C# (.NET 8.0).

## 📋 Estrutura do Projeto

```
m3 bd academia/
├── Scripts/
│   ├── Academia_CriarEsquema.sql    # Criação do banco e tabelas
│   └── Academia_InserirDados.sql    # Dados de exemplo
├── Academia.Domain/              # Camada de domínio (modelos)
│   └── Models/
│       ├── Membro.cs
│       ├── Plano.cs
│       └── Matricula.cs
├── Academia.Data/               # Camada de acesso a dados
│   ├── DatabaseConnection.cs
│   └── Repositories/
│       ├── MembroRepository.cs
│       ├── PlanoRepository.cs
│       └── MatriculaRepository.cs
└── Academia.Console/            # Aplicação console
    └── Program.cs
```

## 🗄️ Modelo de Dados

### Tabelas Principais:
- **Membros**: Clientes da academia
- **Planos**: Planos de assinatura disponíveis
- **Matriculas**: Relacionamento entre membros e planos
- **Instrutores**: Funcionários que orientam os treinos

## 🚀 Como Usar

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
return "Server=localhost;Database=academia_db;Uid=root;Pwd=SUA_SENHA;";
```

### Passo 3: Restaurar Pacotes NuGet

```bash
dotnet restore
```

### Passo 4: Executar a Aplicação

```bash
dotnet run --project Academia.Console
```

## 🔧 Funcionalidades Implementadas

### CRUDs Disponíveis:

1. **Membros** (Create, Read, Update, Delete)
2. **Planos** (Create, Read, Update, Delete)
3. **Matrículas** (Create, Read, Update, Delete)

Cada repositório contém comentários com o SQL equivalente que está sendo executado.

## 📝 Observações

- Todos os métodos CRUD estão implementados
- Os comandos SQL estão documentados nos comentários dos repositórios
- O sistema usa Dapper como ORM leve
- Soft delete implementado para Membros e Planos (campo `ativo`)

## 📄 Licença

Este projeto é um trabalho acadêmico.


