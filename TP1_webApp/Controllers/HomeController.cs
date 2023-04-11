using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using TP1_webApp.Models;

namespace TP1_webApp.Controllers
{
    public class HomeController : Controller
    {
        // HomeController
        private readonly ILogger<HomeController> _logger;
        SQLConnection myConnection = new SQLConnection();

        // Logger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        // Index view
        public IActionResult Index()
        {
            return View();
        }


        // Return to Privacy view
        public IActionResult Return()
        {
            // ... calling the model method
            myConnection.Get();
            ViewBag.Count = myConnection.ItemsListCount;
            return View("Privacy", myConnection);
        }

        // Return Home view
        public IActionResult Sign_Out()
        {
            // ... calling the model method
            myConnection.LogIn_Result = false;
            return View("Index", myConnection);
        }

        // Insert view
        public IActionResult Insert()
        {
            return View("Insert", myConnection);
        }

        // Privacy view
        public IActionResult Privacy()
        {
            
            // ... calling the model method
            myConnection.Get();
            ViewBag.Count = myConnection.ItemsListCount;
            return View(myConnection);
        }

        // Error view
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Get items method
        public ActionResult Get_Items()
        {
            // ... calling the Get method
            myConnection.Get();
            ViewBag.Count = myConnection.ItemsListCount;
            return View("Privacy", myConnection);
        }

        // Add items method
        [HttpPost]
        public ActionResult Insert(Models.SQLConnection SQLconn)
        {
            ViewBag.Class = SQLconn.Class;
            ViewBag.Name = SQLconn.Name;
            ViewBag.Price = SQLconn.Price;
            if (SQLconn.Name == null)
            {
                ViewBag.Class = "ERR";
                ViewBag.Name = "ERR";
                ViewBag.Price = "ERR";
            }
            else
            {
                SQLconn.Add(SQLconn.Class, SQLconn.Name, SQLconn.Price);
            }
            return View("Insert");
        }

        // Add items method
        [HttpPost]
        public ActionResult LogIn(Models.SQLConnection SQLconn)
        {
            SQLconn.LogIn(SQLconn.UserName, SQLconn.Password);
            ViewBag.Result = SQLconn.LogIn_msg;
            if (SQLconn.LogIn_Result)
            {
                return View("Privacy", SQLconn);
            }
            else
            {
                return View("Index", SQLconn);
            }
            
        }

        // GFilter by Name method
        public ActionResult FilterByName()
        {
            // ... calling the Get method
            myConnection.FilterName();
            ViewBag.Count = myConnection.ItemsListCount;
            return View("Privacy", myConnection);
        }

        // GFilter by Name method
        [HttpPost]
        public ActionResult mio(SQLConnection SQLconn)
        {
            // ... calling the Get method
            myConnection.Cosito(SQLconn.NameFilter_txt);
            ViewBag.Count = SQLconn.NameFilter_txt;
            return View("Privacy", myConnection);
        }



    }
}