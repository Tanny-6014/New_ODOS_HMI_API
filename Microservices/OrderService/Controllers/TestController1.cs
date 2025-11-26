using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Oracle.ManagedDataAccess.Client;
using OrderService.Models;
using System.Globalization;

namespace OrderService.Controllers
{
    public class TestController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
         }
}
