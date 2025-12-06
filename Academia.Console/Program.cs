using Academia.Data;
using Academia.Data.Repositories;
using Academia.Domain.Models;

namespace Academia.Console
{
    class Program
    {
        private static DatabaseConnection? _dbConnection;
        private static MembroRepository? _membroRepository;
        private static PlanoRepository? _planoRepository;
        private static MatriculaRepository? _matriculaRepository;
        private static InstrutorRepository? _instrutorRepository;
        private static LecionaRepository? _lecionaRepository;

        static async Task Main(string[] args)
        {
            try
            {
                // Configurar conexão e repositórios
                var connectionString = DatabaseConnection.GetDefaultConnectionString();
                _dbConnection = new DatabaseConnection(connectionString);
                _membroRepository = new MembroRepository(_dbConnection);
                _planoRepository = new PlanoRepository(_dbConnection);
                _matriculaRepository = new MatriculaRepository(_dbConnection);
                _instrutorRepository = new InstrutorRepository(_dbConnection);
                _lecionaRepository = new LecionaRepository(_dbConnection);

                System.Console.WriteLine("=== Sistema de Gestão de Academia ===\n");

                bool continuar = true;
                while (continuar)
                {
                    MostrarMenuPrincipal();
                    var opcao = System.Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            await GerenciarMembros();
                            break;
                        case "2":
                            await GerenciarPlanos();
                            break;
                        case "3":
                            await GerenciarMatriculas();
                            break;
                        case "4":
                            await GerenciarInstrutores();
                            break;
                        case "5":
                            await GerenciarLeciona();
                            break;
                        case "0":
                            continuar = false;
                            System.Console.WriteLine("\nEncerrando aplicação...");
                            break;
                        default:
                            System.Console.WriteLine("\nOpção inválida! Tente novamente.");
                            break;
                    }

                    if (continuar)
                    {
                        System.Console.WriteLine("\nPressione qualquer tecla para continuar...");
                        System.Console.ReadKey();
                        System.Console.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"\nErro: {ex.Message}");
                System.Console.WriteLine("Pressione qualquer tecla para sair...");
                System.Console.ReadKey();
            }
        }

        static void MostrarMenuPrincipal()
        {
            System.Console.WriteLine("=== MENU PRINCIPAL ===");
            System.Console.WriteLine("1. Gerenciar Membros");
            System.Console.WriteLine("2. Gerenciar Planos");
            System.Console.WriteLine("3. Gerenciar Matrículas");
            System.Console.WriteLine("4. Gerenciar Instrutores");
            System.Console.WriteLine("5. Gerenciar Leciona (Instrutor-Membro)");
            System.Console.WriteLine("0. Sair");
            System.Console.Write("\nEscolha uma opção: ");
        }

        #region Gerenciar Membros
        static async Task GerenciarMembros()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== GERENCIAR MEMBROS ===\n");
            System.Console.WriteLine("1. Listar Membros");
            System.Console.WriteLine("2. Cadastrar Membro");
            System.Console.WriteLine("3. Buscar Membro por ID");
            System.Console.WriteLine("4. Atualizar Membro");
            System.Console.WriteLine("5. Excluir Membro");
            System.Console.Write("\nEscolha uma opção: ");

            var opcao = System.Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    await ListarMembros();
                    break;
                case "2":
                    await CadastrarMembro();
                    break;
                case "3":
                    await BuscarMembroPorId();
                    break;
                case "4":
                    await AtualizarMembro();
                    break;
                case "5":
                    await ExcluirMembro();
                    break;
                default:
                    System.Console.WriteLine("Opção inválida!");
                    break;
            }
        }

        static async Task ListarMembros()
        {
            System.Console.WriteLine("\n=== LISTA DE MEMBROS ===\n");
            var membros = await _membroRepository!.GetAllAsync();

            if (membros.Count == 0)
            {
                System.Console.WriteLine("Nenhum membro encontrado.");
                return;
            }

            foreach (var membro in membros)
            {
                System.Console.WriteLine($"ID: {membro.MembroId} | Nome: {membro.Nome} | CPF: {membro.CPF} | Email: {membro.Email}");
            }
        }

        static async Task CadastrarMembro()
        {
            System.Console.WriteLine("\n=== CADASTRAR MEMBRO ===\n");

            System.Console.Write("Nome: ");
            var nome = System.Console.ReadLine() ?? "";

            System.Console.Write("CPF: ");
            var cpf = System.Console.ReadLine() ?? "";

            System.Console.Write("Email: ");
            var email = System.Console.ReadLine() ?? "";

            System.Console.Write("Telefone: ");
            var telefone = System.Console.ReadLine() ?? "";

            System.Console.Write("Data de Nascimento (dd/MM/yyyy) ou Enter para pular: ");
            var dataNascStr = System.Console.ReadLine();
            DateTime? dataNasc = null;
            if (!string.IsNullOrEmpty(dataNascStr) && DateTime.TryParse(dataNascStr, out var dt))
            {
                dataNasc = dt;
            }

            var membro = new Membro
            {
                Nome = nome,
                CPF = cpf,
                Email = email,
                Telefone = telefone,
                DataNascimento = dataNasc,
                DataCadastro = DateTime.Now,
                Ativo = true
            };

            var id = await _membroRepository!.CreateAsync(membro);
            System.Console.WriteLine($"\nMembro cadastrado com sucesso! ID: {id}");
        }

        static async Task BuscarMembroPorId()
        {
            System.Console.Write("\nDigite o ID do membro: ");
            if (int.TryParse(System.Console.ReadLine(), out var id))
            {
                var membro = await _membroRepository!.GetByIdAsync(id);
                if (membro != null)
                {
                    System.Console.WriteLine($"\nID: {membro.MembroId}");
                    System.Console.WriteLine($"Nome: {membro.Nome}");
                    System.Console.WriteLine($"CPF: {membro.CPF}");
                    System.Console.WriteLine($"Email: {membro.Email}");
                    System.Console.WriteLine($"Telefone: {membro.Telefone}");
                    System.Console.WriteLine($"Data Nascimento: {membro.DataNascimento?.ToString("dd/MM/yyyy") ?? "N/A"}");
                    System.Console.WriteLine($"Data Cadastro: {membro.DataCadastro:dd/MM/yyyy}");
                    System.Console.WriteLine($"Ativo: {(membro.Ativo ? "Sim" : "Não")}");
                }
                else
                {
                    System.Console.WriteLine("Membro não encontrado.");
                }
            }
            else
            {
                System.Console.WriteLine("ID inválido.");
            }
        }

        static async Task AtualizarMembro()
        {
            System.Console.Write("\nDigite o ID do membro a atualizar: ");
            if (!int.TryParse(System.Console.ReadLine(), out var id))
            {
                System.Console.WriteLine("ID inválido.");
                return;
            }

            var membro = await _membroRepository!.GetByIdAsync(id);
            if (membro == null)
            {
                System.Console.WriteLine("Membro não encontrado.");
                return;
            }

            System.Console.WriteLine($"\nAtualizando membro: {membro.Nome}\n");

            System.Console.Write($"Nome ({membro.Nome}): ");
            var nome = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(nome)) membro.Nome = nome;

            System.Console.Write($"CPF ({membro.CPF}): ");
            var cpf = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(cpf)) membro.CPF = cpf;

            System.Console.Write($"Email ({membro.Email}): ");
            var email = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(email)) membro.Email = email;

            System.Console.Write($"Telefone ({membro.Telefone}): ");
            var telefone = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(telefone)) membro.Telefone = telefone;

            var sucesso = await _membroRepository!.UpdateAsync(membro);
            if (sucesso)
                System.Console.WriteLine("\nMembro atualizado com sucesso!");
            else
                System.Console.WriteLine("\nErro ao atualizar membro.");
        }

        static async Task ExcluirMembro()
        {
            System.Console.Write("\nDigite o ID do membro a excluir: ");
            if (int.TryParse(System.Console.ReadLine(), out var id))
            {
                var sucesso = await _membroRepository!.DeleteAsync(id);
                if (sucesso)
                    System.Console.WriteLine("Membro excluído com sucesso!");
                else
                    System.Console.WriteLine("Erro ao excluir membro ou membro não encontrado.");
            }
            else
            {
                System.Console.WriteLine("ID inválido.");
            }
        }
        #endregion

        #region Gerenciar Planos
        static async Task GerenciarPlanos()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== GERENCIAR PLANOS ===\n");
            System.Console.WriteLine("1. Listar Planos");
            System.Console.WriteLine("2. Cadastrar Plano");
            System.Console.WriteLine("3. Buscar Plano por ID");
            System.Console.WriteLine("4. Atualizar Plano");
            System.Console.WriteLine("5. Excluir Plano");
            System.Console.Write("\nEscolha uma opção: ");

            var opcao = System.Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    await ListarPlanos();
                    break;
                case "2":
                    await CadastrarPlano();
                    break;
                case "3":
                    await BuscarPlanoPorId();
                    break;
                case "4":
                    await AtualizarPlano();
                    break;
                case "5":
                    await ExcluirPlano();
                    break;
                default:
                    System.Console.WriteLine("Opção inválida!");
                    break;
            }
        }

        static async Task ListarPlanos()
        {
            System.Console.WriteLine("\n=== LISTA DE PLANOS ===\n");
            var planos = await _planoRepository!.GetAllAsync();

            if (planos.Count == 0)
            {
                System.Console.WriteLine("Nenhum plano encontrado.");
                return;
            }

            foreach (var plano in planos)
            {
                System.Console.WriteLine($"ID: {plano.PlanoId} | Nome: {plano.Nome} | Preço: R$ {plano.Preco:F2} | Duração: {plano.DuracaoMeses} meses");
            }
        }

        static async Task CadastrarPlano()
        {
            System.Console.WriteLine("\n=== CADASTRAR PLANO ===\n");

            System.Console.Write("Nome: ");
            var nome = System.Console.ReadLine() ?? "";

            System.Console.Write("Descrição (opcional): ");
            var descricao = System.Console.ReadLine();

            System.Console.Write("Preço: ");
            if (!decimal.TryParse(System.Console.ReadLine(), out var preco))
            {
                System.Console.WriteLine("Preço inválido.");
                return;
            }

            System.Console.Write("Duração (meses): ");
            if (!int.TryParse(System.Console.ReadLine(), out var duracao))
            {
                System.Console.WriteLine("Duração inválida.");
                return;
            }

            var plano = new Plano
            {
                Nome = nome,
                Descricao = descricao,
                Preco = preco,
                DuracaoMeses = duracao,
                Ativo = true
            };

            var id = await _planoRepository!.CreateAsync(plano);
            System.Console.WriteLine($"\nPlano cadastrado com sucesso! ID: {id}");
        }

        static async Task BuscarPlanoPorId()
        {
            System.Console.Write("\nDigite o ID do plano: ");
            if (int.TryParse(System.Console.ReadLine(), out var id))
            {
                var plano = await _planoRepository!.GetByIdAsync(id);
                if (plano != null)
                {
                    System.Console.WriteLine($"\nID: {plano.PlanoId}");
                    System.Console.WriteLine($"Nome: {plano.Nome}");
                    System.Console.WriteLine($"Descrição: {plano.Descricao ?? "N/A"}");
                    System.Console.WriteLine($"Preço: R$ {plano.Preco:F2}");
                    System.Console.WriteLine($"Duração: {plano.DuracaoMeses} meses");
                    System.Console.WriteLine($"Ativo: {(plano.Ativo ? "Sim" : "Não")}");
                }
                else
                {
                    System.Console.WriteLine("Plano não encontrado.");
                }
            }
            else
            {
                System.Console.WriteLine("ID inválido.");
            }
        }

        static async Task AtualizarPlano()
        {
            System.Console.Write("\nDigite o ID do plano a atualizar: ");
            if (!int.TryParse(System.Console.ReadLine(), out var id))
            {
                System.Console.WriteLine("ID inválido.");
                return;
            }

            var plano = await _planoRepository!.GetByIdAsync(id);
            if (plano == null)
            {
                System.Console.WriteLine("Plano não encontrado.");
                return;
            }

            System.Console.WriteLine($"\nAtualizando plano: {plano.Nome}\n");

            System.Console.Write($"Nome ({plano.Nome}): ");
            var nome = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(nome)) plano.Nome = nome;

            System.Console.Write($"Descrição ({plano.Descricao ?? "N/A"}): ");
            var descricao = System.Console.ReadLine();
            plano.Descricao = string.IsNullOrEmpty(descricao) ? plano.Descricao : descricao;

            System.Console.Write($"Preço (R$ {plano.Preco:F2}): ");
            var precoStr = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(precoStr) && decimal.TryParse(precoStr, out var preco))
                plano.Preco = preco;

            System.Console.Write($"Duração ({plano.DuracaoMeses} meses): ");
            var duracaoStr = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(duracaoStr) && int.TryParse(duracaoStr, out var duracao))
                plano.DuracaoMeses = duracao;

            var sucesso = await _planoRepository!.UpdateAsync(plano);
            if (sucesso)
                System.Console.WriteLine("\nPlano atualizado com sucesso!");
            else
                System.Console.WriteLine("\nErro ao atualizar plano.");
        }

        static async Task ExcluirPlano()
        {
            System.Console.Write("\nDigite o ID do plano a excluir: ");
            if (int.TryParse(System.Console.ReadLine(), out var id))
            {
                var sucesso = await _planoRepository!.DeleteAsync(id);
                if (sucesso)
                    System.Console.WriteLine("Plano excluído com sucesso!");
                else
                    System.Console.WriteLine("Erro ao excluir plano ou plano não encontrado.");
            }
            else
            {
                System.Console.WriteLine("ID inválido.");
            }
        }
        #endregion

        #region Gerenciar Matrículas
        static async Task GerenciarMatriculas()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== GERENCIAR MATRÍCULAS ===\n");
            System.Console.WriteLine("1. Listar Matrículas");
            System.Console.WriteLine("2. Cadastrar Matrícula");
            System.Console.WriteLine("3. Buscar Matrícula por ID");
            System.Console.WriteLine("4. Atualizar Matrícula");
            System.Console.WriteLine("5. Excluir Matrícula");
            System.Console.Write("\nEscolha uma opção: ");

            var opcao = System.Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    await ListarMatriculas();
                    break;
                case "2":
                    await CadastrarMatricula();
                    break;
                case "3":
                    await BuscarMatriculaPorId();
                    break;
                case "4":
                    await AtualizarMatricula();
                    break;
                case "5":
                    await ExcluirMatricula();
                    break;
                default:
                    System.Console.WriteLine("Opção inválida!");
                    break;
            }
        }

        static async Task ListarMatriculas()
        {
            System.Console.WriteLine("\n=== LISTA DE MATRÍCULAS ===\n");
            var matriculas = await _matriculaRepository!.GetAllAsync();

            if (matriculas.Count == 0)
            {
                System.Console.WriteLine("Nenhuma matrícula encontrada.");
                return;
            }

            foreach (var matricula in matriculas)
            {
                System.Console.WriteLine($"ID: {matricula.MatriculaId} | Membro ID: {matricula.MembroId} | Plano ID: {matricula.PlanoId} | " +
                    $"Início: {matricula.DataInicio:dd/MM/yyyy} | Status: {matricula.Status}");
            }
        }

        static async Task CadastrarMatricula()
        {
            System.Console.WriteLine("\n=== CADASTRAR MATRÍCULA ===\n");

            System.Console.Write("ID do Membro: ");
            if (!int.TryParse(System.Console.ReadLine(), out var membroId))
            {
                System.Console.WriteLine("ID do membro inválido.");
                return;
            }

            System.Console.Write("ID do Plano: ");
            if (!int.TryParse(System.Console.ReadLine(), out var planoId))
            {
                System.Console.WriteLine("ID do plano inválido.");
                return;
            }

            System.Console.Write("Data de Início (dd/MM/yyyy) ou Enter para hoje: ");
            var dataInicioStr = System.Console.ReadLine();
            var dataInicio = string.IsNullOrEmpty(dataInicioStr) ? DateTime.Now : DateTime.Parse(dataInicioStr);

            System.Console.Write("Data de Fim (dd/MM/yyyy) ou Enter para pular: ");
            var dataFimStr = System.Console.ReadLine();
            DateTime? dataFim = null;
            if (!string.IsNullOrEmpty(dataFimStr))
            {
                dataFim = DateTime.Parse(dataFimStr);
            }

            System.Console.Write("Valor Pago: ");
            if (!decimal.TryParse(System.Console.ReadLine(), out var valorPago))
            {
                System.Console.WriteLine("Valor inválido.");
                return;
            }

            System.Console.Write("Status (Ativa/Cancelada/Concluída) [Ativa]: ");
            var status = System.Console.ReadLine();
            if (string.IsNullOrEmpty(status)) status = "Ativa";

            var matricula = new Matricula
            {
                MembroId = membroId,
                PlanoId = planoId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                ValorPago = valorPago,
                Status = status
            };

            var id = await _matriculaRepository!.CreateAsync(matricula);
            System.Console.WriteLine($"\nMatrícula cadastrada com sucesso! ID: {id}");
        }

        static async Task BuscarMatriculaPorId()
        {
            System.Console.Write("\nDigite o ID da matrícula: ");
            if (int.TryParse(System.Console.ReadLine(), out var id))
            {
                var matricula = await _matriculaRepository!.GetByIdAsync(id);
                if (matricula != null)
                {
                    System.Console.WriteLine($"\nID: {matricula.MatriculaId}");
                    System.Console.WriteLine($"Membro ID: {matricula.MembroId}");
                    System.Console.WriteLine($"Plano ID: {matricula.PlanoId}");
                    System.Console.WriteLine($"Data Início: {matricula.DataInicio:dd/MM/yyyy}");
                    System.Console.WriteLine($"Data Fim: {matricula.DataFim?.ToString("dd/MM/yyyy") ?? "N/A"}");
                    System.Console.WriteLine($"Valor Pago: R$ {matricula.ValorPago:F2}");
                    System.Console.WriteLine($"Status: {matricula.Status}");
                }
                else
                {
                    System.Console.WriteLine("Matrícula não encontrada.");
                }
            }
            else
            {
                System.Console.WriteLine("ID inválido.");
            }
        }

        static async Task AtualizarMatricula()
        {
            System.Console.Write("\nDigite o ID da matrícula a atualizar: ");
            if (!int.TryParse(System.Console.ReadLine(), out var id))
            {
                System.Console.WriteLine("ID inválido.");
                return;
            }

            var matricula = await _matriculaRepository!.GetByIdAsync(id);
            if (matricula == null)
            {
                System.Console.WriteLine("Matrícula não encontrada.");
                return;
            }

            System.Console.WriteLine($"\nAtualizando matrícula ID: {matricula.MatriculaId}\n");

            System.Console.Write($"Membro ID ({matricula.MembroId}): ");
            var membroIdStr = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(membroIdStr) && int.TryParse(membroIdStr, out var membroId))
                matricula.MembroId = membroId;

            System.Console.Write($"Plano ID ({matricula.PlanoId}): ");
            var planoIdStr = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(planoIdStr) && int.TryParse(planoIdStr, out var planoId))
                matricula.PlanoId = planoId;

            System.Console.Write($"Status ({matricula.Status}): ");
            var status = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(status)) matricula.Status = status;

            var sucesso = await _matriculaRepository!.UpdateAsync(matricula);
            if (sucesso)
                System.Console.WriteLine("\nMatrícula atualizada com sucesso!");
            else
                System.Console.WriteLine("\nErro ao atualizar matrícula.");
        }

        static async Task ExcluirMatricula()
        {
            System.Console.Write("\nDigite o ID da matrícula a excluir: ");
            if (int.TryParse(System.Console.ReadLine(), out var id))
            {
                var sucesso = await _matriculaRepository!.DeleteAsync(id);
                if (sucesso)
                    System.Console.WriteLine("Matrícula excluída com sucesso!");
                else
                    System.Console.WriteLine("Erro ao excluir matrícula ou matrícula não encontrada.");
            }
            else
            {
                System.Console.WriteLine("ID inválido.");
            }
        }
        #endregion

        #region Gerenciar Instrutores
        static async Task GerenciarInstrutores()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== GERENCIAR INSTRUTORES ===\n");
            System.Console.WriteLine("1. Listar Instrutores");
            System.Console.WriteLine("2. Buscar Instrutor por ID");
            System.Console.Write("\nEscolha uma opção: ");

            var opcao = System.Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    await ListarInstrutores();
                    break;
                case "2":
                    await BuscarInstrutorPorId();
                    break;
                default:
                    System.Console.WriteLine("Opção inválida!");
                    break;
            }
        }

        static async Task ListarInstrutores()
        {
            System.Console.WriteLine("\n=== LISTA DE INSTRUTORES ===\n");
            var instrutores = await _instrutorRepository!.GetAllAsync();

            if (instrutores.Count == 0)
            {
                System.Console.WriteLine("Nenhum instrutor encontrado.");
                return;
            }

            foreach (var instrutor in instrutores)
            {
                System.Console.WriteLine($"ID: {instrutor.InstrutorId} | Nome: {instrutor.Nome} | " +
                    $"Especialidade: {instrutor.Especialidade ?? "N/A"} | Email: {instrutor.Email}");
            }
        }

        static async Task BuscarInstrutorPorId()
        {
            System.Console.Write("\nDigite o ID do instrutor: ");
            if (int.TryParse(System.Console.ReadLine(), out var id))
            {
                var instrutor = await _instrutorRepository!.GetByIdAsync(id);
                if (instrutor != null)
                {
                    System.Console.WriteLine($"\nID: {instrutor.InstrutorId}");
                    System.Console.WriteLine($"Nome: {instrutor.Nome}");
                    System.Console.WriteLine($"CPF: {instrutor.CPF}");
                    System.Console.WriteLine($"Email: {instrutor.Email}");
                    System.Console.WriteLine($"Telefone: {instrutor.Telefone}");
                    System.Console.WriteLine($"Especialidade: {instrutor.Especialidade ?? "N/A"}");
                    System.Console.WriteLine($"Data Contratação: {instrutor.DataContratacao:dd/MM/yyyy}");
                    System.Console.WriteLine($"Ativo: {(instrutor.Ativo ? "Sim" : "Não")}");
                }
                else
                {
                    System.Console.WriteLine("Instrutor não encontrado.");
                }
            }
            else
            {
                System.Console.WriteLine("ID inválido.");
            }
        }
        #endregion

        #region Gerenciar Leciona
        static async Task GerenciarLeciona()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== GERENCIAR LECIONA (INSTRUTOR-MEMBRO) ===\n");
            System.Console.WriteLine("1. Listar Todas as Relações");
            System.Console.WriteLine("2. Cadastrar Nova Relação");
            System.Console.WriteLine("3. Buscar Alunos de um Instrutor");
            System.Console.WriteLine("4. Buscar Instrutores de um Membro");
            System.Console.WriteLine("5. Atualizar Relação");
            System.Console.WriteLine("6. Excluir Relação");
            System.Console.Write("\nEscolha uma opção: ");

            var opcao = System.Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    await ListarLeciona();
                    break;
                case "2":
                    await CadastrarLeciona();
                    break;
                case "3":
                    await BuscarAlunosPorInstrutor();
                    break;
                case "4":
                    await BuscarInstrutoresPorMembro();
                    break;
                case "5":
                    await AtualizarLeciona();
                    break;
                case "6":
                    await ExcluirLeciona();
                    break;
                default:
                    System.Console.WriteLine("Opção inválida!");
                    break;
            }
        }

        static async Task ListarLeciona()
        {
            System.Console.WriteLine("\n=== LISTA DE RELAÇÕES LECIONA ===\n");
            var leciona = await _lecionaRepository!.GetAllAsync();

            if (leciona.Count == 0)
            {
                System.Console.WriteLine("Nenhuma relação encontrada.");
                return;
            }

            foreach (var l in leciona)
            {
                System.Console.WriteLine($"Instrutor ID: {l.InstrutorId} | Membro ID: {l.MembroId} | " +
                    $"Início: {l.DataInicio:dd/MM/yyyy} | Fim: {l.DataFim?.ToString("dd/MM/yyyy") ?? "N/A"}");
            }
        }

        static async Task CadastrarLeciona()
        {
            System.Console.WriteLine("\n=== CADASTRAR RELAÇÃO LECIONA ===\n");

            System.Console.Write("ID do Instrutor: ");
            if (!int.TryParse(System.Console.ReadLine(), out var instrutorId))
            {
                System.Console.WriteLine("ID do instrutor inválido.");
                return;
            }

            System.Console.Write("ID do Membro: ");
            if (!int.TryParse(System.Console.ReadLine(), out var membroId))
            {
                System.Console.WriteLine("ID do membro inválido.");
                return;
            }

            System.Console.Write("Data de Início (dd/MM/yyyy) ou Enter para hoje: ");
            var dataInicioStr = System.Console.ReadLine();
            var dataInicio = string.IsNullOrEmpty(dataInicioStr) ? DateTime.Now : DateTime.Parse(dataInicioStr);

            System.Console.Write("Data de Fim (dd/MM/yyyy) ou Enter para pular: ");
            var dataFimStr = System.Console.ReadLine();
            DateTime? dataFim = null;
            if (!string.IsNullOrEmpty(dataFimStr))
            {
                dataFim = DateTime.Parse(dataFimStr);
            }

            System.Console.Write("Observação (opcional): ");
            var observacao = System.Console.ReadLine();

            var leciona = new Leciona
            {
                InstrutorId = instrutorId,
                MembroId = membroId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Observacao = observacao
            };

            var sucesso = await _lecionaRepository!.CreateAsync(leciona);
            if (sucesso)
                System.Console.WriteLine("\nRelação cadastrada com sucesso!");
            else
                System.Console.WriteLine("\nErro ao cadastrar relação.");
        }

        static async Task BuscarAlunosPorInstrutor()
        {
            System.Console.Write("\nDigite o ID do instrutor: ");
            if (int.TryParse(System.Console.ReadLine(), out var instrutorId))
            {
                var leciona = await _lecionaRepository!.GetAlunosPorInstrutorAsync(instrutorId);
                
                if (leciona.Count == 0)
                {
                    System.Console.WriteLine("\nNenhum aluno encontrado para este instrutor.");
                    return;
                }

                System.Console.WriteLine($"\n=== ALUNOS DO INSTRUTOR ID {instrutorId} ===\n");
                foreach (var l in leciona)
                {
                    System.Console.WriteLine($"Membro ID: {l.MembroId} | Nome: {l.Membro?.Nome ?? "N/A"} | " +
                        $"Início: {l.DataInicio:dd/MM/yyyy} | Fim: {l.DataFim?.ToString("dd/MM/yyyy") ?? "N/A"}");
                }
            }
            else
            {
                System.Console.WriteLine("ID inválido.");
            }
        }

        static async Task BuscarInstrutoresPorMembro()
        {
            System.Console.Write("\nDigite o ID do membro: ");
            if (int.TryParse(System.Console.ReadLine(), out var membroId))
            {
                var leciona = await _lecionaRepository!.GetInstrutoresPorMembroAsync(membroId);
                
                if (leciona.Count == 0)
                {
                    System.Console.WriteLine("\nNenhum instrutor encontrado para este membro.");
                    return;
                }

                System.Console.WriteLine($"\n=== INSTRUTORES DO MEMBRO ID {membroId} ===\n");
                foreach (var l in leciona)
                {
                    System.Console.WriteLine($"Instrutor ID: {l.InstrutorId} | Nome: {l.Instrutor?.Nome ?? "N/A"} | " +
                        $"Especialidade: {l.Instrutor?.Especialidade ?? "N/A"} | " +
                        $"Início: {l.DataInicio:dd/MM/yyyy} | Fim: {l.DataFim?.ToString("dd/MM/yyyy") ?? "N/A"}");
                }
            }
            else
            {
                System.Console.WriteLine("ID inválido.");
            }
        }

        static async Task AtualizarLeciona()
        {
            System.Console.WriteLine("\n=== ATUALIZAR RELAÇÃO LECIONA ===\n");

            System.Console.Write("ID do Instrutor: ");
            if (!int.TryParse(System.Console.ReadLine(), out var instrutorId))
            {
                System.Console.WriteLine("ID do instrutor inválido.");
                return;
            }

            System.Console.Write("ID do Membro: ");
            if (!int.TryParse(System.Console.ReadLine(), out var membroId))
            {
                System.Console.WriteLine("ID do membro inválido.");
                return;
            }

            System.Console.Write("Data de Início (dd/MM/yyyy): ");
            if (!DateTime.TryParse(System.Console.ReadLine(), out var dataInicio))
            {
                System.Console.WriteLine("Data inválida.");
                return;
            }

            var leciona = await _lecionaRepository!.GetByIdAsync(instrutorId, membroId, dataInicio);
            if (leciona == null)
            {
                System.Console.WriteLine("Relação não encontrada.");
                return;
            }

            System.Console.Write($"Data de Fim ({leciona.DataFim?.ToString("dd/MM/yyyy") ?? "N/A"}): ");
            var dataFimStr = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(dataFimStr))
            {
                leciona.DataFim = DateTime.Parse(dataFimStr);
            }

            System.Console.Write($"Observação ({leciona.Observacao ?? "N/A"}): ");
            var observacao = System.Console.ReadLine();
            leciona.Observacao = string.IsNullOrEmpty(observacao) ? leciona.Observacao : observacao;

            var sucesso = await _lecionaRepository!.UpdateAsync(leciona);
            if (sucesso)
                System.Console.WriteLine("\nRelação atualizada com sucesso!");
            else
                System.Console.WriteLine("\nErro ao atualizar relação.");
        }

        static async Task ExcluirLeciona()
        {
            System.Console.WriteLine("\n=== EXCLUIR RELAÇÃO LECIONA ===\n");

            System.Console.Write("ID do Instrutor: ");
            if (!int.TryParse(System.Console.ReadLine(), out var instrutorId))
            {
                System.Console.WriteLine("ID do instrutor inválido.");
                return;
            }

            System.Console.Write("ID do Membro: ");
            if (!int.TryParse(System.Console.ReadLine(), out var membroId))
            {
                System.Console.WriteLine("ID do membro inválido.");
                return;
            }

            System.Console.Write("Data de Início (dd/MM/yyyy): ");
            if (!DateTime.TryParse(System.Console.ReadLine(), out var dataInicio))
            {
                System.Console.WriteLine("Data inválida.");
                return;
            }

            var sucesso = await _lecionaRepository!.DeleteAsync(instrutorId, membroId, dataInicio);
            if (sucesso)
                System.Console.WriteLine("Relação excluída com sucesso!");
            else
                System.Console.WriteLine("Erro ao excluir relação ou relação não encontrada.");
        }
        #endregion
    }
}

