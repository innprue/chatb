namespace WhatsappNet.Api.Models.WhatsappCloud
{
    public class WhatsAppCloudModel
    {
        public List<Entry> Entry { get; set; } = new();
    }

    public class Entry
    {
        public List<Change>? Changes { get; set; }
    }

    public class Change
    {
        public string? Field { get; set; }
        public Value? Value { get; set; }
    }

    public class Value
    {
        public string? Messaging_Product { get; set; }

        public Metadata? Metadata { get; set; }

        public List<Contact>? Contacts { get; set; }

        public List<Message>? Messages { get; set; }
    }

    public class Metadata
    {
        public string? Display_Phone_Number { get; set; }
        public string? Phone_Number_Id { get; set; }
    }

    public class Contact
    {
        public Profile? Profile { get; set; }
        public string? Wa_Id { get; set; }
    }

    public class Profile
    {
        public string? Name { get; set; }
    }

    public class Message
    {
        public string? From { get; set; }
        public string? Id { get; set; }
        public string? Timestamp { get; set; }
        public string? Type { get; set; }
        public Text? Text { get; set; }
        public Interactive? Interactive { get; set; }
    }

    public class Text
    {
        public string? Body { get; set; }
    }

    public class Interactive
    {
        public string? Type { get; set; }
        public ListReply? List_Reply { get; set; }
        public ButtonReply? Button_Reply { get; set; }
    }

    public class ListReply
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }

    public class ButtonReply
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
    }
}
