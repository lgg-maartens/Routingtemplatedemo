using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Routingtemplatedemo.Database;
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
      var products = GetNames();
      
      return View(products);
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

    public List<Product> GetNames()
    {
      // stel in waar de database gevonden kan worden
      string connectionString = "Server=172.16.160.21;Port=3306;Database=fastfood;Uid=lgg;Pwd=pwVnb69iD@%r;";

      // maak een lege lijst waar we de namen in gaan opslaan
      List<Product> products = new List<Product>();

      // verbinding maken met de database
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        // verbinding openen
        conn.Open();

        // SQL query die we willen uitvoeren
        MySqlCommand cmd = new MySqlCommand("select * from product", conn);

        // resultaat van de query lezen
        using (var reader = cmd.ExecuteReader())
        {
          // elke keer een regel (of eigenlijk: database rij) lezen
          while (reader.Read())
          {
            Product p = new Product();
            p.Id = Convert.ToInt32(reader["Id"].ToString());
            p.Eenheid = reader["eenheid"].ToString();
            p.Naam = reader["naam"].ToString();
            p.Prijs = reader["prijs"].ToString();

            // voeg de naam toe aan de lijst met namen
            products.Add(p);
          }
        }
      }

      // return de lijst met namen
      return products;
    }

  }
}
