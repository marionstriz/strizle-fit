# DistFit .NET App

## Scaffolding

### Database
~~~sh
dotnet ef migrations add --project App.DAL.EF --startup-project WebApp --context AppDbContext Initial 
dotnet ef migrations remove --project App.DAL.EF --startup-project WebApp --context AppDbContext 
dotnet ef database update --project App.DAL.EF --startup-project WebApp
dotnet ef database drop --project App.DAL.EF --startup-project WebApp
~~~

### Controllers

#### MVC Razor based

~~~sh
dotnet aspnet-codegenerator controller -name ExerciseTypesController -actions -m App.Domain.ExerciseType -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name GoalsController -actions -m App.Domain.Goal -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name UnitsController -actions -m App.Domain.Unit -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
~~~

#### WebApi
~~~sh
dotnet aspnet-codegenerator controller -name ExerciseTypesController -actions -m App.Domain.ExerciseType -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
~~~