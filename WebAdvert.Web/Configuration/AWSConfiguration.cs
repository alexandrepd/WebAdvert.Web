namespace WebAdvert.Web.Configuration
{
    public class AWSConfiguration
    {
        public string BucketName { get; set; }
        public string AwsAccessKeyId { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string Region { get; set; }
        public string UserPoolClientId { get; set; }
        public string UserPoolClientSecret { get; set; }
        public string UserPoolId { get; set; }
    }
}
