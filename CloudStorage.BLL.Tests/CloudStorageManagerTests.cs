using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Options;
using CloudStorage.BLL.Services;
using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.DAL;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace CloudStorage.BLL.Tests;

public class CloudStorageManagerTests
{
    private readonly ICloudStorageManager _cloudStorageManager;
    private readonly Mock<ICloudStorageUnitOfWork> _mockCloudStorageUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IFileStorageService> _mockFileStorageService;
    private readonly Mock<IDataHasher> _mockDataHasher;
    private readonly Mock<IUserService> _mockUserService;

    public CloudStorageManagerTests()
    {
        _cloudStorageManager = new CloudStorageManager(_mockCloudStorageUnitOfWork.Object, _mockMapper.Object,
            _mockUserService.Object, _mockFileStorageService.Object, _mockDataHasher.Object,
            new OptionsWrapper<ArchiveOptions>(new ArchiveOptions
                { CodePagesRequired = true, EntryNameEncoding = "enc" }));
    }

    [Fact]
    public async Task Create_Should_Return_Correct_Data()
    {
        // Arrange
        var createData = new List<FileCreateData>
        {
            new() { Content = await GetTestStream(), ContentType = "text/plain", Name = "First.txt" },
            new() { Content = await GetTestStream(), ContentType = "video/mp4", Name = "Second.mp4" }
        };

        _mockDataHasher.Setup(dh => dh.HashStreamData(It.IsAny<Stream>())).Returns("hash");
        _mockUserService.Setup(us => us.Current).Returns(new User { Name = "user" });

        // Act
        var response = await _cloudStorageManager.CreateAsync(createData);

        // Assert
        var expectedResponse = new List<FileDescription>
        {
            new()
            {
                ContentType = "text/plain",
                CreatedDate = DateTime.UtcNow,
                Extension = "txt",
                ProvidedName = "First",
                UploadedBy = "user"
            },
            new()
            {
                ContentType = "video/mp4",
                CreatedDate = DateTime.UtcNow,
                Extension = "mp4",
                ProvidedName = "Second",
                UploadedBy = "user"
            }
        };

        response.Should().BeEquivalentTo(expectedResponse);
    }

    private static async Task<Stream> GetTestStream()
    {
        var ms = new MemoryStream();
        await ms.WriteAsync(new byte[] { 1, 2, 3, 4, 5 });

        return ms;
    }
}