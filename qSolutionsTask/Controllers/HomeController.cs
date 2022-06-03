using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using qSolutionsTask.Entity;
using qSolutionsTask.Models;

namespace qSolutionsTask.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View(new ClQACAddress(){m_sCountry = "Deutschland", m_sStreet = "Offenbachstr.", m_sZIP = "81245"});
    }

   
    [HttpPost]
    public IActionResult SubmitForm([FromForm] ClQACAddress address)
    {
        return Ok();
    }

    
    
}