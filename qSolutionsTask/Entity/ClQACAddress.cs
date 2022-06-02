using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace qSolutionsTask.Entity;

[Serializable]
public class ClQACAddress
{
    public int m_iAddressType { get; set; }
    public int m_iFlags { get; set; }
    public int m_iCountryID { get; set; }
    public int m_iRegionID { get; set; }
    public int m_iLanguageID { get; set; }
    [Display(Name = "House Number")]
    public int m_iHouseNo { get; set; }
    public int m_iHouseNoStart { get; set; }
    public int m_iHouseNoEnd { get; set; }
    [Display( Name = "Country")]
    public string m_sCountry { get; set; }

    public string m_sRegion { get; set; }
    [Display(Name = "Postal Code")]
    public string m_sZIP { get; set; }
    [Display(Name = "City")]
    public string m_sCity { get; set; }
    public string m_sCityExt { get; set; }
    public string m_sShortCity { get; set; }
    public string m_sOldCity { get; set; }
    public string m_sOldCityExt { get; set; }
    [Display(Name = "District")]
    public string m_sDistrict { get; set; }
    [Display(Name = "Street")] 
    public string m_sStreet { get; set; }
    public string m_sShortStreet { get; set; }
    public string m_sOldStreet { get; set; }
    public string m_sHouseExt { get; set; }
    public string m_sHouseExtStart { get; set; }
    public string m_sHouseExtEnd { get; set; }
}