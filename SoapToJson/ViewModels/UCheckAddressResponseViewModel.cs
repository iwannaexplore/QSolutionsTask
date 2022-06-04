using System.Xml.Serialization;
using ClQACEntities;

namespace SoapToJson.ViewModels;

[Serializable]
[XmlType(TypeName = "UCheckAddressResponse")]
[XmlRoot(Namespace = "http://www.qaddress.de/webservices")]
public class UCheckAddressResponseViewModel
{
    public ClQACResultAddress UCheckAddressResult { get; set; }
}