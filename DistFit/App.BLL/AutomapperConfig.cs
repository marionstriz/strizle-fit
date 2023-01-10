using AutoMapper;

namespace App.BLL;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<App.BLL.DTO.ExerciseType, App.DAL.DTO.ExerciseType>().ReverseMap();
        CreateMap<App.BLL.DTO.Goal, App.DAL.DTO.Goal>().ReverseMap();
        CreateMap<App.BLL.DTO.Measurement, App.DAL.DTO.Measurement>().ReverseMap();
        CreateMap<App.BLL.DTO.MeasurementType, App.DAL.DTO.MeasurementType>().ReverseMap();
        CreateMap<App.BLL.DTO.Performance, App.DAL.DTO.Performance>().ReverseMap();
        CreateMap<App.BLL.DTO.Program, App.DAL.DTO.Program>().ReverseMap();
        CreateMap<App.BLL.DTO.ProgramSaved, App.DAL.DTO.ProgramSaved>().ReverseMap();
        CreateMap<App.BLL.DTO.Session, App.DAL.DTO.Session>().ReverseMap();
        CreateMap<App.BLL.DTO.SessionExercise, App.DAL.DTO.SessionExercise>().ReverseMap();
        CreateMap<App.BLL.DTO.SetEntry, App.DAL.DTO.SetEntry>().ReverseMap();
        CreateMap<App.BLL.DTO.Unit, App.DAL.DTO.Unit>().ReverseMap();
        CreateMap<App.BLL.DTO.UserExercise, App.DAL.DTO.UserExercise>().ReverseMap();
        CreateMap<App.BLL.DTO.UserSessionExercise, App.DAL.DTO.UserSessionExercise>().ReverseMap();
        CreateMap<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>().ReverseMap();
    }
}