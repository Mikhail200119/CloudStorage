using System.ComponentModel.DataAnnotations;

namespace CloudStorage.Web.Attributes;

public class FileContentRequiredAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var formFile = (IFormFile)value;

        return formFile.Length >= 1;
    }
}