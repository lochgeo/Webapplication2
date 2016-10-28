﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using Microsoft.AspNet.Identity;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class InvoiceAccountController : Controller
    {
        // GET: InvoiceAccount
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var userId = User.Identity.GetUserId();
            var AccountList = db.InvoiceAccounts.Where(InvoiceAccount => InvoiceAccount.ApplicationUserId == userId);
            return View(AccountList);
        }

        // GET: InvoiceAccount/Details/5
        public ActionResult Details(int id)
        {
            var db = new ApplicationDbContext();
            var AccountList = db.InvoiceAccounts.Single(InvoiceAccount => InvoiceAccount.InvoiceAccountId == id); 
            return View(AccountList);
        }

        // GET: InvoiceAccount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvoiceAccount/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var db = new ApplicationDbContext();
            try
            {
                var invoiceAccount = new InvoiceAccount
                {
                    AccountNumber = collection["AccountNumber"].ToString(),
                    AccountName = collection["AccountName"].ToString(),
                    ApplicationUserId = User.Identity.GetUserId(),
                    InvoiceAccountId = db.InvoiceAccounts.Count() + 1                 
                };

                db.InvoiceAccounts.Add(invoiceAccount);
                db.SaveChanges();
                
                return View("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: InvoiceAccount/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InvoiceAccount/Edit/5
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

        // GET: InvoiceAccount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InvoiceAccount/Delete/5
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
