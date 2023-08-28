using AutoMapper;
using BLL.Models.Tasks.Request;
using BLL.Models.Tasks.View;
using Common.Exceptions.User;
using DAL;
using DAL.Entities;
using DAL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class TaskService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly UserService _userService;

    public TaskService(ApplicationDbContext db, IMapper mapper, UserService userService)
    {
        _db = db;
        _mapper = mapper;
        _userService = userService;
    }

    public IEnumerable<TaskEntity> GetAllTasks(Guid userId)
    {
        if (_userService.CheckUserExistById(userId).Result == false)
            throw new UserNotFoundException();

        var user = _userService.GetUsersById(userId)
            .Include(u => u.Tasks)
            .First();

        return user.Tasks;
    }

    public async Task<TaskEntity> GetTaskById(Guid id, Guid userId)
    {
        var task = await GetOrDefaultTaskById(id, userId);
        if (task == null) throw new TaskModelNotFoundException();
        return task;
    }
    
    public async Task<Guid> AddTask(TaskCreateRequest request, Guid userId)
    {
        if (_userService.CheckUserExistById(userId).Result == false)
            throw new InvalidUserIdException();

        var model = _mapper.Map<TaskEntity>(request);
        model.AuthorId = userId;

        var user = _userService.GetUsersById(userId)
            .Include(u => u.Tasks)
            .First();

        user.Tasks.Add(model);
        await _db.SaveChangesAsync();

        return model.Id;
    }

    public async Task<bool> CheckTaskExist(Guid id, Guid userId)
    {
        return await GetOrDefaultTaskById(id, userId) != null;
    }
    
    public async Task UpdateTask(Guid userId, TaskEditRequest request)
    {
        var task = await GetOrDefaultTaskById(request.EditingId, userId);
        if (task == null) throw new TaskModelNotFoundException();

        task.Title = request.Title ?? task.Title;
        task.Description = request.Description ?? task.Description;
        task.DueDate = request.DueDate ?? task.DueDate;

        await _db.SaveChangesAsync();
    }

    public async Task RemoveTask(Guid id, Guid userId)
    {
        var task = await GetOrDefaultTaskById(id, userId);
        if (task == null) throw new TaskModelNotFoundException();

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
    }
    
    private async Task<TaskEntity> GetOrDefaultTaskById(Guid id, Guid userId)
    {
        return await _db.Tasks.GetFirstOrDefaultAsync(id, userId);
    }
}
