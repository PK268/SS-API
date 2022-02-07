namespace SS_API
{
    public class AccessdFile
    {
        public long UnixTime { get; private set; }
        public string action { get; private set; }

        public AccessdFile(string action)
        {
            UnixTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            this.action = action;
        }
    }
}
