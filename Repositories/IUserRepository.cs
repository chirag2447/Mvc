using Mvc.Models;
namespace Mvc.Repositories;

public interface IUserRepository
{
    public void AddUser(UserModel user);
    public bool IsUser(string email);
    public bool Login(UserModel user);
    public bool VerifyPassword(string storedHash, string providedPassword);
    public List<String> GetTasksByUserId();

    public string GetTaskNameById(int id);


}