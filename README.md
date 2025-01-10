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
