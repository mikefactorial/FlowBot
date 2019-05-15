// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace MikeFactorial.Samples.FlowBot
{
    public class FlowBot : ActivityHandler
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string flowUrl = "https://prod-08.westus.logic.azure.com:443/workflows/c71d9244f1694f81b0b96100b53ff4da/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=ZhVb1CrebU0Q0besdGeossBKp3RwGIgwZK51X-9Djqw";

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string jsonObject = $"{{ \"Message\": \"{turnContext.Activity.Text}\"}}";
            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(flowUrl, content);

            var responseString = await response.Content.ReadAsStringAsync();
            await turnContext.SendActivityAsync(MessageFactory.Text($"Sorry, I'm still learning so I couldn't answer your question. I sent the message to Flow and this is what she said: '{responseString}'"), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Welcome to Echo Bot."), cancellationToken);
                }
            }
        }
    }
}
