using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SparkCogingChallenge.Models;

namespace SparkCogingChallenge.Controllers
{
    public class PayController : Controller
    {
        SparkCodingChallengeEntities context = new SparkCodingChallengeEntities();

        public ActionResult PayList()
        {
            if (TempData["messsage"] != null)
            {
                ViewBag.messsage = TempData["messsage"];
            }
            List<Pay> paylist = context.Pay
                .Where(i => i.Active == true)
                .ToList();
            ViewBag.PayList = paylist;

            ViewBag.UserList = context.User
                .Where(i => i.Active == true)
                .Select(i => new SelectListItem 
            {
                Text = i.Username,
                Value = i.Id_user.ToString()
            }).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult PayList(Pay pay)
        {
            string action = Request.Form["button"].ToString();
            string deleteConfirm = Request.Form["delete"].ToString();
            string modifyConfirm = Request.Form["modify"].ToString();
            Pay temp = new Pay();

            switch (action)
            {
                case "Save":
                    if (!checkDuplicate(pay))
                    {
                        pay.CreationDate = DateTime.Now;
                        pay.Active = true;
                        context.Pay.Add(pay);
                        context.SaveChanges();
                        TempData["messsage"] = "Add";
                    }
                    else 
                    {
                        TempData["messsage"] = "Duplicate";
                    }
                    break;
                case "Modify":
                    if (modifyConfirm.Equals("Yes"))
                    {
                        temp = context.Pay.FirstOrDefault(i => i.Id_pay == pay.Id_pay);

                        if (!checkDuplicate(pay))
                        {
                            temp.Concept = pay.Concept;
                            temp.Quantity = pay.Quantity;
                            temp.Id_user = pay.Id_user;

                            context.SaveChanges();
                            TempData["messsage"] = "Modify";
                        }
                        else
                        {
                            TempData["messsage"] = "Duplicate";
                        }
                    }
                    break;
                case "Delete":
                    if (deleteConfirm.Equals("Yes"))
                    {
                        temp = context.Pay.FirstOrDefault(i => i.Id_pay == pay.Id_pay);

                        temp.Active = false;

                        context.SaveChanges();
                        TempData["messsage"] = "Delete";
                    }
                    break;
            }
            return RedirectToAction("PayList");
        }

        public ActionResult DeletePayList()
        {
            List<Pay> paylist = context.Pay
                .Where(i => i.Active == true)
                .ToList();
            ViewBag.PayList = paylist;

            return View(paylist);
        }
        
        [HttpPost]
        public ActionResult DeletePayList(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return View();
            }

            foreach (string id in ids)
            {
                int idtemp = Convert.ToInt32(id);
                Pay temp = context.Pay.FirstOrDefault(i => i.Id_pay == idtemp);

                temp.Active = false;

                context.SaveChanges();
            }
            return RedirectToAction("DeletePayList");
        }

        private bool checkDuplicate(Pay pay)
        {
            Pay temp = context.Pay.Where(i => i.Concept.Equals(pay.Concept) && i.Active == true).FirstOrDefault();
            if (temp != null)
            {
                if (temp.Concept == pay.Concept && 
                    temp.Id_pay != pay.Id_pay)
                {
                    return true;
                }
            }
            return false;
        }
    }
}