using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebApplication0.Extensions;

public static class TempDataExtensions
{
    private static string ReturnUrlKey = "return_url";
    
    public static string? GetReturnUrl(this ITempDataDictionary tempData)
    {
        if(tempData.ContainsKey(ReturnUrlKey))
            return (string)tempData[ReturnUrlKey];

        return null;
    }
    
    public static void SetReturnUrl(this ITempDataDictionary tempData, string value)
    {
        tempData[ReturnUrlKey] = value;
    }
}