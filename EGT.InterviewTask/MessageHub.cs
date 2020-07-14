using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EGT.Api
{
    public class MessageHub : Hub<IMessageClient>
    {
        public const string MANAGEMENT_GROUP = "MANAGEMENT_GROUP";

        private readonly IRepo _repo;

        private string ClientId { get { return Context.ConnectionId; } }
        public MessageHub(IRepo repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void SaveMessage(string message)
        {
            var newMessage = new Message(message);

            _repo.Messages().TryAdd(newMessage.MessageId, newMessage);
        }

        public async Task MessageDelivered(Guid messageId)
        {
            var deliveredMessageKey = $"{messageId}{ClientId}";

            _repo.DeliveredMesagesToClient().Add(deliveredMessageKey);

            var deliveredToClients = _repo.Clients().Where(client => _repo.DeliveredMesagesToClient().Any(x => x.Contains($"{messageId}{client.Key}"))).Count();

            if (deliveredToClients == _repo.Clients().Count())
            {
                await Clients.Groups(MANAGEMENT_GROUP).MessageDeliveredToAllClients(messageId);

                _repo.Messages()[messageId].MessageStatus = MessageStatus.Received;
            }
        }
        public void DeleteClient()
        {
            _repo.Clients().TryRemove(ClientId, out DateTime value);
        }

        public async Task RegisterManagementClient()
        {
            await Groups.AddToGroupAsync(ClientId, MANAGEMENT_GROUP);
        }

        public override async Task OnConnectedAsync()
        {
            _repo.Clients().TryAdd(ClientId, DateTime.Now);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}