using System;
using EasyNetQ;

namespace Messages
{
    public class MessagesFactory
    {
        public static IMessage<T> GetMessage<T>(string id, string payload) where T : BaseMessage
        {
            T messageContent = (T)Activator.CreateInstance(typeof(T), new Object[] {id, payload});
            return new Message<T>(messageContent);
        }
    }
}
