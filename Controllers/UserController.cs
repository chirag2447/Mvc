using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mvc.Models;
using Mvc.Repositories;

namespace Mvc.Controllers
{

    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserModel user)
        {
            if (_userRepository.Login(user))
            {
                return RedirectToAction("Index", "Student");
            }
            else
            {
                return Ok("wrong");
            }

        }

        public IActionResult Register()
        {
            ViewBag.msg = null;
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserModel user)
        {
            if (!_userRepository.IsUser(user.c_email))
            {

                _userRepository.AddUser(user);

            }
            else
            {
                ViewBag.msg = "User already exists";
                return View();

            }
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}