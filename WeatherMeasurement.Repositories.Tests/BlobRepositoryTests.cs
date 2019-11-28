using Microsoft.Azure.Storage.Blob;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace WeatherMeasurement.Repositories.Tests
{
    public class Tests
    {
        [Test]
        public async Task GetFileStreamAsync_DoesNotCallHistory_IfFilePresent()
        {
            //Arrange
            var path = "path";
            var fileName = "fileName";
            var containerReferenceBuilderSubstitute = new Mock<ContainerReferenceBuilder>();
            var cloudBlobContainerSubstitute = new Mock<CloudBlobContainer>(new Uri("http://mytest"));
            var cloudBlobSubstitute = new Mock<CloudBlob>(new Uri("http://mytest/blob"));

            containerReferenceBuilderSubstitute.Setup(x => x.Build()).Returns(cloudBlobContainerSubstitute.Object).Verifiable();
            cloudBlobContainerSubstitute.Setup(x => x.GetBlobReference(path + fileName)).Returns(cloudBlobSubstitute.Object).Verifiable();
            cloudBlobSubstitute.Setup(x=> x.ExistsAsync()).ReturnsAsync(true).Verifiable();

            var sut = new BlobRepository(containerReferenceBuilderSubstitute.Object);

            //Act
            await sut.GetFileStreamAsync(path, fileName);

            //ASSERT
            cloudBlobContainerSubstitute.Verify(x => x.GetBlobReference(It.IsAny<string>()), Times.Once());
            cloudBlobSubstitute.Verify(x => x.ExistsAsync(), Times.Once());
            cloudBlobSubstitute.Verify(x => x.OpenReadAsync(), Times.Once());
        }
    }
}