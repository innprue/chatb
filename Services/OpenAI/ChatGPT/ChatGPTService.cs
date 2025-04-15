using OpenAI_API.Completions;
using OpenAI_API;

namespace WhatsappNet.Api.Services.OpenAI.ChatGPT
{
    public class ChatGPTService: IChatGPTService
    {
        public async Task<string> Execute(string textUser)
        {
            try
            {
                string apiKey = "YOUR_API_KEY";
                var openAiService = new OpenAIAPI(apiKey);
                var completion = new CompletionRequest
                {
                    Prompt = textUser,
                    Model = "text-davinci-003",
                    NumChoicesPerPrompt = 1,
                    MaxTokens = 200
                };
                var result = await openAiService.Completions.CreateCompletionAsync(completion);

                if (result != null && result.Completions.Count > 0)
                    return result.Completions[0].Text;

                return "Lo siento, sucedió un problema, inténtalo más tarde.";
            }
            catch (Exception e)
            {
                return "Lo siento, sucedió un problema, inténtalo más tarde.";
            }
        }
    }
}
