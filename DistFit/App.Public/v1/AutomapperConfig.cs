using AutoMapper;

namespace App.Public.v1;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<App.Public.DTO.v1.ExerciseType, App.BLL.DTO.ExerciseType>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Goal, App.BLL.DTO.Goal>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Measurement, App.BLL.DTO.Measurement>().ReverseMap();
        CreateMap<App.Public.DTO.v1.MeasurementType, App.BLL.DTO.MeasurementType>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Performance, App.BLL.DTO.Performance>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Program, App.BLL.DTO.Program>().ReverseMap();
        CreateMap<App.Public.DTO.v1.ProgramSaved, App.BLL.DTO.ProgramSaved>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Session, App.BLL.DTO.Session>().ReverseMap();
        CreateMap<App.Public.DTO.v1.SessionExercise, App.BLL.DTO.SessionExercise>().ReverseMap();
        CreateMap<App.Public.DTO.v1.SetEntry, App.BLL.DTO.SetEntry>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Unit, App.BLL.DTO.Unit>().ReverseMap();
        CreateMap<App.Public.DTO.v1.UserExercise, App.BLL.DTO.UserExercise>().ReverseMap();
        CreateMap<App.Public.DTO.v1.UserSessionExercise, App.BLL.DTO.UserSessionExercise>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Identity.AppUser, App.BLL.DTO.Identity.AppUser>().ReverseMap();
    }
}