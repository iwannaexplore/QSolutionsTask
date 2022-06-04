using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Text;
using ClQACEntities;
using Microsoft.AspNetCore.Mvc;
using qSolutionsTask.Entity;
using qSolutionsTask.Models;

namespace qSolutionsTask.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var multipleTest = new ClQACAddress()
            { m_sCountry = "Deutschland", m_sStreet = "Offenbachstr.", m_sZIP = "81245" };
        var correctTest = new ClQACAddress()
        {
            m_sCountry = "Deutschland", m_sZIP = "81245", m_sCity = "München", m_sStreet = "Offenbachstr.",
            m_iHouseNo = 47, m_sDistrict = "Pasing-Obermenzing"
        };
        var autoCorrect = new ClQACAddress()
        {
            m_sCountry = "Deutschland", m_sZIP = "81245", m_sCity = "München", m_sStreet = "Ophenbachstr.",
            m_iHouseNo = 47, m_sDistrict = "Pasing-Obermenzing"
        };
        return View(multipleTest);
    }


    [HttpPost]
    public IActionResult SubmitForm(ClQACAddress address)
    {
        return Ok();
    }
}