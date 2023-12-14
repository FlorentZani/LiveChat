namespace LiveChat.Data
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReciverId { get; set; }
        public string Content { get; set; }


    }
}
