/* * PROJECT: AI Chatbot Integration
 * TASK: Integrating Azure Bot Service with ASP.NET Core
 * TECHNOLOGY: C#, Bot Framework SDK, Azure AI Services
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using System.Threading;
using System.Threading.Tasks;

[Route("api/messages")]
[ApiController]
public class ChatBotController : ControllerBase
{
    private readonly IBotFrameworkHttpAdapter _adapter;
    private readonly IBot _bot;

    public ChatBotController(IBotFrameworkHttpAdapter adapter, IBot bot)
    {
        _adapter = adapter;
        _bot = bot;
    }

    // TASK: Handle incoming messages from the User
    [HttpPost]
    public async Task PostAsync()
    {
        // Delegate the processing of the HTTP POST to the adapter.
        // The adapter will invoke the bot logic.
        await _adapter.ProcessAsync(Request, Response, _bot);
    }
}

// Simple Bot Logic: Echo Bot (Base for AI)
public class AyushAssistantBot : ActivityHandler
{
    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        var replyText = $"AI Response: You said '{turnContext.Activity.Text}'. How can I help you with our E-commerce services?";
        await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
    }

    protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
    {
        var welcomeText = "Hello Aditya! I am your AI Assistant. Ask me anything about your orders.";
        foreach (var member in membersAdded)
        {
            if (member.Id != turnContext.Activity.Recipient.Id)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
            }
        }
    }
}
