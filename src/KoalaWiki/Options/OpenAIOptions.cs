using KoalaWiki.Core.Extensions;
using KoalaWiki.Extensions;
using Newtonsoft.Json;

namespace KoalaWiki.Options;

public enum OpenAIModelUsage { Chat, Analysis, DeepResearch }

public sealed record OpenAIModelConfig(string ModelId, string Endpoint, string ApiKey, string Provider);

public class OpenAIOptions
{
    /// <summary>
    /// ChatGPT模型
    /// </summary>
    public static string ChatModel { get; set; } = string.Empty;

    /// <summary>
    /// 分析模型
    /// </summary>
    public static string AnalysisModel { get; set; } = string.Empty;

    /// <summary>
    /// ChatGPT API密钥
    /// </summary>
    public static string ChatApiKey { get; set; } = string.Empty;

    /// <summary>
    /// API地址
    /// </summary>
    public static string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// 模型提供商
    /// </summary>
    public static string ModelProvider { get; set; } = string.Empty;

    /// <summary>
    /// Chat模型独立API地址
    /// </summary>
    public static string ChatModelEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Chat模型独立API密钥
    /// </summary>
    public static string ChatModelApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Chat模型独立提供商
    /// </summary>
    public static string ChatModelProvider { get; set; } = string.Empty;

    /// <summary>
    /// 分析模型独立API地址
    /// </summary>
    public static string AnalysisModelEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// 分析模型独立API密钥
    /// </summary>
    public static string AnalysisModelApiKey { get; set; } = string.Empty;

    /// <summary>
    /// 分析模型独立提供商
    /// </summary>
    public static string AnalysisModelProvider { get; set; } = string.Empty;

    /// <summary>
    /// 深度研究模型独立API地址
    /// </summary>
    public static string DeepResearchModelEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// 深度研究模型独立API密钥
    /// </summary>
    public static string DeepResearchModelApiKey { get; set; } = string.Empty;

    /// <summary>
    /// 深度研究模型独立提供商
    /// </summary>
    public static string DeepResearchModelProvider { get; set; } = string.Empty;

    /// <summary>
    /// 最大文件限制
    /// </summary>
    public static int MaxFileLimit { get; set; } = 10;

    /// <summary>
    /// 深度研究模型
    /// </summary>
    public static string DeepResearchModel { get; set; } = string.Empty;

    /// <summary>
    /// 启用Mem0
    /// </summary>
    public static bool EnableMem0 { get; set; } = false;

    public static string Mem0ApiKey { get; set; } = string.Empty;

    public static string Mem0Endpoint { get; set; } = string.Empty;

    public static OpenAIModelConfig ResolveModelConfig(OpenAIModelUsage usage)
    {
        var modelId = usage switch
        {
            OpenAIModelUsage.Chat => ChatModel,
            OpenAIModelUsage.Analysis => AnalysisModel,
            OpenAIModelUsage.DeepResearch => DeepResearchModel,
            _ => ChatModel
        };

        var endpoint = usage switch
        {
            OpenAIModelUsage.Chat => ChatModelEndpoint,
            OpenAIModelUsage.Analysis => AnalysisModelEndpoint,
            OpenAIModelUsage.DeepResearch => DeepResearchModelEndpoint,
            _ => string.Empty
        };
        if (string.IsNullOrWhiteSpace(endpoint)) endpoint = Endpoint;

        var apiKey = usage switch
        {
            OpenAIModelUsage.Chat => ChatModelApiKey,
            OpenAIModelUsage.Analysis => AnalysisModelApiKey,
            OpenAIModelUsage.DeepResearch => DeepResearchModelApiKey,
            _ => string.Empty
        };
        if (string.IsNullOrWhiteSpace(apiKey)) apiKey = ChatApiKey;

        var provider = usage switch
        {
            OpenAIModelUsage.Chat => ChatModelProvider,
            OpenAIModelUsage.Analysis => AnalysisModelProvider,
            OpenAIModelUsage.DeepResearch => DeepResearchModelProvider,
            _ => string.Empty
        };
        if (string.IsNullOrWhiteSpace(provider)) provider = ModelProvider;
        if (string.IsNullOrWhiteSpace(provider)) provider = "OpenAI";

        return new OpenAIModelConfig(modelId, endpoint, apiKey, provider);
    }

    public static OpenAIModelConfig ResolveModelConfigByModelId(string modelId)
    {
        if (string.Equals(modelId, ChatModel, StringComparison.OrdinalIgnoreCase))
            return ResolveModelConfig(OpenAIModelUsage.Chat);
        if (string.Equals(modelId, AnalysisModel, StringComparison.OrdinalIgnoreCase))
            return ResolveModelConfig(OpenAIModelUsage.Analysis);
        if (string.Equals(modelId, DeepResearchModel, StringComparison.OrdinalIgnoreCase))
            return ResolveModelConfig(OpenAIModelUsage.DeepResearch);

        var provider = string.IsNullOrWhiteSpace(ModelProvider) ? "OpenAI" : ModelProvider;
        return new OpenAIModelConfig(modelId, Endpoint, ChatApiKey, provider);
    }

    public static void InitConfig(IConfiguration configuration)
    {
        ChatModel = (configuration.GetValue<string>("CHAT_MODEL") ??
                     configuration.GetValue<string>("ChatModel") ?? string.Empty).GetTrimmedValueOrEmpty();
        AnalysisModel = (configuration.GetValue<string>("ANALYSIS_MODEL") ??
                         configuration.GetValue<string>("AnalysisModel") ?? string.Empty).GetTrimmedValueOrEmpty();
        ChatApiKey = (configuration.GetValue<string>("CHAT_API_KEY") ??
                      configuration.GetValue<string>("ChatApiKey") ?? string.Empty).GetTrimmedValueOrEmpty();
        Endpoint = (configuration.GetValue<string>("ENDPOINT") ??
                    configuration.GetValue<string>("Endpoint") ?? string.Empty).GetTrimmedValueOrEmpty();
        ModelProvider = (configuration.GetValue<string>("MODEL_PROVIDER") ??
                         configuration.GetValue<string>("ModelProvider")).GetTrimmedValueOrEmpty();

        DeepResearchModel = (configuration.GetValue<string>("DEEP_RESEARCH_MODEL") ??
                             configuration.GetValue<string>("DeepResearchModel")).GetTrimmedValueOrEmpty();

        ChatModelEndpoint = (configuration.GetValue<string>("CHAT_MODEL_ENDPOINT") ??
                             configuration.GetValue<string>("ChatModelEndpoint") ?? string.Empty).GetTrimmedValueOrEmpty();
        ChatModelApiKey = (configuration.GetValue<string>("CHAT_MODEL_API_KEY") ??
                           configuration.GetValue<string>("ChatModelApiKey") ?? string.Empty).GetTrimmedValueOrEmpty();
        ChatModelProvider = (configuration.GetValue<string>("CHAT_MODEL_PROVIDER") ??
                             configuration.GetValue<string>("ChatModelProvider") ?? string.Empty).GetTrimmedValueOrEmpty();
        AnalysisModelEndpoint = (configuration.GetValue<string>("ANALYSIS_MODEL_ENDPOINT") ??
                                 configuration.GetValue<string>("AnalysisModelEndpoint") ?? string.Empty).GetTrimmedValueOrEmpty();
        AnalysisModelApiKey = (configuration.GetValue<string>("ANALYSIS_MODEL_API_KEY") ??
                               configuration.GetValue<string>("AnalysisModelApiKey") ?? string.Empty).GetTrimmedValueOrEmpty();
        AnalysisModelProvider = (configuration.GetValue<string>("ANALYSIS_MODEL_PROVIDER") ??
                                 configuration.GetValue<string>("AnalysisModelProvider") ?? string.Empty).GetTrimmedValueOrEmpty();
        DeepResearchModelEndpoint = (configuration.GetValue<string>("DEEP_RESEARCH_MODEL_ENDPOINT") ??
                                     configuration.GetValue<string>("DeepResearchModelEndpoint") ?? string.Empty).GetTrimmedValueOrEmpty();
        DeepResearchModelApiKey = (configuration.GetValue<string>("DEEP_RESEARCH_MODEL_API_KEY") ??
                                   configuration.GetValue<string>("DeepResearchModelApiKey") ?? string.Empty).GetTrimmedValueOrEmpty();
        DeepResearchModelProvider = (configuration.GetValue<string>("DEEP_RESEARCH_MODEL_PROVIDER") ??
                                     configuration.GetValue<string>("DeepResearchModelProvider") ?? string.Empty).GetTrimmedValueOrEmpty();

        MaxFileLimit = configuration.GetValue<int>("MAX_FILE_LIMIT") > 0
            ? configuration.GetValue<int>("MAX_FILE_LIMIT")
            : 10;

        EnableMem0 = configuration.GetValue<bool?>("ENABLE_MEM0") ?? false;

        if (EnableMem0)
        {
            Mem0ApiKey = (configuration.GetValue<string>("MEM0_API_KEY") ??
                          configuration.GetValue<string>("Mem0ApiKey") ?? string.Empty).GetTrimmedValueOrEmpty();

            Mem0Endpoint = (configuration.GetValue<string>("MEM0_ENDPOINT") ??
                            configuration.GetValue<string>("Mem0Endpoint") ?? string.Empty).GetTrimmedValueOrEmpty();

            if (string.IsNullOrEmpty(Mem0Endpoint))
            {
                throw new Exception("Mem0Endpoint is empty or not set");
            }
        }

        if (string.IsNullOrEmpty(ModelProvider))
        {
            ModelProvider = "OpenAI";
        }

        // 检查参数
        if (string.IsNullOrEmpty(ChatModel))
        {
            throw new Exception("ChatModel is empty");
        }

        var chatConfig = ResolveModelConfig(OpenAIModelUsage.Chat);
        if (string.IsNullOrEmpty(chatConfig.ApiKey))
        {
            throw new Exception("ChatApiKey is empty");
        }

        if (string.IsNullOrEmpty(chatConfig.Endpoint))
        {
            throw new Exception("Endpoint is empty");
        }

        if (string.IsNullOrEmpty(DeepResearchModel))
        {
            DeepResearchModel = ChatModel;
        }

        // 如果没设置分析模型则使用默认的
        if (string.IsNullOrEmpty(AnalysisModel))
        {
            AnalysisModel = ChatModel;
        }
    }
}