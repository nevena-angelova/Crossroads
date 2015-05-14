using DropNet;

namespace Crossroads.Web.Infrastructure.Services
{
    public static class DropboxHandler
    {
        private const string ApiKey = "5clxj2l3uxngqro";

        private const string ApiSecret = "5h2h840wnnxkz8h";

        private const string ApiToken = "lanZWbRTBWAAAAAAAAAAGvSH6kIqthpNsE539C7lzfJ78Etv5fPCSZT0SR8WA0WJ";

        public static void UploadFile(byte[] content, string name)
        {
            var client = new DropNetClient(ApiKey, ApiSecret, ApiToken);

            client.UseSandbox = true;
            client.UploadFile("/", name, content);
        }

        public static byte[] GetBytes(string name)
        {
            var client = new DropNetClient(ApiKey, ApiSecret, ApiToken);
            client.UseSandbox = true;
            return client.GetFile(name);
        }

        public static void Delete(string name)
        {
            var client = new DropNetClient(ApiKey, ApiSecret, ApiToken);
            client.UseSandbox = true;
            client.Delete(name);
        }
    }
}