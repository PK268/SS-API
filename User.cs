namespace SS_API
{
    public class User
    {
        public string Username;
        public string Password;
        public string PFPLocation;
        public DateTime DateCreated;

        public User(string username, string password, string pfpLocation, DateTime dateCreated)
        {
            Username = username;
            Password = password;
            PFPLocation = pfpLocation;
            DateCreated = dateCreated;
        }
    }
}
