using System.Xml.Serialization;
using qSolutionsTask.Entity;

namespace qSolutionsTask.ViewModels;

[Serializable]
[XmlType(TypeName="UCheckAddress")]
[XmlRoot(Namespace = "http://www.qaddress.de/webservices")]
public class ClQACUCheckAddressViewModel
{
    public string UserName { get; set; }
    public string UserPassword { get; set; }
    public int Tolerance { get; set; }
    public ClQACAddress SourceAddress { get; set; }
}