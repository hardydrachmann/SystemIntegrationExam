using System;
namespace Messages
{
    public class StatusRequestMessage : BaseMessage
    {
        public StatusRequestMessage(string sender, string payload) : base(sender, payload) { }
    }

    public class StatusResponseMessage : BaseMessage
    {
        public StatusResponseMessage(string sender, string payload) : base(sender, payload) { }
    }
}
