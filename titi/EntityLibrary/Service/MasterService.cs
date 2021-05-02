using DatabaseUtility.EntityLibrary;
using EntityLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Util.Entity;

namespace EntityLibrary.Service
{
    public class MasterService:IMasterService
    {
        private readonly MasterRepositories masRep = new MasterRepositories();

        public SearchResult<IList<Province>> GetAllProvince()
        {
            return masRep.GetAllProvince();
        }

        public SearchResult<IList<District>> GetDistrictByProvinceId(long provinceId)
        {
            return masRep.GetDistrictByProvinceId(provinceId);
        }

        public SearchResult<ShippingEntity> GetShippingByUserID(long UserId)
        {
            return masRep.GetShippingByUserID(UserId);
        }

        public SearchResult<IList<Ward>> GetWardByDistrictId(long districtId)
        {
            return masRep.GetWardByDistrictId(districtId);
        }

        public PostResult<Shipping> InserShipping(Shipping shipping)
        {
            return masRep.InserShipping(shipping);
        }
        public PostResult<Shipping> UpdateShipping(Shipping shipping)
        {
            return masRep.UpdateShipping(shipping);
        }
    }
}
