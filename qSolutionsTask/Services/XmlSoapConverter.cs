using System.Collections;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using qSolutionsTask.Entity;
using qSolutionsTask.ViewModels;

namespace qSolutionsTask.Services;

public static class XmlSoapConverter
{
    /*Изначально идея состояла в том, что я буду конвертировать строку в XML и обратно используя только классы
 и встроенный Serializer. Но из-за того, что SOAP вставляет свои теги в XML, приходится вручную вытаскивать нужный 
 элемент. Возможно, это можно как-то обойти, по крайней мере я пару раз пробовал, но безуспешно. Есть ещё пару идей,
  но нет времени на проверку, так что оставлю как есть*/
    public static object ConvertFromSoapXml(string postResponse, Type type)
    {
        
        XmlSerializer serializer = new XmlSerializer(type);

        XDocument document = XDocument.Parse(postResponse);
        XElement? evelope = (XElement)document.FirstNode!; //можно было бы написать через проверку на null
        var body = evelope.FirstNode as XElement;
        var uCheckAddress = body!.FirstNode as XElement;
        
        object response = serializer.Deserialize(uCheckAddress!.CreateReader())!;
        
        return response;
    }

    private static List<ClQACSimilarAddress> GetSimilarAddresses(ArrayList similarAddressesXml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ClQACSimilarAddress));
        var similarAddresses = new List<ClQACSimilarAddress>();
        foreach (var similarAddress in similarAddressesXml)
        {
            // var result = serializer.Deserialize(());
        }
        return new List<ClQACSimilarAddress>();
    }

    public static string ConvertToSoapXml(object obj, Type objType)
    {
        var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        var serializer = new XmlSerializer(objType);

        var settings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true
        };

        using var stream = new StringWriter();
        using var objectWriter = XmlWriter.Create(stream, settings);
        serializer.Serialize(objectWriter, obj, emptyNamespaces);

        var xmlObject = stream.ToString();
        var xmlSoap = WrapInSoap(xmlObject);

        return xmlSoap;
    }

    private static string WrapInSoap(string text)
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