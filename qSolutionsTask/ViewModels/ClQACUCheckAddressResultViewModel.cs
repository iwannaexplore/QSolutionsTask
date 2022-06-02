using System.Xml.Serialization;
using qSolutionsTask.Entity;

namespace qSolutionsTask.ViewModels;

[Serializable]
[XmlRoot(Namespace = "http://www.qaddress.de/webservices", ElementName = "UCheckAddressResponse")]
public class UCheckAddressResponse
{
    public ClQACResultAddress UCheckAddressResult { get; set; }
}