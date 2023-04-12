using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using TP1_webApp.Models;

namespace TP1_webApp.Controllers
{
    public class HomeController : Controller
    {
        // HomeController
        private readonly ILogger<HomeController> _logger;
        SQLConnection myConnection = new SQLConnection();
        String User;
        String myIP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

        // Logger
        public HomeController(ILogger<HomeController> logger)
        {
            User = "Carito";
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
            ViewBag.UserName = myConnection.UserName;
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
            if (SQLconn.Name == null)
            {
                ViewBag.Class = "ERR";
                ViewBag.Name = "ERR";
                ViewBag.Price = "ERR";
            }
            else
            {
                SQLconn.Add(SQLconn.Class, SQLconn.Name, SQLconn.Price, User, myIP);

                if (SQLconn.Add_msg == "")
                {
                    ViewBag.Class = SQLconn.Class;
                    ViewBag.Name = SQLconn.Name;
                    ViewBag.Price = SQLconn.Price;
                }
                ViewBag.Add_msg = SQLconn.Add_msg;
            }
            return View("Insert");
        }

        // Add items method
        [HttpPost]
        public ActionResult LogIn(Models.SQLConnection SQLconn)
        {
            myConnection = SQLconn;
            myConnection.LogIn(myConnection.UserName, myConnection.Password, User, myIP);
            ViewBag.Result = myConnection.LogIn_msg;
            if (myConnection.LogIn_Result)
            {
                User = myConnection.UserName;
                return View("Privacy", myConnection);
            }
            else
            {
                return View("Index", myConnection);
            }
            
        }

        // Filter by Name method
        [HttpPost]
        public ActionResult FilterByName(SQLConnection SQLconn)
        {
            ViewBag.filter = SQLconn.NameFilter_txt;
            if (SQLconn.NameFilter_txt == null)
            {
                SQLconn.Get();
                ViewBag.Count = SQLconn.ItemsListCount;
                return View("Privacy", SQLconn);
            }
            // ... calling the Get method
            SQLconn.FilterName(ViewBag.filter, User, myIP);
            ViewBag.Count = SQLconn.ItemsListCount;
            return View("Privacy", SQLconn);
        }

        // Filter by Count method
        [HttpPost]
        public ActionResult FilterByCount(SQLConnection SQLconn)
        {
            ViewBag.filter = SQLconn.CountFilter_txt;
            if (SQLconn.CountFilter_txt != (int)SQLconn.CountFilter_txt)
            {
                SQLconn.Get();
                ViewBag.Count = SQLconn.ItemsListCount;
                return View("Privacy", SQLconn);
            }
            // ... calling the Get method
            SQLconn.FilterCount(ViewBag.filter, User, myIP);
            ViewBag.Count = SQLconn.ItemsListCount;
            return View("Privacy", SQLconn);
        }

        // Filter by Count method
        [HttpPost]
        public ActionResult FilterByClass(SQLConnection SQLconn)
        {
            ViewBag.filter = SQLconn.ClassFilter_txt;
            if (SQLconn.ClassFilter_txt == null)
            {
                SQLconn.Get();
                ViewBag.Count = SQLconn.ItemsListCount;
                return View("Privacy", SQLconn);
            }
            // ... calling the Get method
            SQLconn.FilterClass(ViewBag.filter, User, myIP);
            ViewBag.Count = SQLconn.ItemsListCount;
            return View("Privacy", SQLconn);
        }



    }
}