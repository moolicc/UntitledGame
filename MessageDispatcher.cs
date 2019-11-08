using System;
using System.Collections.Generic;
using System.Text;

namespace UntitledGame
{
    public delegate void MessageCallback(Message message);

    class MessageDispatcher
    {
        public void Subscribe(string messageType, MessageCallback callback)
        {
        }

        public void Unsubscribe(string messageType, MessageCallback callback)
        {
        }

        public void Invoke(string messageType, object data)
        {
        }
    }
}
