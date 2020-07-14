using System;
using System.Threading.Tasks;

namespace EGT.Api
{
    public interface IMessageClient
    {
        Task SendMessageToClient(Message message);

        Task MessageDeliveredToAllClients(Guid messageId);
    }
}
