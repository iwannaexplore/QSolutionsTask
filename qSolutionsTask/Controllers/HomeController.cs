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

    public IActionResult Index()
    {
        SoapApiController asd = new SoapApiController();
        var dima = asd.CheckAddressAsync(new ClQACAddress()).Result;
        return View(new ClQACAddress());
    }

    public IActionResult ResultFromCall()
    {
        return PartialView("SubmittionInfoPartial", );
    }

    
}