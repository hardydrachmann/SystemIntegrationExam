using System;
namespace Messages
{
    public class ActorDeclarationMessage : BaseMessage
    {
        public ActorDeclarationMessage(string sender, string payload) : base(sender, payload) { }
    }
}
