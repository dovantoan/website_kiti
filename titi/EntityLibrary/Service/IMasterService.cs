using System.Collections.Generic;
using Util;
using Util.Entity;

namespace EntityLibrary.Service
{
    public interface IMasterService
    {
        SearchResult<IList<Province>> GetAllProvince();
        SearchResult<IList<District>> GetDistrictByProvinceId(long provinceId);
        SearchResult<IList<Ward>> GetWardByDistrictId(long districtId);
        PostResult<Shipping> InserShipping(Shipping shipping);
        PostResult<Shipping> UpdateShipping(Shipping shipping);
        SearchResult<ShippingEntity> GetShippingByUserID(long UserId);
    }
}
