using System;
namespace Messages
{
    public class ObjectiveRequestMessage : BaseMessage
    {
        public ObjectiveRequestMessage(string sender, string payload) : base(sender, payload) { }
    }

    public class ObjectiveResponseMessage : BaseMessage
    {
        public ObjectiveResponseMessage(string sender, string payload) : base(sender, payload) { }
    }
}
