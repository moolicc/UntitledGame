using System;
using System.Collections.Generic;
using System.Text;

namespace UntitledGame
{
    public struct Message
    {
        public string Type;
        public object Data;

        public T DataAs<T>()
        {
            try
            {
                return (T)Data;
            }
            catch
            {
                throw new InvalidOperationException($"Payload from message type {Type} cannot be converted to type {typeof(T).Name}.");
            }
        }
    }
}
