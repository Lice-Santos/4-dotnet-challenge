# 🛠️DESCRIÇÃO DO PROJETO - TRIA #

**TRIATAG**

Posto a dor relatada pela Mottu, o principal problema é a falta de organização na movimentação de motos dentro do próprio pátio. As motos após a triagem, são direcionadas aos setores e muitas delas são perdidas nessa movimentação, já que muitas acabam no setor errado. Por isso, criamos a TriaTag - uma câmera conectada a um sistema (com várias funcionalidades) que lê a placa das motos ao entrarem em um setor. 

**1. Iniciar Triagem:**
Após a moto ser analisada e diagnosticada na triagem, o funcionário seleciona na aplicação para qual setor a moto deve ser colocada. Se por um acaso a mesma for posta no setor errado, o sistema impedirá o cadastro dessa moto em tal setor. 

**2. Localizar moto:**
Dentro da aplicação, o funcionário insere a placa da moto, e a mesma mostra em qual setor ela está. Conecatado com o IOT da moto, o sistema traz a opção de acionar a buzina e o pisca-alerta, para auxiliar o funcionário a encontrar a encontrá-la.

**3. Ver motos no pátio:**
O funcionário insere o código da filial onde ele se encontra, seleciona o setor que deseja ver, e o sistema lista todas as motos que se encontram no momento, naquele setor.


## 📌 ROTAS

Todas as entidades possuem rotas GET para:
- Buscar **todos os registros**
- Buscar **um registro específico por ID**

### Rotas adicionais:
- /api/Endereco/cep/{cep} -> busca um endereço através do cep;
- /api/Endereco/logradouro/{logradouro} -> lista todos os endereços que possuam o logradouro passado;
- /api/Filial/nome/{nomeFilial} -> lista filiais através do nome passado;
- /api/Funcionario/nome/{nomeFuncionario} -> lista todos os funcionários que tenham o nome passado;
- /api/Funcionario/login -> simula um login com email e senha;
- /api/Funcionario/cargo/{cargo} -> lista todos os funcionários com o cargo passado;
- /api/Moto/ano/{ano} -> lista todas as motos que possuem o ano igual ou maior que o ano passado;
- /api/Moto/placa/{placa} -> busca uma moto pela placa;
- /api/Moto/modelo/{modelo} -> lista todas as motos com o modelo passado;
- /api/MotoSetor/placa/{placa} -> lista todos os registros encontrados da respectiva moto da placa passada.

## ⚙️INSTALAÇÃO ##
**Bibliotecas instaladas:**
- dotnet add package Microsoft.EntityFrameworkCore.Design
- dotnet add package Microsoft.EntityFrameworkCore
- dotnet add package Microsoft.EntityFrameworkCore.Tools
- dotnet add package Oracle.EntityFrameworkCore

**EF Core:**
- dotnet tool install --global dotnet-ef

**Comandos Utilizados para Criação do Migration:**
- Add-Migration InitialCreate
- dotnet ef migrations add InitialCreate







