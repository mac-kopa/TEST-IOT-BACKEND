using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace WeatherMeasurement.Repositories
{
    public class ContainerReferenceBuilder
    {
        private string connectionString;
        private string containerName;

        public ContainerReferenceBuilder WithConnectionString(string value)
        {
            connectionString = value;
            return this;
        }

        public ContainerReferenceBuilder WithContainerName(string value)
        {
            containerName = value;
            return this;
        }

        public virtual CloudBlobContainer Build() 
        {
            return CloudStorageAccount
                .Parse(connectionString)
                .CreateCloudBlobClient()
                .GetContainerReference(containerName);
        }
    }
}
