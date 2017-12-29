using System;
namespace Messages
{
    public class BaseMessage
    {
        public string Sender { get; set; }
        public string Payload { get; set; }

        public BaseMessage(string id, string payload) {
            this.Sender = id;
            this.Payload = payload;
        }
    }
}
