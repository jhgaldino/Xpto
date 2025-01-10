#  Sistema de Gerenciamento de Pedidos do Restaurante Xpto

## Descrição
API REST para gerenciamento de pedidos de refeições, com suporte a diferentes tipos de refeição (café da manhã e almoço) e validações de horário.

Banco de Dados Sqlite para facilidade de uso.

Interface de Usuario(Front-end) em Angular.

## Pré-requisitos
- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code
- SQLite
- Node.js e npm (para o frontend Angular)

## Configuração do Backend (XptoAPI)

1.  Clone o repositório:
```bash
git clone https://github.com/jhgaldino/Xpto
cd XptoProject
```
2.  Restaure as dependências do projeto:
```
cd XptoAPI
dotnet restore
```
3. Execute as migrações do banco de dados
```
dotnet ef database update
```
4. Execute a API
```
dotnet run
```

A API estará disponível em:

HTTP: http://localhost:5081

HTTPS: https://localhost:7211

Swagger UI: http://localhost:5081/swagger

## Configuração do Frontend (XptoFrontend)

1. Instale as dependências:
```
cd ../XptoFrontend
npm install
```

2. Execute o frontend:
```
ng serve
```

O frontend estará disponível em http://localhost:4200

### Horários de Funcionamento

Café da Manhã: 06:00 às 10:30

Almoço: 11:30 às 14:30

OBS : Ao fazer pedido na tela, o sistema está usando o horário real do sistema tanto no frontend quanto no backend. Caso esteja fora do horario dos pedidos, ajuste o horário do seu sistema.

## Documentação da API

A documentação completa da API está disponível via Swagger UI em http://localhost:5081/swagger

### Coleção do Postman

Para testes da API, uma coleção do Postman está disponível em XptoAPI-Pedido.postman_collection.json

## Estrutura do Projeto
### Backend (XptoAPI)
src/Controllers/: Controladores da API

src/Models/: Modelos de dados

src/Services/: Lógica de negócios
src/Validators/: Validações

src/Data/: Contexto do banco de dados

### Frontend (XptoFrontend)

src/app/components/: Componentes Angular

src/app/services/: Serviços

src/app/models/: Interfaces e tipos

# Testes

Para executar os testes do backend:

```
cd XptoAPI.Tests
dotnet test
```