## NDS - Backend - Todo List

### Objetivo:

Criar uma API Rest usando .NET 7 e EF Core.

- O sistema deverá ter uma página de registro de usuário e login.
- Cada usuário terá suas tasks (e não poderá visualizar as tasks dos demais usuários).

> #### Observação:
>
> O projeto está separado em 3 camadas (API, Application, Domain e Infra.Data),
> porém para essa prática não precisa ser seguido exatamento essa abordagem.

---

#### Requisitos Gerais

1. O backend será uma API Rest.
2. Utilizar .NET 7. [SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
3. Utilizar Migrations com EF Core para criar as tabelas no banco. [Doc EF Core](https://docs.microsoft.com/pt-br/ef/core/)
4. Como usamos MySQL no NDS iremos usar aqui tambem, para isso vamos usar o [Pomelo](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) junto com o EF Core.
5. Para gerar e executar as migrations:
   1. Usando o rider:
      1. Criar migration: Vá em `tools > Entity Framework Core > Add Migration`.
      2. Atualizar banco: Vá em `tools > Entity Framework Core > Update Database`
   2. Package Manager Console (Visual Studio) - Setar o projeto corrente para o projeto de **Infra.Data**
      1. Criar migration: `Add-Migration <migration_nome> -Context "ApplicationDbContext"`
      2. Atualizar banco: `Update-Database -Context "ApplicationDbContext"`
   3. dotnet ef (Executar comandos da raiz do projeto)
      1. Criar migration: `dotnet ef migrations add <migration_nome> -c ApplicationDbContext -s "src\IFCE.Intranet.API" -p "src\IFCE.Intranet.Infra.Data"`
      2. Atualizar banco: `dotnet ef database update -c ApplicationDbContext -s "src\IFCE.Intranet.API" -p "src\IFCE.Intranet.Infra.Data"`
6. Nos .csproj onde tiver `<Nullable>enable</Nullable>` alterar para `<Nullable>disable</Nullable>` para manter compatibilidade com o exemplo.

---

#### Dependencias por Camadas

- API:
  - Microsoft.AspnetCore.Authentication 
  - Microsoft.AspNetCore.Authentication.JwtBearer 
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore 
  - Microsoft.EntityFrameworkCore 
  - Microsoft.EntityFrameworkCore.Design 
  - Pomelo.EntityFrameworkCore.MySql 
  - Swashbuckle.AspNetCore 
  - Swashbuckle.AspNetCore.Annotations
  

- Application:
  - AutoMapper.Extensions.Microsoft.DependencyInjection 
  - Microsoft.AspNetCore.Authentication.JwtBearer 
  - ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher


- Domain:
  - FluentValidation


- Infra.Data:
  - Microsoft.EntityFrameworkCore 
  - Microsoft.EntityFrameworkCore.Design 
  - Pomelo.EntityFrameworkCore.MySql


#### Descrição de Entidades

- Usuario
  - Id
  - Name
  - Email
  - Password
- Task (Assignment)
  - Id
  - Description
  - UserId - Referência a um usuário - obrigatório.
  - AssignmentListId - Referência a uma lista, campo não obrigatório.
  - Concluded - bool
  - ConcludedAt - DataTime
  - Deadline - DateTime
- TodoList (AssignmentList)
  - Id
  - Name
  - UserId - Referência a um usuário - obrigatório.
- Campos opcionais nas entidades
  - CreateAt
  - UpdatedAt

> O `id` foi utilizado o tipo `Guid`, porém pode utilizar o `int` como alternativa.

---

#### Registro do usuário

- [x] Pedir `Name`, `Email`, `Password`
- [x] Realizar confimação do `Password`
- [x] Deve ser verificado se o `Email` já está em uso.
- [x] O `password` deve ser armazenado utilizando algum algoritmo de hash. [Artigo sobre Password Hash](https://www.scottbrady91.com/aspnet-identity/improving-the-aspnet-core-identity-password-hasher) (Opte pelo Argon2)

#### Login

- [x] Pedir `Email` e `Password`
- [x] Verificar se o `Password` informado bate com o `Password` armazenado. [Exemplo](https://gitlab.com/nds-ifce-maracanau/desafios-nds/backend-todo-list/-/blob/main/src/IFCE.TodoList.Application/Services/AuthService.cs#L41)
- [x] Caso o usuário não for encontrado ou a senha for incorreta, retonar um erro genérico, como `Usuário ou senha incorretos`
- [x] Após as verificações, gerar o `JWT`.

#### Cadastrar uma Lista (AssigmentList)

- [x] Solicitar o uma `Name` para lista
- [x] O `UserId` deve ser obtido do `JWT`
- [x] Validar se o nome foi preenchido.
- [x] Verificar se o `UserId` não é inválido. Se utilizar `Guid` verificar se não é um `Guid` vazio.
