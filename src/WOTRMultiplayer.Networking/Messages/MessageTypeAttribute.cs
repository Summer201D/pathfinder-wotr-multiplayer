using System;

namespace WOTRMultiplayer.Networking.Messages
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageTypeAttribute : Attribute
    {
        public int Id { get; set; }

        public MessageTypeAttribute(int id)
        {
            Id = id;
        }
    }
}
