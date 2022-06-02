using System.Xml.Serialization;
using qSolutionsTask.Entity;

namespace qSolutionsTask.ViewModels;

[Serializable]
[XmlType(TypeName="UCheckAddressResponse")]

public class ClQACUCheckAddressResultViewModel
{
    public ClQACResultAddress UCheckAddressResult { get; set; }
}