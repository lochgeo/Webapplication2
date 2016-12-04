using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccountRegistry.Models;

namespace AccountRegistry.Controllers
{
    [Authorize]
    public class AccessCodeController : Controller
    {
        // GET: AccessCode
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var user = User.Identity.GetUserId();
            var AccountList = db.InvoiceAccounts
                .Where(InvoiceAccount => InvoiceAccount.ApplicationUserId == user)
                .Select(InvoiceAccount => InvoiceAccount.InvoiceAccountId).ToList();
            var AccessList = db.AccessCodes
                .Where(a => AccountList.Contains(a.InvoiceAccountId));
            return View(AccessList);
        }

        // GET: AccessCode/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccessCode/Create
        public ActionResult Create(int id)
        {
            var db = new ApplicationDbContext();
            var account = db.InvoiceAccounts.Single(InvoiceAccount => InvoiceAccount.InvoiceAccountId == id);
            ViewBag.AccountNumber = account.AccountNumber;

            var accessCode = new AccessCode
            {
                BuyerEmail = string.Empty,
                InvoiceAccountId = id,
                AccessCodeId = 0,
                UniqueCode = string.Empty
            };
            return View(accessCode);
        }

        // POST: AccessCode/Create
        [HttpPost]
        public ActionResult Create(AccessCode model)
        {
            var db = new ApplicationDbContext();
            try
            {
                var accessCode = new AccessCode
                {
                    BuyerEmail = model.BuyerEmail,
                    InvoiceAccountId = model.InvoiceAccountId,
                    AccessCodeId = db.AccessCodes.Count() + 1,
                    UniqueCode = RandomString(8)
                };

                db.AccessCodes.Add(accessCode);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghjkmnpqrstuvwxyz0123456789#$%&";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        // GET: AccessCode/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccessCode/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AccessCode/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccessCode/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
