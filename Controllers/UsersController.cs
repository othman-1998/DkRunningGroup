using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapp.Interfaces;
using webapp.Repository;
using webapp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _usersRepository.GetAllUsers();
            List<UsersViewModel> result = new List<UsersViewModel>();

            foreach(var user in users)
            {
                var usersViewModel = new UsersViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Milage = user.Milage,
                };
                result.Add(usersViewModel);
            };
            return View(result);
        }

    }
}

