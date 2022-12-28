using System.ComponentModel.DataAnnotations;

namespace CloudStorage.Web.Attributes;

public class FileContentRequiredAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        var formFile = (IFormFile)value;

        return formFile.Length >= 1;
    }
}