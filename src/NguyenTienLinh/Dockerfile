FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80


## Copy the HTTPS certificate from the 'certificates' folder into the container
#COPY certificates/aspnetapp.pfx /https/aspnetapp.pfx
#
## Copy HTTPS certificate
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password="YourPassword"
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NguyenTienLinh/NguyenTienLinh.csproj", "NguyenTienLinh/"]
COPY ["BuildingBlock/BuildingBlock.csproj", "BuildingBlock/"]
RUN dotnet restore "NguyenTienLinh/NguyenTienLinh.csproj"
COPY . .
WORKDIR "/src/NguyenTienLinh"
RUN dotnet build "NguyenTienLinh.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "NguyenTienLinh.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","NguyenTienLinh.dll"]