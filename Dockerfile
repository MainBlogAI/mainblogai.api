
# Imagem para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MainBlogDb/MainBlogDb.csproj", "MainBlogDb/"]
RUN dotnet restore "MainBlogDb/MainBlogDb.csproj"
COPY . .
WORKDIR "/src/MainBlogDb"
RUN dotnet build "MainBlogDb.csproj" -c Release -o /app/build


# Publicação da aplicação
FROM build AS publish
RUN dotnet publish "MainBlogDb.csproj" -c Release -o /app/publish

# Imagem final para rodar a aplicação
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiCatalogo.dll"]