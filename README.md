# 🚀 TRIA-2025 | Projeto TriaTag (Gerenciamento de Frotas)

## 👤 Integrantes do Projeto
| Nome do Integrante       | RM       |
|--------------------------|----------|
| Alice Nunes              | 559052   |
| Guilherme Akira          | 556128   |
| Anne Rezendes            | 556779   |

---

## 🎯 Objetivo do Projeto
A partir da dor relatada pela **Mottu**, o principal problema é a **falta de organização na movimentação de motos** dentro do pátio.  

O **Projeto TriaTag** é um sistema que visa **controlar a alocação de motos aos setores**, usando **validações de sistema** para garantir que as motos não sejam direcionadas ao setor errado.

### 🔑 Funcionalidades Chave
- **Validação de Setor:** Impede o cadastro de uma moto em um setor incorreto.  
- **Localização Rápida:** Localiza a moto por placa e aciona o IoT (*buzina/pisca-alerta – simulado na API*).  
- **Controle de Pátio:** Lista todas as motos presentes em uma filial e setor específico.  

---

## 💡 Justificativa Arquitetural (ASP.NET Core)
O projeto utiliza a arquitetura **Multi-Camadas** (Repository e Service) baseada em **Injeção de Dependência (DI)**.

1. **Robustez com Exceções:**  
   Uso de **Exceções de Domínio** (`CampoJaExistenteException`, `ObjetoNaoEncontradoException`) na camada de Serviço, mapeadas para *status codes* HTTP adequados (**400, 404**) nos Controllers.  
   Isso evita erros genéricos (**500**) no cliente.  

2. **Separação Clara:**  
   Os **Controllers** são leves, focados apenas em receber requisições e retornar respostas HTTP.  
   Toda lógica de negócio fica isolada na camada de **Services**.  

---

## ⚙️ Instruções de Execução da API
### Pré-requisitos
1. .NET 8.0 SDK instalado  
2. Ferramenta `dotnet-ef` instalada globalmente  
3. Banco de dados **Oracle** configurado na `ConnectionStrings:DefaultConnection`  

---

## 📌 Rotas Disponíveis
Todas as entidades possuem rotas **GET** para:
- Buscar **todos os registros**  
- Buscar **um registro específico por ID**  

### Rotas adicionais
- `/api/Endereco/cep/{cep}` → busca endereço pelo CEP  
- `/api/Endereco/logradouro/{logradouro}` → lista endereços pelo logradouro  
- `/api/Filial/nome/{nomeFilial}` → busca filial pelo nome  
- `/api/Funcionario/nome/{nomeFuncionario}` → lista funcionários por nome  
- `/api/Funcionario/login` → simula login com e-mail e senha  
- `/api/Funcionario/cargo/{cargo}` → lista funcionários por cargo  
- `/api/Moto/ano/{ano}` → lista motos com ano ≥ informado  
- `/api/Moto/placa/{placa}` → busca moto pela placa  
- `/api/Moto/modelo/{modelo}` → lista motos por modelo  
- `/api/MotoSetor/placa/{placa}` → lista registros da moto pela placa  

---

## ⚙️ Instalação
### 📦 Bibliotecas Instaladas
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Oracle.EntityFrameworkCore
```

### 📌 EF Core CLI
```bash
dotnet tool install --global dotnet-ef
```

### 🔨 Comandos Utilizados para Migration
```bash
Add-Migration InitialCreate
dotnet ef migrations add InitialCreate
```

---

## 🔗 Acesso e Endpoints
- **Swagger UI:** [https://localhost:7143/swagger/index.html](https://localhost:7143/swagger/index.html)  
- **OpenAPI (JSON):** [https://localhost:7143/swagger/v1/swagger.json](https://localhost:7143/swagger/v1/swagger.json)  

⚠️ **Atenção:** Verificar se o **cache do navegador** está limpo, pois causou erros em execuções anteriores.
