﻿using Microsoft.AspNetCore.Http;

namespace CloudStorage.Common.Extensions;

public static class FormFileExtensions
{
    public static byte[] ToByteArray(this IFormFile file)
    {
        var stream = new MemoryStream();
        file.CopyTo(stream);

        return stream.ToArray();
    }
}