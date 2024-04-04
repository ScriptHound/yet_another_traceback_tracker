using Yet_Another_Traceback_Tracker.Models;

namespace Yet_Another_Traceback_Tracker.Services;

public interface IUserService
{
    public void RegisterUser(string username, string password);
    public void DeleteUser(string username);
    public bool AuthenticateUser(string username, string password);
}

public class UserService : IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    public UserService(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    
    public void RegisterUser(string username, string password)
    {
        var hashedPassword = _passwordHasher.HashPassword(password);
        using (var context = new UserContext())
        {
            context.Add(new UserEntity
            {
                Username = username,
                Password = hashedPassword
            });
            context.SaveChanges();
        }
    }

    public bool AuthenticateUser(string username, string password)
    {
        var hashedPassword = _passwordHasher.HashPassword(password);
        using (var context = new UserContext())
        {
            var userEntity = context.Users.Single(u => u.Username == username);
            if (userEntity is null)
                return false;

            var userHashedPassword = userEntity.Password;
            return _passwordHasher.VerifyPassword(password, hashedPassword);
        }
    }

    public void DeleteUser(string username)
    {
        using (var context = new UserContext())
        {
            var userToRemove = new UserEntity
            {
                Username = username
            };

            context.Attach(userToRemove);
            context.Remove(userToRemove);
        }
    }
}