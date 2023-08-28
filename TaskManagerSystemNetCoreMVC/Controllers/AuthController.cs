using System.Security.Claims;
using AutoMapper;
using BLL.Models.Auth;
using BLL.Models.UserModels;
using BLL.Services;
using Common.Exceptions.General;
using Common.Extensions;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerSystemNetCoreMVC.Consts;

namespace TaskManagerSystemNetCoreMVC.Controllers;

public class AuthController : BaseAuthController
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public AuthController(UserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    #region Pages

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string? returnUrl = null)
    {
        if (UserAlreadyAuthorized())
            return RedirectToUrl(returnUrl);
        
        if (returnUrl != null) TempData.SetReturnUrl(returnUrl);
        return View();
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        var id = GetUserId();
        
        if (UserAlreadyAuthorized())
            return RedirectToUrl(returnUrl);
        
        if (returnUrl != null) TempData.SetReturnUrl(returnUrl);
        return View();
    }

    #endregion

    #region Logic

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (UserAlreadyAuthorized())
            return RedirectToTempDataReturnUrl();

        if (!ModelState.IsValid)
            return View(request);

        var modelErrors = await GenerateLoginModelStateErrors(request);

        if (modelErrors.HaveAnyError)
        {
            ModelState.AddModelErrors(modelErrors);
            return View(request);
        }

        User user = await _userService.GetUserByCredentials(_mapper.Map<CredentialModel>(request));
        await Authenticate(user);

        return RedirectToTempDataReturnUrl();
    }
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (UserAlreadyAuthorized())
            return RedirectToTempDataReturnUrl();
        
        if (!ModelState.IsValid)
            return View(request);

        var model = _mapper.Map<RegisterModel>(request);
        var modelErrors = await GenerateRegisterModelStateErrors(model);
        
        if(modelErrors.HaveAnyError)
        {
            ModelState.AddModelErrors(modelErrors);
            return View("Register", request);
        }
        
        if (await Register(model))
        {
            return RedirectToTempDataReturnUrl();
        }
        else
        {
            return View("Register", request);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteAccount(string? returnUrl = null)
    {
        await _userService.DeleteUser(GetUserId().Value);
        return await Logout(returnUrl);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout(string? returnUrl = null)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToUrl(returnUrl);
    }

    #endregion

    private async Task Authenticate(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Nick),
            new Claim(Claims.UserClaim, user.Id.ToString()),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleId.ToString()),
            new Claim(Claims.EmailClaim, user.Email)
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    }

    private async Task<bool> Register(RegisterModel model)
    {
        User user = await _userService.RegisterUser(model);

        await Authenticate(user);

        return true;
    }
    
    private async Task<ModelStateErrorsCollection> GenerateRegisterModelStateErrors(RegisterModel model)
    {
        var errors = new ModelStateErrorsCollection();
        
        if (await _userService.CheckUserExistByEmail(model.Email))
            errors.Add(nameof(DAL.Entities.User.Email), "Пользователь с такой почтой уже существует");
        if (await _userService.CheckUserExistByNick(model.Nick))
            errors.Add(nameof(DAL.Entities.User.Nick), "Пользователь с таким ником уже существует");

        return errors;
    }

    private async Task<ModelStateErrorsCollection> GenerateLoginModelStateErrors(LoginRequest request)
    {
        var errors = new ModelStateErrorsCollection();
        
        if(await _userService.AnyUserByCredentials(_mapper.Map<CredentialModel>(request)))
            ModelState.AddModelError(nameof(DAL.Entities.User.Email), "Некорректные логин и(или) пароль");

        return errors;
    }
}
