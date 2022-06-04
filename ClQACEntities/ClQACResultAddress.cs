using System.Collections;
using System.Xml.Serialization;

namespace ClQACEntities;

[Serializable]
public class ClQACResultAddress
{
    public int ErrorCode;
    public string ErrorMessage;
    public int ResultStatus;
    public ClQACAddress ResultAddress;
    [XmlArrayItem("anyType", Type = typeof(ClQACSimilarAddress))]
    public ArrayList SimilarAddresses;
}