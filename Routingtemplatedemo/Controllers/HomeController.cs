using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Routingtemplatedemo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Routingtemplatedemo.Controllers
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
      var names = GetNames();
      
      return View(names);
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [Route("detail/{id}")]
    public IActionResult Detail(string id)
    {
      ViewData["tekst"] = id;
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public List<string> GetNames()
    {
      // stel in waar de database gevonden kan worden
      string connectionString = "Server=172.16.160.21;Port=3306;Database=fastfood;Uid=lgg;Pwd=;";

      // maak een lege lijst waar we de namen in gaan opslaan
      List<string> names = new List<string>();

      // verbinding maken met de database
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        // verbinding openen
        conn.Open();

        // SQL query die we willen uitvoeren
        MySqlCommand cmd = new MySqlCommand("select * from product where id = 1", conn);

        // resultaat van de query lezen
        using (var reader = cmd.ExecuteReader())
        {
          // elke keer een regel (of eigenlijk: database rij) lezen
          while (reader.Read())
          {
            // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
            string Name = reader["naam"].ToString();

            // voeg de naam toe aan de lijst met namen
            names.Add(Name);
          }
        }
      }

      // return de lijst met namen
      return names;
    }

  }
}
