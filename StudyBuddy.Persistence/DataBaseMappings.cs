using AutoMapper;
using StudyBuddy.Domain.Models;
using StudyBuddy.Persistence.Entities;


namespace StudyBuddy.Persistence;

public class DataBaseMappings : Profile
{
    public DataBaseMappings()
    {
        CreateMap<UserEntity, User>();
    }
}