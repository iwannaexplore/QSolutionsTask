using ClQACEntities;
using SoapToJson.ViewModels;

namespace SoapToJson.Extensions;

public static class ClQACAddressExtension
{
    public static ClQACAddress FillFromViewModel(this ClQACAddress address, ClQACAddressViewModel viewModel)
    {
        address.m_sCity = viewModel.m_sCity;
        address.m_sCountry = viewModel.m_sCountry;
        address.m_sDistrict = viewModel.m_sDistrict;
        address.m_sStreet = viewModel.m_sStreet;
        address.m_iHouseNo = int.Parse(viewModel.m_iHouseNo);
        address.m_sZIP = viewModel.m_sZIP;
        return address;
    }
}