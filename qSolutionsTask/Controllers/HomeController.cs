using System.Buffers;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using qSolutionsTask.Models;

namespace qSolutionsTask.Controllers;

public class HomeController : Controller
{


    public HomeController()
    {
    }

    public IActionResult Index()
    {
        PostSoapRequest("https://qws1.de/QWS/QACWebService.asmx?op=UCheckAddress", @"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
  <soap12:Body>
    <UCheckAddress xmlns=""http://www.qaddress.de/webservices"">
      <UserName>testuser</UserName>
      <UserPassword>TgNmUh-uw!sl$</UserPassword>
      <Tolerance>2</Tolerance>
      <SourceAddress>
        <m_iAddressType>2</m_iAddressType>
        <m_iFlags>2</m_iFlags>
        <m_iCountryID>2</m_iCountryID>
        <m_iRegionID>2</m_iRegionID>
        <m_iLanguageID>2</m_iLanguageID>
        <m_iHouseNo>2</m_iHouseNo>
        <m_iHouseNoStart>2</m_iHouseNoStart>
        <m_iHouseNoEnd>2</m_iHouseNoEnd>
        <m_sCountry>asd</m_sCountry>
        <m_sRegion>asd</m_sRegion>
        <m_sZIP>as</m_sZIP>
        <m_sCity>asd</m_sCity>
        <m_sCityExt>asd</m_sCityExt>
        <m_sShortCity>asd</m_sShortCity>
        <m_sOldCity>asd</m_sOldCity>
        <m_sOldCityExt>asd</m_sOldCityExt>
        <m_sDistrict>asd</m_sDistrict>
        <m_sStreet>asd</m_sStreet>
        <m_sShortStreet>asd</m_sShortStreet>
        <m_sOldStreet>asd</m_sOldStreet>
        <m_sHouseExt>asd</m_sHouseExt>
        <m_sHouseExtStart>asd</m_sHouseExtStart>
        <m_sHouseExtEnd>asd</m_sHouseExtEnd>
      </SourceAddress>
    </UCheckAddress>
  </soap12:Body>
</soap12:Envelope>");
        return View();
    }
    
    
    private  async Task<string> PostSoapRequestAsync(string url, string text)
    {
        var httpClient = new HttpClient();
        using (HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml"))
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
        {
            request.Content = content;
            using (HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                //response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
                return await response.Content.ReadAsStringAsync();
            }
        }
    }

    private void PostSoapRequest(string endPoint, string requestMesg)
    {
        WebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
        request.Method = "POST";

        byte[] byteArray = Encoding.UTF8.GetBytes(requestMesg);
        request.ContentLength = byteArray.Length;

        request.ContentType = "application/soap+xml;charset=UTF-8";

        NetworkCredential creds = new NetworkCredential("testuser", "TgNmUh-uw!sl$");
        request.Credentials = creds;

        Stream datastream = request.GetRequestStream();
        datastream.Write(byteArray, 0, byteArray.Length);
        datastream.Close();

        WebResponse response = request.GetResponse();
        datastream = response.GetResponseStream();

        StreamReader reader = new StreamReader(datastream);

        var respMesg = reader.ReadToEnd().ToString();

        reader.Close();
    }
    
    
    
}