using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Util.Entity;

namespace EntityLibrary.Repositories
{
    public interface IMasterRepositories
    {
        SearchResult<IList<Province>> GetAllProvince();
        SearchResult<IList<District>> GetDistrictByProvinceId(long provinceId);
        SearchResult<IList<Ward>> GetWardByDistrictId(long districtId);
        PostResult<Shipping> InserShipping(Shipping shipping);
        PostResult<Shipping> UpdateShipping(Shipping shipping);
        SearchResult<ShippingEntity> GetShippingByUserID(long UserId);
    }
}
