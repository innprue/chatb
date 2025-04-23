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

                string jsonBody = JsonConvert.SerializeObject(model);
                Console.WriteLine("==> JSON ENVIADO A WHATSAPP:");
                Console.WriteLine(jsonBody);

                var byteData = Encoding.UTF8.GetBytes(jsonBody);

                using var content = new ByteArrayContent(byteData);

                string endpoint = "https://graph.facebook.com";
                string phoneNumberId = "663318953525967";
                string accessToken = "EAAKmEekJsngBO4eUPZAzQVp87qpFqWQMSWj7Jb5ZANhDIE60a2DN5PsnZBgTbZCj6Hjk2LqeUXceCxiFbwZALYMBZATwZAmkRuk2l0wDqD7iLqPPAufIgiysLK4197g4d4Ax2GwnJjVfovt4crhBKKuHfJ8LZAB31jU7VOMprsaBeB6jQrN5FenplLLoENz2DZCRHvgZDZD";
                string uri = $"{endpoint}/v22.0/{phoneNumberId}/messages";
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.PostAsync(uri, content);

                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine("⬅ RESPUESTA DE WHATSAPP:");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine(responseBody);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(" ERROR AL ENVIAR MENSAJE A WHATSAPP:");
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
