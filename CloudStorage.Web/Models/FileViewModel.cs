﻿namespace CloudStorage.Web.Models;

public class FileViewModel
{
    public int Id { get; set; }

    public string ProvidedName { get; set; }

    public byte[] Content { get; set; }

    public string UploadedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int SizeInBytes { get; set; }

    public string ContentType { get; set; }

    public byte[] Preview { get; set; }

    public FileFolderViewModel Folder { get; set; }
}