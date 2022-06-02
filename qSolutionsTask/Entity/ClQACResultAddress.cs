using System.Collections;

namespace qSolutionsTask.Entity;

public class ClQACResultAddress
{
    public int ErrorCode;
    public string ErrorMessage;
    public int ResultStatus;
    public ClQACAddress ResultAddress;
    public ArrayList SimilarAddresses;
}