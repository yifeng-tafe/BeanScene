using BeanScene.Areas.Identity.Data;
using BeanScene.Models;
using BeanScene.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace BeanScene.Controllers

{
    [Authorize(Roles = "Manager")]

    public class UserController : Controller

    {

        private UserManager<ApplicationUser> _userManager;





        public UserController(UserManager<ApplicationUser> userManager)

        {

            _userManager = userManager;

        }



        /// <summary>

        /// Generate a view model with a list of users, either from a specific role or all users.

        /// </summary>

        /// <param name="roleName">The role name to filter users by. If null, all users returned.</param>

        /// <returns>View model populated with users.</returns>

        private async Task<UserViewModel> GenerateUserViewModel(string? roleName = null)

        {

            IEnumerable<ApplicationUser> users;



            if (roleName == null)

            {

                // All users

                users = _userManager.Users.ToList();

            }

            else

            {

                // Users in specific role

                users = await _userManager.GetUsersInRoleAsync(roleName);

            }



            return new UserViewModel

            {

                Users = users

            };

        }



        // GET: UserController

        public async Task<ActionResult> Index()

        {

            // Get all user data into VM

            return View(await GenerateUserViewModel());

        }



        // GET: UserController/User

        public async Task<ActionResult> Member()

        {

            // Get User user data into VM

            return View("Index", await GenerateUserViewModel("Member"));

        }



        // GET: UserController/Staff

        public async Task<ActionResult> Staff()

        {

            // Get Staff user data into VM

            return View("AllStaff", await GenerateUserViewModel("Staff"));

        }



        // GET: UserController/Manager

        public async Task<ActionResult> Manager()

        {

            // Get Manager user data into VM

            return View("Index", await GenerateUserViewModel("Manager"));

        }

    }

}