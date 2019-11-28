using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using WeatherMeasurement.Repositories.Interfaces;

namespace WeatherMeasurement.Repositories
{
    public class BlobRepository : IBlobRepository, IDisposable
    {
        private readonly ContainerReferenceBuilder _builder;
        private readonly string connectionString;
        private const string containerName = "iotbackend";
        private const string historicalFileName = "historical.zip";
        private bool disposedValue = false;

        private IList<IDisposable> undisposedObjects = new List<IDisposable>();
        public BlobRepository(ContainerReferenceBuilder builder)
        {
            _builder = builder;
            connectionString = System.Environment.GetEnvironmentVariable("BlobConnectionString", EnvironmentVariableTarget.Process); ;
        }
        public async Task<Stream> GetFileStreamAsync(string path, string fileName)
        {
            var containerReference = _builder
                .WithConnectionString(connectionString)
                .WithContainerName(containerName)
                .Build();

            var fileReference = containerReference.GetBlobReference(path + fileName);

            if (await fileReference.ExistsAsync())
            {
                return await fileReference.OpenReadAsync();
            }

            return await ReadHistorical(containerReference, path, fileName);
        }

        private async Task<Stream> ReadHistorical(CloudBlobContainer containerReference, string path, string fileName)
        {
            var hitoricalFileReference = containerReference.GetBlobReference(path + historicalFileName);

            if (!await hitoricalFileReference.ExistsAsync())
            {
                return null;
            }

            var historicalZipStream = await hitoricalFileReference.OpenReadAsync();
            undisposedObjects.Add(historicalZipStream);

            var zip = new ZipArchive(historicalZipStream);
            undisposedObjects.Add(zip);

            if (zip.Entries.Any(x => x.Name == fileName))
            {
                return zip.Entries.First(x => x.Name == fileName).Open();
            }
            else
            {
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach(var undisposedObject in undisposedObjects)
                    {
                        undisposedObject.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

}

