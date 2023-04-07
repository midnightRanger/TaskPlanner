FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TaskPlannerProject/TaskPlannerProject.csproj", "TaskPlannerProject/"]
RUN dotnet restore "TaskPlannerProject/TaskPlannerProject.csproj"
COPY . .
WORKDIR "/src/TaskPlannerProject"
RUN dotnet build "TaskPlannerProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskPlannerProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskPlannerProject.dll"]
