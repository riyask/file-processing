FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR app
COPY *.sln .
#WORKDIR JobSchedule.API
#COPY /JobSchedule.API/*.csproj app/JobSchedule.API/
#COPY /JobSchedule.Data/*.csproj app/JobSchedule.Data/
#COPY /JobSchedule.External/*.csproj app/JobSchedule.External/
#COPY /JobSchedule.Shared/*.csproj app/JobSchedule.Shared/
#COPY /JobSchedule.External.UnitTest/*.csproj app/JobSchedule.External.UnitTest/
#COPY /JobSchedule.IntegrationTest/*.csproj app/JobSchedule.IntegrationTest/
#RUN dotnet restore "JobSchedule.API/JobSchedule.API.csproj"

COPY . .
RUN dotnet restore
# copy full solution over
COPY . .
RUN dotnet build
FROM build AS unittestrunner
WORKDIR JobSchedule.External.UnitTest
CMD ["dotnet", "test", "--logger:trx"]

# run the unit tests
FROM build AS unittest
WORKDIR JobSchedule.External.UnitTest
RUN dotnet test --logger:trx

RUN dotnet build
FROM build AS integratointestrunner
WORKDIR JobSchedule.IntegrationTest
CMD ["dotnet", "test", "--logger:trx"]

# run the integration tests
FROM build AS integrationtest
WORKDIR JobSchedule.IntegrationTest
RUN dotnet test --logger:trx

# publish the API
FROM build AS publish
WORKDIR /app/JobSchedule.API
RUN dotnet publish -c Release -o out

# run the api
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS runtime
WORKDIR /app/JobSchedule.API
COPY --from=publish /app/TestWebAPI/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "TestWebAPI.dll"]