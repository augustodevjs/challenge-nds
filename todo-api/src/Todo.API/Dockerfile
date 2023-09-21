# Defina a imagem base para a construção
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

# Configure o diretório de trabalho dentro do contêiner
WORKDIR /app

# Exponha as portas 80 e 443 do contêiner
EXPOSE 80
EXPOSE 443

# Defina a imagem base para a construção
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Configure o diretório de trabalho dentro do contêiner
WORKDIR /src

# Copie o arquivo de projeto da aplicação para dentro do contêiner
COPY ["src/Todo.API/Todo.API.csproj", "Todo.API"]

# Execute a restauração das dependências da aplicação
RUN dotnet restore "src/Todo.API/Todo.API.csproj"

# Copie todo o conteúdo do diretório de trabalho local para dentro do contêiner
COPY . .

# Configure o diretório de trabalho para a construção da aplicação
WORKDIR "/src/Todo.API"

# Compile a aplicação com configuração Release e coloque os artefatos no diretório /app/build
RUN dotnet build "Todo.API.csproj" -c Release -o /app/build

# Defina uma etapa intermediária chamada 'publish' para publicar a aplicação
FROM build AS publish

# Publique a aplicação com configuração Release e coloque os artefatos no diretório /app/publish
RUN dotnet publish "Todo.API.csproj" -c Release -o /app/publish

# Defina uma etapa final usando a imagem base 'base'
FROM base AS final

# Configure o diretório de trabalho no contêiner final
WORKDIR /app

# Copie os artefatos da etapa de 'publish' para o diretório de trabalho do contêiner final
COPY --from=publish /app/publish .

# Defina o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "Todo.API.dll"]
