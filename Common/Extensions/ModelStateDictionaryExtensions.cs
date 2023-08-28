using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Common.Extensions;

public static class ModelStateDictionaryExtensions
{
    public static void AddModelErrors(this ModelStateDictionary modelState, ModelStateErrorsCollection errorsCollection)
    {
        errorsCollection.AddErrorsToModelState(modelState);
    }
}

public class ModelStateErrorsCollection
{
    private readonly List<ModelStateError> _modelStateErrors = new();
    
    public IEnumerable<ModelStateError> Errors => _modelStateErrors;
    public bool HaveAnyError => _modelStateErrors.Any();
    
    public void Add(string key, string errorMessage)
    {
        Add(new ModelStateError(key, errorMessage));
    }
    
    public void Add(ModelStateError modelStateError)
    {
        _modelStateErrors.Add(modelStateError);
    }

    public void AddErrorsToModelState(ModelStateDictionary modelState)
    {
        foreach (var modelStateError in _modelStateErrors)
        {
            modelState.AddModelError(modelStateError.Key, modelStateError.ErrorMessage);
        }
    }
}

public class ModelStateError
{
    public readonly string Key;
    public readonly string ErrorMessage;

    public ModelStateError(string key, string errorMessage)
    {
        Key = key;
        ErrorMessage = errorMessage;
    }
}