# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o csproj e restaura dependências
COPY ["nf_app_v2.csproj", "./"]
RUN dotnet restore "nf_app_v2.csproj"

# Copia o restante e publica
COPY . .
RUN dotnet publish "nf_app_v2.csproj" -c Release -o /app/publish

# Estágio de Execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Porta padrão do Render
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "nf_app_v2.dll"]
