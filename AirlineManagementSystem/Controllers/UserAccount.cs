using Microsoft.AspNetCore.Mvc;

public class UserAccountController : Controller{
    public IActionResult Index(){
        return View();
    }
    public IActionResult Tickets(){
        return View();
    }
}