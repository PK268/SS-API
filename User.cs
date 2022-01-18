namespace SS_API
{
    public class User
    {
        public string Username;
        public string Password;
        public string PFPLocation;

        public User(string username, string password, string pfpLocation)
        {
            Username = username;
            Password = password;
            PFPLocation = pfpLocation;
        }
    }
}
