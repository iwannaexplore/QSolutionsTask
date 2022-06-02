namespace qSolutionsTask.Services;

public static class SoapWrapper
{
    public static string WrapInSoap(string text)
    {
        var result =
            $@"<?xml version=""1.0"" encoding=""utf-8""?>   
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
            <soap:Body>
    {text}
   </soap:Body>
    </soap:Envelope>";
        return result;
    }
}