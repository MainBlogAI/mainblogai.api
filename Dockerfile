
# Imagem para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiCatalogo/ApiCatalogo.csproj", "ApiCatalogo/"]
RUN dotnet restore "ApiCatalogo/ApiCatalogo.csproj"
COPY . .
WORKDIR "/src/ApiCatalogo"
RUN dotnet build "ApiCatalogo.csproj" -c Release -o /app/build


# Publicação da aplicação
FROM build AS publish
RUN dotnet publish "ApiCatalogo.csproj" -c Release -o /app/publish

# Imagem final para rodar a aplicação
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiCatalogo.dll"]