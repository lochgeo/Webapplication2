using AccountRegistry.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AccountRegistry.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var eth = new Ethereum();

            Task.Run(async () =>
            {
                var BlockNumber = await eth.GetBlockNum();
                ViewBag.BlockNumber = BlockNumber.Value;

            }).Wait();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}