using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using WiredSneakz.Models;

namespace WiredSneakz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Sneaker> sneakers = new List<Sneaker>();
            // Connect to MySQL.
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;database=wiredsneakz_db;port=3306;password=rootroot"))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Sneaker", connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Extract your data
                    Sneaker sneaker = new Sneaker();
                    sneaker.Id = Convert.ToInt32(reader["id"]);
                    sneaker.Name = reader["name"].ToString();
                    sneaker.ReleaseDate = Convert.ToDateTime(reader["releaseDate"]);
                    sneaker.Colorway = reader["colorway"].ToString();
                    sneaker.Price = Convert.ToDecimal(reader["price"]);

                    sneakers.Add(sneaker);
                }
                reader.Close();
            }

            return View(sneakers);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
