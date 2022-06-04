using ClQACEntities;

namespace SoapToJson.Extensions;

public static class ClQACAddressExtension
{
    public static string ToFullAddress(this ClQACAddress address)
    {
        return
            $"Country: {address.m_sCountry}, City: {address.m_sCity}, " +
            $"District: {address.m_sDistrict}," +
            $" Street:{address.m_sStreet} , HouseNumber: {address.m_iHouseNo}";
    }
}