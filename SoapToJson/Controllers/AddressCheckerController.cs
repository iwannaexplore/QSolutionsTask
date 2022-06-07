using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoapToJson.Extensions;
using SoapToJson.QACWebService;
using SoapToJson.ViewModels;

namespace SoapToJson.Controllers;

[EnableCors]
[ApiController]
[Route("[controller]")]
public class AddressCheckerController : ControllerBase
{
    public QACWebServiceSoapClient Client { get; set; }
    public AddressCheckerController(QACWebServiceSoapClient client)
    {
        Client = client;
    }
    [HttpPost]
    public async Task<string> CheckAddressAsync([FromBody] ClQACAddressViewModel addressViewModel)
    {
        
        var address = new ClQACAddress().FillFromViewModel(addressViewModel);
        var result = await Client.UCheckAddressAsync(AppConstants.TestUserName, AppConstants.TestUserPassword,
            AppConstants.TestTolerance, address);
        if (result == null)
        {
            return "Error";
        }

        var model = result.Body.UCheckAddressResult;
        model.ErrorMessage = CreateMessage(model.ResultStatus, model.ResultAddress);
        var modelJson = ConvertToJson(model);

        return modelJson;
    }

    private string ConvertToJson(ClQACResultAddress model)
    {
        var modelJson = JsonConvert.SerializeObject(model);
        return modelJson;
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
}

public enum QAC_STATUS
{
    ERROR = -2,
    ABROAD = -1,
    NOTFOUND = 0,
    CORRECT = 1,
    AUTOCORRECTED = 2,
    MULTIPLERESULTS = 3
}