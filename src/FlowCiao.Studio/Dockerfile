FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/FlowCiao.Studio/FlowCiao.Studio.csproj", "src/FlowCiao.Studio/"]
RUN dotnet restore "src/FlowCiao.Studio/FlowCiao.Studio.csproj"
COPY . .
WORKDIR "/src/src/FlowCiao.Studio"
RUN dotnet build "FlowCiao.Studio.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlowCiao.Studio.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlowCiao.Studio.dll"]
