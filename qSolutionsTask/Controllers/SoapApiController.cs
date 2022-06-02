using System.Net;
using System.Reflection.Metadata;
using System.Security;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using qSolutionsTask.Entity;
using qSolutionsTask.Services;
using qSolutionsTask.ViewModels;

namespace qSolutionsTask.Controllers;

public class SoapApiController : Controller
{
    [HttpPost]
    public async Task<string> CheckAddressAsync([FromForm] ClQACAddress address)
    {
        
        var dumbObject = DumbValue();
        var xmlResult = XmlSoapConverter.ConvertToSoapXml(dumbObject, typeof(UCheckAddressViewModel));

        var postResponse = await PostSoapRequestAsync(AppConstants.ApiUrl, xmlResult);
        var model = (UCheckAddressResponseViewModel)XmlSoapConverter.ConvertFromSoapXml(postResponse,
            typeof(UCheckAddressResponseViewModel));
        var modelJson = JsonSerializer.Serialize(model);
        return modelJson;
    }

    private async Task<string> PostSoapRequestAsync(string url, string xmtText)
    {
        var httpClient = new HttpClient();
        using (HttpContent content = new StringContent(xmtText, Encoding.UTF8, "text/xml"))
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(xmtText);
            request.Content = content;

            request.Headers.Add("Content-Lenght", byteArray.Length.ToString());
            using (HttpResponseMessage response =
                   await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }
    }

    private UCheckAddressViewModel DumbValue()
    {
        ClQACAddress address1 = new ClQACAddress()
        {
            m_iFlags = 1, m_sCity = "asd", m_sCountry = "Aad", m_sDistrict = "s", m_sRegion = "s", m_sStreet = "asd",
            m_sCityExt = "asd", m_iAddressType = 2, m_iHouseNo = 2, m_sHouseExt = "s", m_sOldCity = "asd",
            m_sOldStreet = "sd", m_sShortCity = "ds", m_sShortStreet = "sd", m_iCountryID = 2, m_iHouseNoEnd = 2,
            m_sHouseExtEnd = "asd", m_iLanguageID = 2, m_iRegionID = 3, m_sHouseExtStart = "asd",
            m_sOldCityExt = "a", m_sZIP = "ds"
        };
        UCheckAddressViewModel viewModel = new UCheckAddressViewModel()
        {
            UserName = AppConstants.TestUserName, UserPassword = AppConstants.TestUserPassword,
            Tolerance = AppConstants.TestTolerance, SourceAddress = address1
        };
        return viewModel;
    }


    public string DumbXml { get; set; } = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
    <soap:Body>
    <UCheckAddress xmlns=""http://www.qaddress.de/webservices"">
        <UserName>testuser</UserName>
    <UserPassword>TgNmUh-uw!sl$</UserPassword>
    <Tolerance>2</Tolerance>
    <SourceAddress>
    <m_iAddressType>1</m_iAddressType>
    <m_iFlags>1</m_iFlags>
    <m_iCountryID>1</m_iCountryID>
    <m_iRegionID>1</m_iRegionID>
    <m_iLanguageID>1</m_iLanguageID>
    <m_iHouseNo>1</m_iHouseNo>
    <m_iHouseNoStart>1</m_iHouseNoStart>
    <m_iHouseNoEnd>1</m_iHouseNoEnd>
    <m_sCountry>w</m_sCountry>
    <m_sRegion>w</m_sRegion>
    <m_sZIP>w</m_sZIP>
    <m_sCity>w</m_sCity>
    <m_sCityExt>w</m_sCityExt>
    <m_sShortCity>w</m_sShortCity>
    <m_sOldCity>w</m_sOldCity>
    <m_sOldCityExt>w</m_sOldCityExt>
    <m_sDistrict>w</m_sDistrict>
    <m_sStreet>w</m_sStreet>
    <m_sShortStreet>w</m_sShortStreet>
    <m_sOldStreet>w</m_sOldStreet>
    <m_sHouseExt>w</m_sHouseExt>
    <m_sHouseExtStart>w</m_sHouseExtStart>
    <m_sHouseExtEnd>w</m_sHouseExtEnd>
    </SourceAddress>
    </UCheckAddress>
    </soap:Body>
    </soap:Envelope>";
}