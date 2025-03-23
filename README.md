# API de Produto

Repositório com a resposta para o exercício 2 (Desenvolvimento de API de Produto).

## Estrutura do Projeto

A solution é composta pelos seguintes projetos:

### Produto.Api

Projeto principal que contém a API REST. Responsável por:

- Configuração da aplicação
- Definição dos endpoints
- Middlewares
- Documentação OpenAPI/Swagger
- Injeção de dependências

### Produto.Application

Camada de aplicação que contém:

- Serviços de aplicação
- DTOs (Data Transfer Objects)
- Validações
- Regras de negócio
- Interfaces de serviços

### Produto.Data

Camada de dados que contém:

- Contexto do Entity Framework
- Entidades
- Repositórios
- Configurações de mapeamento
- Migrações

### Produto.Tests

Projeto de testes que contém:

- Testes de integração
- Testes unitários
- Mocks e configurações de teste

## Requisitos

- .NET 9.0.201 ou superior
- SQLServer

## Apply Migrations

### Restore Tools

```bash
dotnet tool restore
```

### Build

```bash
dotnet build
```

### Configure a Api

Atualize o arquivo `appsettings.Development.json` com as informações do banco de dados.

Exemplo:

```json
{
  "ConnectionStrings": {
    "ProdutoContext": "Server=localhost\\MSSQLSERVER01;Database=projetoApi;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Migrate

```bash
dotnet tool run dotnet-ef database update --project src/Produto.Data/Produto.Data.csproj --context ProdutosContext --startup-project src/Produto.Api/Produto.Api.csproj
```

## Execução

Para executar a API, utilize o seguinte comando:

```bash
dotnet run --project src/Produto.Api/Produto.Api.csproj
```

A API estará disponível em `http://localhost:5087`.

## Testes

Para executar os testes do projeto, utilize o comando:

```bash
dotnet test
```

Os testes incluem:

- Testes unitários
- Testes de integração
