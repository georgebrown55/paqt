using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Paqt.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            // Calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;
            var reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){ Title = "Blue", Type=ActionTypes.ImBack, Value="Blue" },
                    new CardAction(){ Title = "Red", Type=ActionTypes.ImBack, Value="Red" },
                    new CardAction(){ Title = "Green", Type=ActionTypes.ImBack, Value="Green" }
                }
            };
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            await connector.Conversations.ReplyToActivityAsync(reply);

            context.Wait(MessageReceivedAsync);
        }
    }
}