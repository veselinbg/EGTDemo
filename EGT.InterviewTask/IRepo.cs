using System;
using System.Collections.Concurrent;

namespace EGT.Api
{
    public interface IRepo
    {
        ConcurrentDictionary<string, DateTime> Clients();
        ConcurrentDictionary<Guid, Message> Messages();
        BlockingCollection<string> SentMesagesToClient();
        BlockingCollection<string> DeliveredMesagesToClient();
    }
}
