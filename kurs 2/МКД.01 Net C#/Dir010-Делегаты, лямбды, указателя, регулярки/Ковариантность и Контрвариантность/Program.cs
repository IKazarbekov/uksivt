EmailMessage EmailMessageBuild(string text) => new EmailMessage(text);
Message MessageBuild(string text) => new Message(text);
void InMessageMethod(Message message) { }

// step 1
MessageBuilder messageBuilder = EmailMessageBuild;
//EmailMessageBuilder emailMessageBuilder = MessageBuild; Error compile

// step 2
InMessageDelegat inMessageDelegat = InMessageMethod;

// step 3

delegate Message MessageBuilder(string text);
delegate EmailMessage EmailMessageBuilder(string text);
delegate void InMessageDelegat(EmailMessage message);
class Message
{
    public string Text { get; }
    public Message(string text) => Text = text;
    public virtual void Print() => Console.WriteLine($"Message: {Text}");
}
class EmailMessage : Message
{
    public EmailMessage(string text) : base(text) { }
    public override void Print() => Console.WriteLine($"Email: {Text}");
}
class SmsMessage : Message
{
    public SmsMessage(string text) : base(text) { }
    public override void Print() => Console.WriteLine($"Sms: {Text}");
}

