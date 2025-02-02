namespace WebApp1
{
    internal class BlobServiceClient
    {
        private string connectionString;

        public BlobServiceClient(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}