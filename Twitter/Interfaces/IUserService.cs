using Twitter.Model;

namespace Twitter.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password, string env);
    }
}
