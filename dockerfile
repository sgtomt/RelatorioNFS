# Estágio de Build
FROM ://microsoft.com AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

# Estágio de Execução
FROM ://microsoft.com
WORKDIR /app
COPY --from=build /app/out .

# Configuração para porta dinâmica (necessário para Render/Railway)
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "nf_app_v2.dll"]
