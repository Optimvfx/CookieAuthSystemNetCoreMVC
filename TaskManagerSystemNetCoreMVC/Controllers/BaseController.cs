using System.Net;
using System.Web.Mvc;
using Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace TaskManagerSystemNetCoreMVC.Controllers;

public abstract class BaseController : Controller
{
    public Uri? CustomRedirectUrl { get; set; }
    public string StandartJsonContentType  { get; set; } = "application/json";
    
    protected IActionResult RedirectToUrl(string? returnUrl)
    {
        if (returnUrl == null)
            return DefaultRedirectUrl();
        
        return Redirect(returnUrl);
    }

    protected IActionResult RedirectToTempDataReturnUrl()
    {
        var url = TempData.GetReturnUrl();

        if (url == null)
            return DefaultRedirectUrl();
        
        return Redirect(url);
    }

    protected IActionResult DefaultRedirectUrl()
    {
        if (CustomRedirectUrl == null)
            return RedirectToAction("Index", "Home");

        return Redirect(CustomRedirectUrl.ToString());
    }
}