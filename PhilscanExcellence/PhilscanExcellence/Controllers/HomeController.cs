using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhilscanExcellence.Models;
using PhilscanExcellence.Services;

namespace PhilscanExcellence.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCurrentUser()
        {
            var user = UniversalService.CurrentUser;

            return Json(user);
        }

        [HttpPost]
        public JsonResult TryLogin(string _username, string _password)
        {
            string serverResponse = "";

            var user = LoginService.ValidateLogin(_username, _password, out serverResponse);

            if(user != null)
            {
                LoginService.LoginToSession(user);
            }

            return Json(serverResponse);
        }

        [HttpPost]
        public JsonResult Logout()
        {
            string serverResponse = "";

            LoginService.LogoutFromSession(out serverResponse);

            return Json(serverResponse);
        }
    }
}