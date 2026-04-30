EmailMessage MethodEmailMessage(string text) => new EmailMessage(text);
void MethodINputMessage(Message message) { };

// step 1
DelegateReturnMessage delegateReturnMessage = MethodEmailMessage;

// step 2
DelegateInputEmailMessage delegateInputEmailMessage = MethodINputMessage;

delegate Message DelegateReturnMessage(string text);
delegate void DelegateInputEmailMessage(EmailMessage email);
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