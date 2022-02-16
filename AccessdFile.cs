namespace SS_API
{
    public class AccessdFile
    {
        public long UnixTime { get; private set; }
        public string Action { get; private set; }

        public AccessdFile(string action)
        {
            UnixTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            Action = action;
        }

        public AccessdFile(string action, long unixTime)
        {
            UnixTime = unixTime;
            Action = action;
        }
    }
}
