using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EntityLibrary
{
    public partial class EntityLibContext : DbContext
    {
        public EntityLibContext(string connectionstring)
            : base(connectionstring)
        {}
    }


}
