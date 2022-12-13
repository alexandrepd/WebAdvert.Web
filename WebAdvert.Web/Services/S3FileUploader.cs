using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace WebAdvert.Web.Services
{
    public class S3FileUploader : IFileUploader
    {
        private readonly IConfiguration _configuration;
        private readonly RegionEndpoint _region = Amazon.RegionEndpoint.USEast1;
        private readonly BasicAWSCredentials credentials;

        public S3FileUploader(IConfiguration configuration)
        {
            _configuration = configuration;
            credentials = new BasicAWSCredentials(_configuration["AWS:AwsAccessKeyId"], _configuration["AWS:AwsSecretAccessKey"]);
        }

        public async Task<bool> UploadFileAsync(string fileName, Stream storageStream)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("The file name must be specified.");

            string bucketName = _configuration.GetValue<string>("BucketName");

            using (var client = new AmazonS3Client(region: _region))
            {
                if (storageStream.Length > 0)
                    if (storageStream.CanSeek)
                        storageStream.Seek(0, SeekOrigin.Begin);

                var request = new PutObjectRequest { AutoCloseStream = true, BucketName = bucketName, InputStream = storageStream, Key = fileName };

                var response = await client.PutObjectAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }

        }
    }
}
