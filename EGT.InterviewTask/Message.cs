using System;

namespace EGT.Api
{
    public class Message
    {
        public Guid MessageId { get; }

        public string MessageData { get; }

        public DateTime MessageDate { get; }

        public MessageStatus MessageStatus { get; set; }

        public Message(string messageData)
        {
            MessageId = Guid.NewGuid();
            MessageDate = DateTime.Now;
            MessageData = messageData ?? throw new ArgumentNullException(nameof(messageData));
        }
    }
}
