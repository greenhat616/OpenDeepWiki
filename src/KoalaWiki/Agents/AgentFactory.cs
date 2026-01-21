using System.ClientModel;
using Azure.AI.OpenAI;
using KoalaWiki.Options;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OpenAI;

namespace KoalaWiki.Agents;

public class AgentFactory
{
    public static ChatClientAgent CreateChatClientAgentAsync(string modelId,
        Action<ChatClientAgentOptions> agentAction,
        ILoggerFactory? loggerFactory = null)
    {
        var config = OpenAIOptions.ResolveModelConfigByModelId(modelId);

        if (config.Provider.Equals("OpenAI", StringComparison.OrdinalIgnoreCase))
        {
            var openAIClient = new OpenAIClient(new ApiKeyCredential(config.ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(config.Endpoint),
            });

            var chatClient = openAIClient.GetChatClient(modelId);

            var agentOptions = new ChatClientAgentOptions
            {
                // ChatMessageStoreFactory = (messageContext) =>
                // {
                //     var logger = loggerFactory?.CreateLogger<AutoContextCompress>();
                //     return new AutoContextCompress(messageContext, chatClient.AsIChatClient(), logger);
                // },
            };
            agentAction.Invoke(agentOptions);

            var agent = chatClient.CreateAIAgent(agentOptions);

            return agent;
        }
        else if (config.Provider.Equals("AzureOpenAI", StringComparison.OrdinalIgnoreCase) ||
                 config.Provider.Equals("Azure", StringComparison.OrdinalIgnoreCase))
        {
            var azureOpenAIClient =
                new AzureOpenAIClient(new Uri(config.Endpoint), new ApiKeyCredential(config.ApiKey));

            var chatClient = azureOpenAIClient.GetChatClient(modelId);

            var agentOptions = new ChatClientAgentOptions();
            // agentOptions.ChatMessageStoreFactory = (messageContext) =>
            // {
            //     var logger = loggerFactory?.CreateLogger<AutoContextCompress>();
            //     return new AutoContextCompress(messageContext, chatClient.AsIChatClient(), logger);
            // };
            agentAction.Invoke(agentOptions);
            var agent = chatClient.CreateAIAgent(agentOptions);

            return agent;
        }
        else
        {
            throw new NotSupportedException($"Model provider '{config.Provider}' is not supported.");
        }
    }
}