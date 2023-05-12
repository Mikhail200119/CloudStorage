using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using CloudStorage.BLL.Options;
using CloudStorage.BLL.Services;
using CloudStorage.BLL.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CloudStorage.BLL.Tests;

public class FileStorageServiceTests
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IFileSystem _fileSystem;
    
    public FileStorageServiceTests()
    {
        _fileStorageService = new FileStorageService(new OptionsWrapper<FileStorageOptions>(new FileStorageOptions
        {
            FilesDirectoryPath = "path",
            FFmpegExecutablesPath = "path"
        }));

        _fileSystem = new MockFileSystem();
    }

    [Fact]
    public async Task Upload_Files_Should_Not_Throw_Any_Exception()
    {
        // Arrange
        
        // Act
        
        // Assert
    }
}