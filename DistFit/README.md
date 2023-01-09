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
dotnet aspnet-codegenerator controller -name ExerciseTypeController -actions -m App.Domain.ExerciseType -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
~~~

#### WebApi
~~~sh
dotnet aspnet-codegenerator controller -name MeasurementController -actions -m App.Domain.Measurement -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
~~~