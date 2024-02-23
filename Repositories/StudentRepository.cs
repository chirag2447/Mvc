using Mvc.Models;
using Npgsql;

namespace Mvc.Repositories;

public class StudentRepository : IStudentRepository
{
    private NpgsqlConnection conn;
    public StudentRepository(IConfiguration config)
    {
        conn = new NpgsqlConnection(config.GetConnectionString("GroupA"));
    }


    public List<StudentModel> GetStudents()
    {
        try
        {

            List<StudentModel> students = new List<StudentModel>();
            conn.Open();
            //get studentw with course name instead of course id
            string query = "SELECT t_studentmaster.c_id, t_studentmaster.c_name, t_studentmaster.c_dob, t_studentmaster.c_gender, t_studentmaster.c_address, t_studentmaster.c_language, t_studentmaster.c_course, t_studentmaster.c_profile, t_studentmaster.c_phone, t_course.c_name as c_course_name FROM t_studentmaster INNER JOIN t_course ON t_studentmaster.c_course = t_course.c_id order by t_studentmaster.c_id asc";
            // string query = "SELECT * FROM t_studentmaster";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                StudentModel student = new StudentModel();
                student.c_id = reader.GetInt32(0);
                student.c_name = reader.GetString(1);
                student.c_dob = reader.GetDateTime(2);
                student.c_gender = reader.GetString(3);
                student.c_address = reader.GetString(4);
                student.c_language = (string[])reader["c_language"];
                student.c_course = reader.GetInt32(6);
                student.c_profile = reader.GetString(7);
                student.c_phone = reader.GetString(8);
                student.c_course_name = reader.GetString(9);
                students.Add(student);
            }
            return students;


        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
        finally
        {
            conn.Close();
        }
        return null;
    }

    public StudentModel GetStudent(int id)
    {
        try
        {
            conn.Open();
            string query = "SELECT * FROM t_studentmaster WHERE c_id = @id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                StudentModel student = new StudentModel();
                student.c_id = reader.GetInt32(0);
                student.c_name = reader.GetString(1);
                student.c_dob = reader.GetDateTime(2);
                student.c_gender = reader.GetString(3);
                student.c_address = reader.GetString(4);
                student.c_language = (string[])reader["c_language"];
                student.c_course = reader.GetInt32(6);
                student.c_profile = reader.GetString(7);
                student.c_phone = reader.GetString(8);
                return student;
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
        finally
        {
            conn.Close();
        }
        return null;
    }


    public void AddStudent(StudentModel student)
    {
        try
        {
            conn.Open();
            string query = "INSERT INTO t_studentmaster(c_name, c_dob, c_gender, c_address, c_language, c_course, c_profile, c_phone) VALUES(@name, @dob, @gender, @address, @language, @course, @profile, @phone)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", student.c_name);
            cmd.Parameters.AddWithValue("@dob", student.c_dob);
            cmd.Parameters.AddWithValue("@gender", student.c_gender);
            cmd.Parameters.AddWithValue("@address", student.c_address);
            cmd.Parameters.AddWithValue("@language", student.c_language);
            cmd.Parameters.AddWithValue("@course", student.c_course);
            cmd.Parameters.AddWithValue("@profile", student.c_profile);
            cmd.Parameters.AddWithValue("@phone", student.c_phone);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
        finally
        {
            conn.Close();
        }

    }




    public void UpdateStudent(StudentModel student)
    {
        try
        {
            conn.Open();
            string query = "UPDATE t_studentmaster SET c_name = @name, c_dob = @dob, c_gender = @gender, c_address = @address, c_language = @language, c_course = @course, c_profile = @profile, c_phone = @phone WHERE c_id = @id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", student.c_name);
            cmd.Parameters.AddWithValue("@dob", student.c_dob);
            cmd.Parameters.AddWithValue("@gender", student.c_gender);
            cmd.Parameters.AddWithValue("@address", student.c_address);
            cmd.Parameters.AddWithValue("@language", student.c_language);
            cmd.Parameters.AddWithValue("@course", student.c_course);
            cmd.Parameters.AddWithValue("@profile", student.c_profile);
            cmd.Parameters.AddWithValue("@phone", student.c_phone);
            cmd.Parameters.AddWithValue("@id", student.c_id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
        finally
        {
            conn.Close();
        }

    }

    public void DeleteStudent(int id)
    {
        try
        {
            conn.Open();
            string query = "DELETE FROM t_studentmaster WHERE c_id = @id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }


    public List<CourseModel> GetCourses()
    {
        try
        {
            List<CourseModel> courses = new List<CourseModel>();
            conn.Open();
            string query = "SELECT * FROM t_course";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CourseModel course = new CourseModel();
                course.c_id = reader.GetInt32(0);
                course.c_name = reader.GetString(1);
                courses.Add(course);
            }
            return courses;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
        finally
        {
            conn.Close();
        }
        return null;
    }




}