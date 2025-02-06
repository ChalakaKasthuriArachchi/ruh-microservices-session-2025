using Microsoft.AspNetCore.Mvc;

namespace invoicing_service.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{

    public HomeController()
    {
        
    }

    [HttpGet]
    public IActionResult Get()
    {
        Console.WriteLine("Home Page Accessed!");
        return Ok("Welcome to the invoicing service!");
    }
}
