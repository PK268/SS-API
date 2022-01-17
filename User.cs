namespace SS_API
{
    public class User
    {
        public string Username;
        public string Password;
        public string Email;

        public User(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
