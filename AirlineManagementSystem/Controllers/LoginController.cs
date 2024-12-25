using Microsoft.AspNetCore.Mvc;

namespace AirlineManagementSystem.Controllers{
    public class LoginController : Controller{
        public IActionResult Index(){
            return View();
        }
    }
}