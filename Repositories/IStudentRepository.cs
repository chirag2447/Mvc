using Mvc.Models;
namespace Mvc.Repositories;
public interface IStudentRepository
{
    List<StudentModel> GetStudents();
    StudentModel GetStudent(int id);
    void AddStudent(StudentModel student);
    void UpdateStudent(StudentModel student);
    void DeleteStudent(int id);
    List<CourseModel> GetCourses();
}
