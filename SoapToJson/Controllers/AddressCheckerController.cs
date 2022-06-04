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
                return "Correct";
            case QAC_STATUS.AUTOCORRECTED:
                return "Autocorrected";
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