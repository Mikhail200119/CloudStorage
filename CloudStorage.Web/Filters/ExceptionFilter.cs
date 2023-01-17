using System.Net;
using CloudStorage.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CloudStorage.Web.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly IModelMetadataProvider _modelMetadataProvider;
    private readonly IHostEnvironment _hostEnvironment;

    public ExceptionFilter(IModelMetadataProvider modelMetadataProvider, IHostEnvironment hostEnvironment)
    {
        _modelMetadataProvider = modelMetadataProvider;
        _hostEnvironment = hostEnvironment;
    }

    public void OnException(ExceptionContext context)
    {
        context.Result = new ViewResult
        {
            ViewName = "../Error/Error",
            StatusCode = (int)HttpStatusCode.BadRequest,
            ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
            {
                Model = new ErrorViewModel(context.Exception, context.HttpContext, _hostEnvironment.IsDevelopment())
            }
        };
    }
}