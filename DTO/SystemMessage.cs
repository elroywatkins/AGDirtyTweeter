namespace Tweeter
{
    public class SystemMessage
    {
        public MessageType MessageType { get; set; }
        public string Message {get;set;}

        public SystemMessage(MessageType messageType)
        {
            MessageType = messageType;
        }

        public SystemMessage(MessageType messageType,string message)
        {
            MessageType = messageType;
            Message = message;
        }
    }


}