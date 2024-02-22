using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Mvc.Models;
using Mvc.Repositories;

namespace Mvc.Controllers
{

    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _studentRepository;
        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository)
        {
            _logger = logger;
            _studentRepository = studentRepository;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }
            var students = _studentRepository.GetStudents();
            return View(students);
        }

        public IActionResult AddStudent()
        {
            var courses = _studentRepository.GetCourses();
            ViewBag.Courses = new SelectList(courses, "c_id", "c_name");
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(StudentModel student)
        {
            if (student.Photo != null)
            {
                student.c_profile = student.Photo.FileName;
                var path = "wwwroot/images/" + student.Photo.FileName;
                using (var stream = System.IO.File.Create(path))
                {
                    student.Photo.CopyTo(stream);
                }

            }

            _studentRepository.AddStudent(student);
            return RedirectToAction("Index");
        }

        public IActionResult EditStudent(int id)
        {
            var student = _studentRepository.GetStudent(id);
            var course = _studentRepository.GetCourses();
            ViewBag.Courses = new SelectList(course, "c_id", "c_name");
            return View(student);
        }

        [HttpPost]
        public IActionResult EditStudent(StudentModel student)
        {
            if (student.Photo != null)
            {
                student.c_profile = student.Photo.FileName;
                var path = "wwwroot/images/" + student.Photo.FileName;
                using (var stream = System.IO.File.Create(path))
                {
                    student.Photo.CopyTo(stream);
                }

            }
            else
            {
                if (student.c_id.HasValue)
                {

                    var old = _studentRepository.GetStudent(student.c_id.Value);
                    student.c_profile = old.c_profile;
                }
            }


            _studentRepository.UpdateStudent(student);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteStudent(int id)
        {
            _studentRepository.DeleteStudent(id);
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}