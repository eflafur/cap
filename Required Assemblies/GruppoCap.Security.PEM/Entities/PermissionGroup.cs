using GruppoCap.Core;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Security.PEM
{
    [TableName("REVO_PERMISSIONGROUPS")]
    [PrimaryKey("PERMISSION_GROUP_ID", autoIncrement = false)]
    public class PermissionGroup
        : IPermissionGroup
    {

        #region " CTORs "

        // CTOR
        public PermissionGroup()
        {
            this.PermissionGroupId = Guid.NewGuid().ToString();
            this.Title = String.Empty;
            this.Description = String.Empty;
            this.IsPrivileged = false;
        }

        #endregion

        #region IPermissionGroup Members

        // PERMISSION GROUP ID
        [Column("PERMISSION_GROUP_ID")]
        public String PermissionGroupId { get; set; }

        // NAME
        public String Title { get; set; }

        // DESCRIPTION
        public String Description { get; set; }

        // IS PRIVILEGED
        [Column("IS_PRIVILEGED")]
        public Boolean IsPrivileged { get; set; }

        #endregion

        [Ignore]
        public Object EntityId
        {
            get { return PermissionGroupId; }
        }

        [Ignore]
        public String DisplayText
        {
            get { return Title;  }
        }
    }
}
