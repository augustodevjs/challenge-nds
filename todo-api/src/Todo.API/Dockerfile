FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Todo.API/Todo.API.csproj", "Todo.API"]
RUN dotnet restore "src/Todo.API/Todo.API.csproj"
COPY . .
WORKDIR "/src/Todo.API"
RUN dotnet build "Todo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Todo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Todo.API.dll"]
