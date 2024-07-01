using ImageCollector.API.Controllers;
using ImageCollector.Application.DTOs;
using ImageCollector.Application.Services;
using ImageCollector.Domain.Entities;
using ImageCollector.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ImageCollector.Tests
{
    public class ImageControllerTests
    {
        private readonly Mock<ImageService> _imageServiceMock;
        private readonly ImageController _imageController;

        public ImageControllerTests()
        {
            _imageServiceMock = new Mock<ImageService>(null, null, null, null);
            _imageController = new ImageController(_imageServiceMock.Object);
        }

        [Fact]
        public async Task GetImages_ReturnsPaginatedImages()
        {
            // Arrange
            var location = "TestLocation";
            var images = new List<ImageDto>
            {
                new ImageDto { Location = "TestLocation", ImageUrl = "http://test.com/image1.jpg", Description = "Test Image 1" },
                new ImageDto { Location = "TestLocation", ImageUrl = "http://test.com/image2.jpg", Description = "Test Image 2" }
            };
            _imageServiceMock.Setup(service => service.GetImagesByLocationAsync(location)).ReturnsAsync(images);

            // Act
            var result = await _imageController.GetImages(location, 1, 1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as dynamic;
            Assert.Equal(1, response.data.Count);
            Assert.Equal(2, response.totalPages);
        }

        [Fact]
        public async Task AddImage_CreatesImage()
        {
            // Arrange
            var imageDto = new ImageDto { Location = "TestLocation", ImageUrl = "http://test.com/image1.jpg", Description = "Test Image 1" };
            var image = new ImageDto { Location = "TestLocation", ImageUrl = "http://test.com/image1.jpg", Description = "Test Image 1" };
            _imageServiceMock.Setup(service => service.AddImageAsync(imageDto)).ReturnsAsync(image);

            // Act
            var result = await _imageController.AddImage(imageDto) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GetImages", result.ActionName);
            Assert.Equal(image.Location, result.RouteValues["location"]);
            Assert.Equal(image, result.Value);
        }

        [Fact]
        public async Task AddImage_ThrowsException_WhenImageExists()
        {
            // Arrange
            var imageDto = new ImageDto { Location = "TestLocation", ImageUrl = "http://test.com/image1.jpg", Description = "Test Image 1" };
            _imageServiceMock.Setup(service => service.AddImageAsync(imageDto)).ThrowsAsync(new InvalidOperationException("Image already exists"));

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _imageController.AddImage(imageDto));

            // Assert
            Assert.Equal("Image already exists", exception.Message);
        }
    }
}
