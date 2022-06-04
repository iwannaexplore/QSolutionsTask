using System.Text;
using ClQACEntities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoapToJson.Extensions;
using SoapToJson.Services;
using SoapToJson.ViewModels;

namespace SoapToJson.Controllers;

[EnableCors]
[ApiController]
[Route("[controller]")]
public class AddressCheckerController : ControllerBase
{
    private string DumbXml { get; set; } = @"<?xml version=""1.0"" encoding=""utf-8""?>
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

    [HttpPost]
    public async Task<string> CheckAddressAsync([FromBody] ClQACAddressViewModel addressViewModel)
    {
        var address = new ClQACAddress().FillFromViewModel(addressViewModel);
        var viewModel = CreateCheckAddressViewModel(address);
        var viewModelSoap = XmlSoapConverter.ConvertToSoapXml(viewModel, typeof(UCheckAddressViewModel));

        var postResponse = await PostSoapRequestAsync(AppConstants.ApiUrl, viewModelSoap);
        var container = XmlSoapConverter.ConvertFromSoapXmlToModel(postResponse);
        var model = container.UCheckAddressResult;


        ChangeAddress(model);
        model.ErrorMessage = CreateMessage(model.ResultStatus, model.ResultAddress);
        var modelJson = ConvertToJson(model);

        return modelJson;
    }

    [HttpGet]
    public string CheckAddressAsync()
    {
        return "Hello world";
    }

    private string ConvertToJson(ClQACResultAddress model)
    {
        var modelJson = JsonConvert.SerializeObject(model);
        return modelJson;
    }

    private void ChangeAddress(ClQACResultAddress model)
    {
        var status = (QAC_STATUS)model.ResultStatus;
        switch (status)
        {
            case QAC_STATUS.ERROR:
            case QAC_STATUS.NOTFOUND:
            case QAC_STATUS.ABROAD:
            case QAC_STATUS.CORRECT:
            case QAC_STATUS.MULTIPLERESULTS:
                break;
            case QAC_STATUS.AUTOCORRECTED:
                model.ResultAddress = (model.SimilarAddresses[0] as ClQACSimilarAddress)!.Address;
                break;
        }
    }

    private string CreateMessage(int statusNumber, ClQACAddress address)
    {
        var status = (QAC_STATUS)statusNumber;
        switch (status)
        {
            case QAC_STATUS.ERROR:
            case QAC_STATUS.NOTFOUND:
            case QAC_STATUS.ABROAD:
                return "Status ERROR/ABROAD/NOT FOUND obtained during address check, submitting is cancelled";
            case QAC_STATUS.CORRECT:
            case QAC_STATUS.AUTOCORRECTED:
                return address.ToFullAddress();
            case QAC_STATUS.MULTIPLERESULTS:
                return "Choose between multiple results";
            default:
                return "Unknown Error";
        }
    }

    private async Task<string> PostSoapRequestAsync(string url, string xmtText)
    {
        var httpClient = new HttpClient();
        using (HttpContent content = new StringContent(xmtText, Encoding.UTF8, "text/xml"))
        using (var request = new HttpRequestMessage(HttpMethod.Post, url))
        {
            var byteArray = Encoding.UTF8.GetBytes(xmtText);
            request.Content = content;

            request.Headers.Add("Content-Lenght", byteArray.Length.ToString());
            using (var response =
                   await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }
    }

    private UCheckAddressViewModel CreateCheckAddressViewModel(ClQACAddress address)
    {
        var viewModel = new UCheckAddressViewModel
        {
            UserName = AppConstants.TestUserName, UserPassword = AppConstants.TestUserPassword,
            Tolerance = AppConstants.TestTolerance, SourceAddress = address
        };
        return viewModel;
    }
}