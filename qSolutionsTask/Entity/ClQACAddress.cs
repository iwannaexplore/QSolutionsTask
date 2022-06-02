using System.Xml.Serialization;

namespace qSolutionsTask.Entity;

[Serializable]
public class ClQACAddress
{
    public int m_iAddressType;
    public int m_iFlags;
    public int m_iCountryID;
    public int m_iRegionID;
    public int m_iLanguageID;
    public int m_iHouseNo;
    public int m_iHouseNoStart;
    public int m_iHouseNoEnd;
    public string m_sCountry;
    public string m_sRegion;
    public string m_sZIP;
    public string m_sCity;
    public string m_sCityExt;
    public string m_sShortCity;
    public string m_sOldCity;
    public string m_sOldCityExt;
    public string m_sDistrict;
    public string m_sStreet;
    public string m_sShortStreet;
    public string m_sOldStreet;
    public string m_sHouseExt;
    public string m_sHouseExtStart;
    public string m_sHouseExtEnd;
}