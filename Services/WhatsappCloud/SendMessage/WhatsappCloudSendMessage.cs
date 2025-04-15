using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace WhatsappNet.Api.Services.WhatsappCloud.SendMessage
{
    public class WhatsappCloudSendMessage: IWhatsappCloudSendMessage
    {
        public async Task<bool> Execute(object model)
        {
            var client = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

            using (var content = new ByteArrayContent(byteData))
            {
                string endpoint = "https://graph.facebook.com";
                string phoneNumberId = "598074300060846";
                string accessToken = "EAAe7n1WHxxABOzRUulpFkQn2EB6PXr7EWhZBZBlhvzwqo6hVSqsrfqRQZCj4lcfEXunix89GBu4EMyZAvRZBSaNG5787kklxNYT19UaVfcqSXOFMrTpVVhhU2pBNHQ2hxb3ZCPQyn3P0ahzW8AeI2be4HhEgw5xHLYHZBK7MZA1wve4OylU999DogEzE5iECjQvwzgZDZD";
                string uri = $"{endpoint}/v22.0/{phoneNumberId}/messages";

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
