using AutoMapper;
using BLL.Models.Tasks.Request;
using BLL.Models.Tasks.View;
using BLL.Services;
using Common.Exceptions.User;
using Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerSystemNetCoreMVC.Controllers;

[Authorize]
[Route("Task")]
public class TaskController : BaseAuthController
{
    private readonly TaskService _taskService;
    private readonly IMapper _mapper;

    public TaskController(TaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    #region Pages

    [HttpGet]
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        return View(_taskService.GetAllTasks(GetUserId().Value).Select(t => _mapper.Map<TaskViewModel>(t)));
    }

    [HttpGet]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        if (await _taskService.CheckTaskExist(id, GetUserId().Value) == false)
            return DefaultRedirectUrl();

        var task = await _taskService.GetTaskById(id, GetUserId().Value);
        
        return View("Get",_mapper.Map<TaskViewModel>(task));
    }

    [HttpGet]
    [HttpGet("{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (await _taskService.CheckTaskExist(id, GetUserId().Value) == false)
            return DefaultRedirectUrl();

        var task = await _taskService.GetTaskById(id, GetUserId().Value);
        
        return View("Edit", _mapper.Map<TaskViewModel>(task));
    }

    #endregion

    #region Logic

    [HttpPost]
    public async Task<IActionResult> Create(TaskCreateRequest request, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(request);
        
        var newTaskId = await _taskService.AddTask(request, GetUserId().Value);

        if (returnUrl != null)
            return RedirectToUrl(returnUrl);
        
        return await Get(newTaskId);
    }

    [HttpPut]
    public async Task<IActionResult> Update(TaskEditRequest request, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(request);

        var requestErrors = await GenerateEditTaskModelStateErrors(request);

        if (requestErrors.HaveAnyError)
        {
            ModelState.AddModelErrors(requestErrors);
            return View(request);
        }
        
        await _taskService.UpdateTask(GetUserId().Value, request);
        
        return RedirectToUrl(returnUrl);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id, string? returnUrl = null)
    {
        if (await _taskService.CheckTaskExist(id, GetUserId().Value) == false)
            return RedirectToUrl(returnUrl);

        await _taskService.RemoveTask(id, GetUserId().Value);
        
        return RedirectToUrl(returnUrl);
    }

    #endregion
    
    private async Task<ModelStateErrorsCollection> GenerateEditTaskModelStateErrors(TaskEditRequest request)
    {
        var errors = new ModelStateErrorsCollection();
        
        if (await _taskService.CheckTaskExist(request.EditingId, GetUserId().Value) == false)
            errors.Add(nameof(TaskEditRequest.Title), "Некоректно указан id доступа.");

        return errors;
    }
}