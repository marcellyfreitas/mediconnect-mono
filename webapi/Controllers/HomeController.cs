using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Ok(new
        {
            Name = "APIV1",
            Version = "v1.0.0",
        });
    }
}
