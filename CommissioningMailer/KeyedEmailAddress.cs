namespace CommissioningMailer
{
    public class KeyedEmailAddress : IKeyedObject<string>
    {
        public string Key { get; set; }
        public string EmailAddress { get; set; }
    }

    public interface IKeyedObject<out T>
    {
        T Key { get; }
    }
}