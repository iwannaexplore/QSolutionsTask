using System.Collections;
using System.Xml.Serialization;

namespace qSolutionsTask.Entity;

[Serializable]
public class ClQACResultAddress
{
    public int ErrorCode;
    public string ErrorMessage;
    public int ResultStatus;
    public ClQACAddress ResultAddress;
    public ArrayList SimilarAddresses;
}