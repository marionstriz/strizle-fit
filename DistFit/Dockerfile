FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /app

COPY *.props .
COPY *.sln .

COPY App.BLL/*.csproj ./App.BLL/
COPY App.BLL.DTO/*.csproj ./App.BLL.DTO/
COPY Base.BLL/*.csproj ./Base.BLL/
COPY App.Contracts.BLL/*.csproj ./App.Contracts.BLL/
COPY Base.Contracts.BLL/*.csproj ./Base.Contracts.BLL/

COPY App.Contracts.DAL/*.csproj ./App.Contracts.DAL/
COPY Base.Contracts.DAL/*.csproj ./Base.Contracts.DAL/
COPY App.DAL.DTO/*.csproj ./App.DAL.DTO/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY Base.DAL/*.csproj ./Base.DAL/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/

COPY Base.Contracts.Domain/*.csproj ./Base.Contracts.Domain/
COPY App.Domain/*.csproj ./App.Domain/
COPY Base.Domain/*.csproj ./Base.Domain/

COPY Base.Contracts.Base/*.csproj ./Base.Contracts.Base/
COPY App.Public/*.csproj ./App.Public/
COPY App.Public.DTO/*.csproj ./App.Public.DTO/
COPY Base.Extensions/*.csproj ./Base.Extensions/
COPY App.Resources/*.csproj ./App.Resources/
COPY Base.Resources/*.csproj ./Base.Resources/

COPY WebApp/*.csproj ./WebApp/

RUN dotnet restore

COPY App.BLL/. ./App.BLL/
COPY App.BLL.DTO/. ./App.BLL.DTO/
COPY Base.BLL/. ./Base.BLL/
COPY App.Contracts.BLL/. ./App.Contracts.BLL/
COPY Base.Contracts.BLL/. ./Base.Contracts.BLL/

COPY App.Contracts.DAL/. ./App.Contracts.DAL/
COPY Base.Contracts.DAL/. ./Base.Contracts.DAL/
COPY App.DAL.DTO/. ./App.DAL.DTO/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY Base.DAL/. ./Base.DAL/
COPY Base.DAL.EF/. ./Base.DAL.EF/

COPY Base.Contracts.Domain/. ./Base.Contracts.Domain/
COPY App.Domain/. ./App.Domain/
COPY Base.Domain/. ./Base.Domain/

COPY Base.Contracts.Base/. ./Base.Contracts.Base/
COPY App.Public/. ./App.Public/
COPY App.Public.DTO/. ./App.Public.DTO/
COPY Base.Extensions/. ./Base.Extensions/
COPY App.Resources/. ./App.Resources/
COPY Base.Resources/. ./Base.Resources/

COPY WebApp/. ./WebApp/

WORKDIR /app/WebApp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:latest AS runtime
WORKDIR /app
ENV ConnectionStrings:NpgsqlConnection="Host=distfit-postgres;Port=5432;Username=postgres;Password=postgres;database=DistFit"
COPY --from=build /app/WebApp/out ./
ENTRYPOINT ["dotnet", "WebApp.dll", "--urls", "http://*:80;https://*:443"]