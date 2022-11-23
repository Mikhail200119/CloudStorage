using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Services;
using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.DAL;
using CloudStorage.DAL.Entities;
using FluentAssertions;
using Moq;

namespace CloudStorage.BLL.Tests
{
    public class CloudStorageManagerTests
    {
        private readonly ICloudStorageManager _cloudStorageManager;
        private readonly Mock<ICloudStorageUnitOfWork> _mockCloudStorageUnitOfWork;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly Mock<IDataHasher> _mockDataHasher;

        public CloudStorageManagerTests()
        {
            _mockCloudStorageUnitOfWork = new Mock<ICloudStorageUnitOfWork>();
            _mockDataHasher = new Mock<IDataHasher>();
            _mockFileStorageService = new Mock<IFileStorageService>();
            _mockMapper = new Mock<IMapper>();
            _mockUserService = new Mock<IUserService>();

            _cloudStorageManager = new CloudStorageManager(_mockCloudStorageUnitOfWork.Object, _mockMapper.Object, _mockUserService.Object, _mockFileStorageService.Object, _mockDataHasher.Object);
        }

        [Fact]
        public async Task ShouldSuccessfullyCreateNewFile()
        {
            // Arrange
            var fileCreateData = new FileCreateData
            {
                Name = "someName",
                ContentType = "image/png",
                Content = new byte[] { 1, 2, 3, 4, 5 }
            };

            var userEmail = "someEmail";
            var contentHash = "someHash";

            _mockUserService.Setup(service => service.Current.Email).Returns(userEmail);
            _mockDataHasher.Setup(dataHasher => dataHasher.HashData(fileCreateData.Content)).Returns(contentHash);
            _mockMapper.Setup(mapper => mapper.Map<FileCreateData, FileDescriptionDbModel>(fileCreateData))
                .Returns(new FileDescriptionDbModel
                {
                    Id = 0,
                    ContentHash = contentHash,
                    ContentType = fileCreateData.ContentType,
                    CreatedDate = DateTime.UtcNow,
                    ProvidedName = fileCreateData.Name,
                    SizeInBytes = fileCreateData.Content.Length,
                    UniqueName = "someName",
                    UploadedBy = userEmail
                });

            // Act
            var result = await _cloudStorageManager.CreateAsync(fileCreateData);

            // Assert
            var expectedResult = new FileDescription
            {
                SizeInBytes = fileCreateData.Content.Length,
                ContentType = fileCreateData.ContentType,
                CreatedDate = DateTime.UtcNow,
                Preview = new byte[] { 1, 2, 3, 4 },
                ProvidedName = fileCreateData.Name,
                UploadedBy = userEmail,
                ContentHash = contentHash
            };

            result
                .Should()
                .BeEquivalentTo(expectedResult);
        }
    }
}