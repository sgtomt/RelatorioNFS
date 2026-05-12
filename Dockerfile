# Estágio de Build
FROM ://microsoft.com AS build
WORKDIR /src

# Copia e restaura
COPY ["nf_app_v2.csproj", "./"]
RUN dotnet restore "nf_app_v2.csproj"

# Copia tudo e compila
COPY . .
RUN dotnet publish "nf_app_v2.csproj" -c Release -o /app/publish

# Estágio de Execução
FROM ://microsoft.com
WORKDIR /app
COPY --from=build /app/publish .

# Porta padrão para Render/Railway
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "nf_app_v2.dll"]
