using System.Xml.Serialization;
using ClQACEntities;

namespace SoapToJson.ViewModels;

[Serializable]
[XmlType(TypeName = "UCheckAddress")]
[XmlRoot(Namespace = "http://www.qaddress.de/webservices")]
public class UCheckAddressViewModel
{
    public string UserName { get; set; }
    public string UserPassword { get; set; }
    public int Tolerance { get; set; }
    public ClQACAddress SourceAddress { get; set; }
}