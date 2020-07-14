using System;
using System.Collections.Concurrent;

namespace EGT.Api
{
    public class Repo : IRepo
    {
        private readonly ConcurrentDictionary<string, DateTime> _clientIds = new ConcurrentDictionary<string, DateTime>();

        private readonly ConcurrentDictionary<Guid, Message> _messages = new ConcurrentDictionary<Guid, Message>();

        private readonly BlockingCollection<string> _sentMesagesToClient = new BlockingCollection<string>();

        private readonly BlockingCollection<string> _deliveredMesagesToClient = new BlockingCollection<string>();

        public ConcurrentDictionary<string, DateTime> Clients() { return _clientIds; }
        public ConcurrentDictionary<Guid, Message> Messages() { return _messages; }
        public BlockingCollection<string> SentMesagesToClient() { return _sentMesagesToClient; }
        public BlockingCollection<string> DeliveredMesagesToClient() { return _deliveredMesagesToClient; }
    }
}
