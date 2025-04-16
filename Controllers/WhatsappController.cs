using Microsoft.AspNetCore.Mvc;
using WhatsappNet.Api.Models.WhatsappCloud;
using WhatsappNet.Api.Services.OpenAI.ChatGPT;
using WhatsappNet.Api.Services.WhatsappCloud.SendMessage;
using WhatsappNet.Api.Util;

namespace WhatsappNet.Api.Controllers
{
    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsappController : Controller
    {
        private readonly IWhatsappCloudSendMessage _whatsappCloudSendMessage;
        private readonly IUtil _util;
        public WhatsappController(IWhatsappCloudSendMessage whatsappCloudSendMessage, IUtil util)
        {
            _whatsappCloudSendMessage = whatsappCloudSendMessage;
            _util = util;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Sample()
        {
            var data = new
            {
                messaging_product = "whatsapp",
                to = "527711512740",
                type = "text",
                text = new
                {
                    body = "este es un mensaje de prueba"
                }
            };

            var result = await _whatsappCloudSendMessage.Execute(data);


            return Ok("ok sample");
        }

        [HttpGet]
        public IActionResult VerifyToken()
        {
            string AccessToken = "50DD88RRREREREREFDVCN38DU3JJODJLDHWEBWHATSAPP";

            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();

            if(challenge != null && token != null && token == AccessToken)
            {
                return Ok(challenge);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ReceivedMessage([FromBody] WhatsAppCloudModel body)
        {
            Console.WriteLine("userNumber, userText");

            try
            {
                var Message = body.Entry[0]?.Changes[0]?.Value?.Messages?[0];

                if (Message != null)
                {
                    var userNumber = Message.From;
                    var userText = GetUserText(Message);

                    // Asegúrate de que se imprima correctamente el texto
                    Console.WriteLine($"Mensaje de {userNumber}: {userText}");

                    // Obtener el nombre del contacto, si es null o "_", asignar "Desconocido"
                    var contactName = body.Entry[0]?.Changes[0]?.Value?.Contacts?[0]?.Profile?.Name;

                    // Si el contactName es igual a "_", asignar "Desconocido"
                    if (contactName == "_")
                    {
                        contactName = "Desconocido";
                    }
                    // Si contactName es null, también asignar "Desconocido"
                    else if (string.IsNullOrEmpty(contactName))
                    {
                        contactName = "Desconocido";
                    }
                    Console.WriteLine($"Mensaje de {contactName} ({userNumber}): {userText}");

                    // Procesar el mensaje recibido
                    object ObjectMessage;
                    switch (userText.ToUpper())
                    {
                        case "TEXT":
                            ObjectMessage = _util.TextMessage("Hola, ¿cómo te puedo ayudar? 😃", userNumber);
                            break;
                        case "IMAGE":
                            ObjectMessage = _util.ImageMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/image_whatsapp.png", userNumber);
                            break;
                        default:
                            ObjectMessage = _util.TextMessage("Hola, ¿no se entendió? 😃", userNumber);
                            break;
                    }

                    //await _whatsappCloudSendMessage.Execute(ObjectMessage);
                    try
                    {
                        await _whatsappCloudSendMessage.Execute(ObjectMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al enviar mensaje: " + ex.Message);
                    }

                }

                return Ok("EVENT_RECEIVED");
            }
            catch (Exception ex)
            {
                // Loggear el error para poder rastrear lo que sucedió
                Console.WriteLine($"Error: {ex.Message}");
                return Ok("EVENT_RECEIVED");
            }
        }

        private string GetUserText(Message message)
        {
            string TypeMessage = message.Type;

            if (TypeMessage.ToUpper() == "TEXT")
            {
                return message.Text?.Body ?? string.Empty;  // Asegúrate de que Body no sea nulo
            }
            else if (TypeMessage.ToUpper() == "INTERACTIVE")
            {
                string interactiveType = message.Interactive?.Type;

                if (interactiveType != null)
                {
                    if (interactiveType.ToUpper() == "LIST_REPLY")
                    {
                        return message.Interactive.List_Reply?.Title ?? "Respuesta de lista no válida";
                    }
                    else if (interactiveType.ToUpper() == "BUTTON_REPLY")
                    {
                        return message.Interactive.Button_Reply?.Title ?? "Respuesta de botón no válida";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }

                return "Interactividad no válida";  // Si Interactive no está presente
            }
            else
            {
                return string.Empty;  // Si el tipo de mensaje no es reconocido
            }
        }
    }
}
