using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace EGT.Api
{
    public class MessageQueue : IMessageQueue, IDisposable
    {
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IRepo _repo;
        private readonly CancellationTokenSource token = new CancellationTokenSource();

        public MessageQueue(IHubContext<MessageHub> hubContext, IRepo repo)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            Task.Factory.StartNew(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await SendMessagesToAll();
                    await Task.Delay(TimeSpan.FromSeconds(1), token.Token);
                }
            });
        }


        public void Dispose()
        {
            token.Cancel();
        }

        private async Task SendMessagesToAll()
        {
            if (_repo.Messages().IsEmpty)
            {
                return;
            }

            var activeMessages = _repo.Messages().Values.Where(x => x.MessageStatus == MessageStatus.Send);

            foreach (var message in activeMessages)
            {
                foreach (var clientId in _repo.Clients().Keys)
                {
                    if (message.MessageDate < _repo.Clients()[clientId])
                    {
                        continue;
                    }

                    var sentMessageKey = $"{message.MessageId}{clientId}";

                    if (_repo.SentMesagesToClient().Any(x => x.Equals(sentMessageKey)))
                    {
                        continue;
                    }
                    else
                    {
                        await _hubContext.Clients.Client(clientId).SendAsync("SendMessageToClient", message);

                        _repo.SentMesagesToClient().Add(sentMessageKey);
                    }
                }
            }
        }
    }
}
