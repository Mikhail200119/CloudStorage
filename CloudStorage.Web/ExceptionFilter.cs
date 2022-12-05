using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;
using CloudStorage.Web.Models;

namespace CloudStorage.Web;

public class ExceptionFilter : IExceptionFilter
{
    private readonly IModelMetadataProvider _modelMetadataProvider;

    public ExceptionFilter(IModelMetadataProvider modelMetadataProvider)
    {
        _modelMetadataProvider = modelMetadataProvider;
    }

    public void OnException(ExceptionContext context)
    {
        context.Result = new ViewResult
        {
            ViewName = "../Error/Error",
            StatusCode = (int)HttpStatusCode.BadRequest,
            ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
            {
                Model = new ErrorViewModel(context.Exception, context.HttpContext, false)
            }
        };
    }
}