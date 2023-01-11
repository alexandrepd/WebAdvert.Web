using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using WebAdvert.Web.Configuration;

namespace WebAdvert.Web.Services
{
    public class S3FileUploader : IFileUploader
    {
        private readonly AWS _awsConfiguration;

        public S3FileUploader(AWS awsConfiguration)
        {
            _awsConfiguration = awsConfiguration;
        }

        public async Task<bool> UploadFileAsync(string fileName, Stream storageStream)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("The file name must be specified.");

            string bucketName = _awsConfiguration.BucketName;

            using (var client = new AmazonS3Client(region: RegionEndpoint.GetBySystemName(_awsConfiguration.Region)))
            {
                if (storageStream.Length > 0)
                    if (storageStream.CanSeek)
                        storageStream.Seek(0, SeekOrigin.Begin);

                var request = new PutObjectRequest
                {
                    AutoCloseStream = true,
                    BucketName = bucketName,
                    InputStream = storageStream,
                    Key = fileName,
                    CannedACL = Amazon.S3.S3CannedACL.PublicRead
                };

                var response = await client.PutObjectAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }

        }
    }
}
