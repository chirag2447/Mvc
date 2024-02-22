using Npgsql;
using Mvc.Models;
using Mvc.Repositories;
using Microsoft.AspNetCore.Identity;



namespace Mvc.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _conn;
    private readonly NpgsqlConnection conn;
    private NpgsqlConnection connForTask;
    private readonly IHttpContextAccessor access;
    public UserRepository(IConfiguration config, IHttpContextAccessor accessor)
    {
        _conn = config.GetConnectionString("GroupA");
        conn = new NpgsqlConnection(_conn);
        connForTask = new NpgsqlConnection(_conn);
        access = accessor;
    }

    public void AddUser(UserModel user)
    {
        try
        {

            var hasher = new PasswordHasher<UserModel>();
            user.c_password = hasher.HashPassword(user, user.c_password);
            conn.Open();

            string query = "insert into public.t_usermaster(c_username,c_email,c_password,c_role) values(@u,@e,@p,'user')";
            var command = new NpgsqlCommand(query, conn);
            command.Parameters.AddWithValue("@u", user.c_username);
            command.Parameters.AddWithValue("@e", user.c_email);
            command.Parameters.AddWithValue("@p", user.c_password);

            command.ExecuteNonQuery();


        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            conn.Close();
        }
    }

    public bool IsUser(string email)
    {
        try
        {
            conn.Open();
            string query = "select * from public.t_usermaster where c_email=@email";
            var command = new NpgsqlCommand(query, conn);
            command.Parameters.AddWithValue("@email", email);
            var reader = command.ExecuteReader();

            // Check if there is at least one row in the result set
            if (reader.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            conn.Close();
        }
        return false;
    }

    public bool Login(UserModel user)
    {
        try
        {
            conn.Open();
            string query = "select c_id, c_username, c_email, c_password, c_role from public.t_usermaster where c_email = @email";
            var command = new NpgsqlCommand(query, conn);
            command.Parameters.AddWithValue("@email", user.c_email);
            // command.Parameters.AddWithValue("@password", user.c_password);

            var rows = command.ExecuteReader();
            if (rows.Read())
            {

                if (VerifyPassword(rows["c_password"].ToString(), user.c_password))
                {

                    string username = rows["c_username"].ToString();
                    string role = rows["c_role"].ToString();
                    access.HttpContext.Session.SetInt32("userid", rows.GetInt32(0));
                    access.HttpContext.Session.SetString("userrole", role);
                    access.HttpContext.Session.SetString("useremail", user.c_email);
                    access.HttpContext.Session.SetString("username", username);

                    return true;
                }
                else
                {
                    return false;
                }



            }
            else
            {
                return false;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

        }
        finally
        {
            conn.Close();
        }
        return false;
    }
    public bool VerifyPassword(string storedHash, string providedPassword)
    {
        var passwordHasher = new PasswordHasher<UserModel>();
        var result = passwordHasher.VerifyHashedPassword(null, storedHash, providedPassword);

        return result == PasswordVerificationResult.Success;
    }

    public List<string> GetTasksByUserId()
    {
        List<string> tasks = new List<string>();
        try
        {
            conn.Open();

            // Retrieve user ID from session
            int userId = access.HttpContext.Session.GetInt32("userid") ?? 0;

            // Query to get tasks by user ID
            string query = "SELECT c_task FROM public.t_usertask WHERE c_user = @userId";
            var command = new NpgsqlCommand(query, conn);
            command.Parameters.AddWithValue("@userId", userId);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int taskId = Convert.ToInt32(reader["c_task"]);
                string taskName = GetTaskNameById(taskId); // Use a separate method to get task name
                tasks.Add(taskName);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            conn.Close();
        }
        return tasks;
    }

    public string GetTaskNameById(int id)
    {
        string taskName = "";
        // Create a new connection
        try
        {
            connForTask.Open();

            // Query to get task name by ID
            string query = "SELECT c_task FROM public.t_tasks WHERE c_id = @taskId";
            var command = new NpgsqlCommand(query, connForTask);
            command.Parameters.AddWithValue("@taskId", id);

            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                taskName = reader["c_task"].ToString();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            connForTask.Close();
        }
        return taskName;
    }



}