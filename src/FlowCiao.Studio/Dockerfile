FROM mcr.microsoft.com/dotnet/sdk:6.0-bookworm-slim AS publish
WORKDIR /src
COPY ["FlowCiao.Studio.csproj", "."]
RUN dotnet restore "FlowCiao.Studio.csproj"
COPY . .
RUN dotnet publish "FlowCiao.Studio.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlowCiao.Studio.dll"]
