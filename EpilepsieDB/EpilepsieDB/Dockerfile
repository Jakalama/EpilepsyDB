#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5001

# use this to specify on which address the application is listening
ENV ASPNETCORE_URLS=http://+:5001


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EpilepsieDB/EpilepsieDB.csproj", "EpilepsieDB/"]
RUN dotnet restore "EpilepsieDB/EpilepsieDB.csproj"
COPY . .
WORKDIR "/src/EpilepsieDB"
RUN dotnet build "EpilepsieDB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EpilepsieDB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EpilepsieDB.dll"]