namespace SS_API
{
    public class AccessdFile
    {
        public long UnixTime { get; private set; }
        public string Action { get; private set; }

        public AccessdFile(string action, long unixTime = 0)
        {
            if (unixTime == 0)
            {
                UnixTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            }
            else
            {
                UnixTime = unixTime;
            }
            Action = action;
        }
    }
}
