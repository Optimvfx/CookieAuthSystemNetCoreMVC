using Microsoft.AspNetCore.Mvc;
using WebApplication0.Extensions;

namespace WebApplication0.Controllers;

public abstract class BaseController : Controller
{
    public Uri? CustomRedirectUrl;

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