namespace CloudStorage.Web.Models;

public class ErrorViewModel
{
    public string RequestPath { get; set; }
    public string TraceIdentifier { get; set; }
    public IEnumerable<ExceptionViewModel> ExceptionModels { get; set; }

    public ErrorViewModel(Exception ex, HttpContext httpContext, bool isDevelopmentEnvironment)
    {
        RequestPath = httpContext.Request.Path;
        TraceIdentifier = httpContext.TraceIdentifier;

        List<ExceptionViewModel> exceptions = new();

        while (ex is not null)
        {
            var exceptionViewModel = new ExceptionViewModel(ex);
            if (!isDevelopmentEnvironment)
            {
                exceptionViewModel.StackTrace = ex.StackTrace;
                exceptionViewModel.Data = null;
                exceptionViewModel.Type = ex.GetType().ToString();
            }

            exceptions.Add(exceptionViewModel);
            ex = ex.InnerException;
        }

        ExceptionModels = exceptions;
    }
}