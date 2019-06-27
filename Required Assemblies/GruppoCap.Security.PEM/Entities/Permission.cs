using GruppoCap.Core;
using PetaPoco;
using System;

namespace GruppoCap.Security.PEM
{
    [TableName("REVO_PERMISSIONS")]
    [PrimaryKey("PERMISSION_ID", autoIncrement = false)]
    public class Permission
        : IPermission
    {

        #region " CTORs "

        // CTOR
        public Permission()
        {
            this.PermissionId = Guid.NewGuid().ToString();
            this.Description = String.Empty;
            this.CategoryName = String.Empty;
            this.DefaultGrant = false;
            this.IsPrivileged = false;
        }

        // CTOR
        public Permission(String code)
            : this()
        {
            this.PermissionId = Guid.NewGuid().ToString();
            this.PermissionCode = code;
        }

        #endregion

        #region IPermission Members

        // PERMISSION ID
        [Column("PERMISSION_ID")]
        public String PermissionId { get; set; }

        // CODE
        [Column("PERMISSION_CODE")]
        public String PermissionCode { get; set; }

        // DESCRIPTION
        public String Description { get; set; }

        // CATEGORY NAME
        [Column("CATEGORY_NAME")]
        public String CategoryName { get; set; }

        // DEFAULT GRANT
        [Column("DEFAULT_GRANT")]
        public Boolean DefaultGrant { get; set; }

        // IS PRIVILEGED
        [Column("IS_PRIVILEGED")]
        public Boolean IsPrivileged { get; set; }

        #endregion

        [Ignore]
        public Object EntityId
        {
            get { return PermissionId; }
        }

        [Ignore]
        public String DisplayText
        {
            get { return "{0} {1}".FormatWith(PermissionCode.ToUpper(), Description); }
        }
    }
}
