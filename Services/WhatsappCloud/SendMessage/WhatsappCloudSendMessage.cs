using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace WhatsappNet.Api.Services.WhatsappCloud.SendMessage
{
    public class WhatsappCloudSendMessage : IWhatsappCloudSendMessage
    {
        public async Task<bool> Execute(object model)
        {
            try
            {
                using var client = new HttpClient();

                // Mostrar el objeto que se enviará
                string jsonBody = JsonConvert.SerializeObject(model);
                Console.WriteLine("➡️ JSON ENVIADO A WHATSAPP:");
                Console.WriteLine(jsonBody);

                var byteData = Encoding.UTF8.GetBytes(jsonBody);

                using var content = new ByteArrayContent(byteData);

                string endpoint = "https://graph.facebook.com";
                string phoneNumberId = "598074300060846";
                string accessToken = "EAAe7n1WHxxABOzRUulpFkQn2EB6PXr7EWhZBZBlhvzwqo6hVSqsrfqRQZCj4lcfEXunix89GBu4EMyZAvRZBSaNG5787kklxNYT19UaVfcqSXOFMrTpVVhhU2pBNHQ2hxb3ZCPQyn3P0ahzW8AeI2be4HhEgw5xHLYHZBK7MZA1wve4OylU999DogEzE5iECjQvwzgZDZD";
                string uri = $"{endpoint}/v18.0/{phoneNumberId}/messages";

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.PostAsync(uri, content);

                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine("⬅️ RESPUESTA DE WHATSAPP:");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine(responseBody);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR AL ENVIAR MENSAJE A WHATSAPP:");
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
