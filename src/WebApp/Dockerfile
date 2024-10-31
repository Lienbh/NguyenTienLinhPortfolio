FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

## Copy the HTTPS certificate from the 'certificates' folder into the container
#COPY certificates/aspnetapp.pfx /https/aspnetapp.pfx
### Copy HTTPS certificate
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password="YourPassword"
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApp/WebApp.csproj", "WebApp/"]
COPY ["BuildingBlock/BuildingBlock.csproj", "BuildingBlock/"]
RUN dotnet restore "WebApp/WebApp.csproj"
COPY . .
WORKDIR "/src/WebApp"
RUN dotnet build "WebApp.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "WebApp.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet","WebApp.dll"]

