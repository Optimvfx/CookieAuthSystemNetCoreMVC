using BLL.Models.Tasks.Request;
using BLL.Models.Tasks.View;
using DAL.Entities;

namespace BLL.AutoMapper.Profiles;

public class TasksProfile : AutoMapperProfile
{
    public TasksProfile() : base()
    {
        CreateMap<TaskEntity, TaskViewModel>();
        CreateMap<TaskCreateRequest, TaskEntity>();
    }
}