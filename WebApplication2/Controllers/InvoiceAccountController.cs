using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccountRegistry.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using AccountRegistry.Services;

namespace AccountRegistry.Controllers
{
    [Authorize]
    public class InvoiceAccountController : Controller
    {
        // GET: InvoiceAccount
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var userId = User.Identity.GetUserId();
            var AccountList = db.InvoiceAccounts
                .Where(InvoiceAccount => InvoiceAccount.ApplicationUserId == userId)
                .OrderBy(a => a.InvoiceAccountId);
            var address = db.Companies.Where(a => a.ApplicationUserId == userId).Select(a => a.Address).ToList<string>();
            ViewBag.Address = address[0];
            return View(AccountList);
        }

        // GET: InvoiceAccount/Details/5
        public ActionResult Details(int id)
        {
            var db = new ApplicationDbContext();
            var Account = db.InvoiceAccounts.Single(InvoiceAccount => InvoiceAccount.InvoiceAccountId == id); 
            return View(Account);
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
                    Confirmed = "No",
                    InvoiceAccountId = db.InvoiceAccounts.Count() + 1                 
                };

                db.InvoiceAccounts.Add(invoiceAccount);
                db.SaveChanges();

                Task.Run(async () =>
                {
                    var userId = User.Identity.GetUserId();
                    var address = db.Companies.Where(a => a.ApplicationUserId == userId).Select(a => a.Address).ToList<string>();
                    var eth = new Ethereum(address[0]);
                    var result = await eth.ExecuteContractStore("h@ck3r00", invoiceAccount.AccountNumber, invoiceAccount.AccountName);

                    if (result == "Account registered")
                    {
                        invoiceAccount.Confirmed = "Yes";
                        db.InvoiceAccounts.Attach(invoiceAccount);
                        var entry = db.Entry(invoiceAccount);
                        entry.Property(e => e.Confirmed).IsModified = true;
                        db.SaveChanges();
                    }
                });

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: InvoiceAccount/Edit/5
        public ActionResult Edit(int id)
        {
            var db = new ApplicationDbContext();
            var Account = db.InvoiceAccounts.Single(InvoiceAccount => InvoiceAccount.InvoiceAccountId == id);
            return View(Account);
        }

        // POST: InvoiceAccount/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var db = new ApplicationDbContext();

                var invoiceAccount = new InvoiceAccount
                {
                    AccountNumber = collection["AccountNumber"].ToString(),
                    AccountName = collection["AccountName"].ToString(),
                    ApplicationUserId = User.Identity.GetUserId(),
                    InvoiceAccountId = id
                };

                db.InvoiceAccounts.Attach(invoiceAccount);
                var entry = db.Entry(invoiceAccount);
                entry.Property(e => e.AccountNumber).IsModified = true;
                entry.Property(e => e.AccountName).IsModified = true;
                db.SaveChanges();
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
            var db = new ApplicationDbContext();
            var Account = db.InvoiceAccounts.Single(InvoiceAccount => InvoiceAccount.InvoiceAccountId == id);
            return View(Account);
        }

        // POST: InvoiceAccount/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var db = new ApplicationDbContext();
                var invoiceAccount = new InvoiceAccount {InvoiceAccountId = id};
                db.InvoiceAccounts.Attach(invoiceAccount);
                db.InvoiceAccounts.Remove(invoiceAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
