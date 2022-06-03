using System.Xml.Serialization;

namespace qSolutionsTask.Entity;

[Serializable]
public class ClQACSimilarAddress
{
    public int Similarity;
    public ClQACAddress Address;
}  