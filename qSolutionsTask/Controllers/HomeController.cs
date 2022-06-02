using System.Buffers;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using qSolutionsTask.Entity;
using qSolutionsTask.Models;

namespace qSolutionsTask.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {
    }

    public async Task<IActionResult> Index()
    {
        SoapApiController asd = new SoapApiController();
        await asd.CheckAddressAsync(new ClQACAddress());
        return View();
    }


    
}