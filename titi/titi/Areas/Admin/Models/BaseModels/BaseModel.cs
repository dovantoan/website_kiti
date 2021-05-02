using System;

namespace titi.Areas.Admin.Models.BaseModels
{

    public class BaseModel
    {
       
        public long Pid { get; set; }
      
        public DateTime? ModifiedDate { get; set; }
      
        public DateTime? CreatedDate { get; set; }
      
        public string CreatedBy { get; set; }
       
        public string ModifiedBy { get; set; }

    }
}