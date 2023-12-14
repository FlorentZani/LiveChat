namespace LiveChat.Data
{
    public class UserMessage
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReciverId { get; set; }
        public Guid MessageId { get; set; }

    }
}
