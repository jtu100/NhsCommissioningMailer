namespace Commissioning.Data
{
    public class KeyedData : IKeyedObject<string>
    {
        const int KeyColumnIndex = 0;

        public string Key
        {
            get { return Data[KeyColumnIndex]; }
        }

        public string[] Data { get; set; }
    }
}