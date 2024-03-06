using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SparkCogingChallenge.Models;

namespace SparkCogingChallenge.Controllers
{
    public class UserController : Controller
    {
        SparkCodingChallengeEntities context = new SparkCodingChallengeEntities();

        public ActionResult User()
        {
            return View(GetUserList());
        }

        [HttpPost]
        public ActionResult User(User user) 
        {
            List<User> userlist = context.User
                .Where(i => i.Active == true)
                .ToList();

            int count;

            if (userlist == null)
            {
                count = 0;
            }
            else 
            {
                count = userlist.Count;
            }

            User newUser = new User();

            newUser.Username = "User " + count;
            newUser.Active = true;

            context.User.Add(newUser);
            context.SaveChanges();

            return View(GetUserList());
        }

        private List<User> GetUserList()
        {
            return context.User.ToList();
        }
    }
}