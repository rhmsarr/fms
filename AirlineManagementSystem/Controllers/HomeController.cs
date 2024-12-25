using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AirlineManagementSystem.Models;
using Npgsql;


namespace AirlineManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly string _connectionString;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("Database");

    }

    public IActionResult Index()
    {
        var query  = @"SELECT ""CountryName"" FROM ""Airport"" ORDER BY ""CountryName"" ASC;";
        List<string> Countries = new List<string>();
        
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       Countries.Add(reader.GetString(reader.GetOrdinal("CountryName")));
                    }
                }
            }
        }

        return View(Countries);
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
