using System.Security.Claims;
using AutoMapper;
using BLL.Models.Auth;
using BLL.Models.UserModels;
using BLL.Services;
using Common.Convertors;
using Common.Exceptions.General;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication0.Consts;
using WebApplication0.Extensions;

namespace WebApplication0.Controllers;

public class AuthController : BaseAuthController
{
    private readonly UserService _userService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;

    public AuthController(UserService userService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
    {
        _userService = userService;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (UserAlreadyAuthorized())
            return AlreadyAuthorizedForbid();
        
        try
        {
            if (ModelState.IsValid)
            {
                User user = await _userService.GetUserByCredentials(_mapper.Map<CredentialModel>(request));
                await Authenticate(user);

                return RedirectToTempDataReturnUrl();
            }

            return View(request);
        }
        catch (NotFoundException)
        {
            await GenerateLoginModelStateErrors();
            return View(request);
        }
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string? returnUrl = null)
    {
        if (UserAlreadyAuthorized())
            return RedirectToUrl(returnUrl);
        
        if (returnUrl != null) TempData.SetReturnUrl(returnUrl);
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (UserAlreadyAuthorized())
            return AlreadyAuthorizedForbid();
        
        if (!ModelState.IsValid)
            return View(request);

        var model = _mapper.Map<RegisterModel>(request);

        if (await TryRegister(model))
        {
            return RedirectToTempDataReturnUrl();
        }
        else
        {
            await GenerateRegisterModelStateErrors(model);
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout(string? returnUrl = null)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToUrl(returnUrl);
    }

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

    private async Task<bool> TryRegister(RegisterModel model)
    {
        if (await RegisterModelIsValid(model) == false)
            return false;

        User user = await _userService.RegisterUser(model);

        await Authenticate(user);

        return true;
    }

    private async Task<bool> RegisterModelIsValid(RegisterModel model)
    {
        return await _userService.CheckUserExistByEmail(model.Email) == false &&
               await _userService.CheckUserExistByNick(model.Nick) == false;
    }

    private async Task GenerateRegisterModelStateErrors(RegisterModel model)
    {
        if (await _userService.CheckUserExistByEmail(model.Email))
            ModelState.AddModelError(nameof(DAL.Entities.User.Email), "Пользователь с такой почтой уже существует");
        if (await _userService.CheckUserExistByNick(model.Nick))
            ModelState.AddModelError(nameof(DAL.Entities.User.Nick), "Пользователь с таким ником уже существует");
    }

    private async Task GenerateLoginModelStateErrors()
    {
        ModelState.AddModelError(nameof(DAL.Entities.User.Email), "Некорректные логин и(или) пароль");
    }
}
