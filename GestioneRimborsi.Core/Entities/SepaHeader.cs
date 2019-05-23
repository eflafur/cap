using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using PetaPoco;
using System.ComponentModel.DataAnnotations;

namespace GestioneRimborsi.Core
{
    [TableName("SEPAHEADER")]
    public class SepaHeader : IEntity
    {        
        [Column("ID")]
        public Int32 ID { get; set; }

        [Column("COMPANYNUMBER")]
        public String CompanyNumber { get; set; }

        [Column("BANKACCOUNTNUMBER")]
        public String BankAccountNumber { get; set; }

        [Column("INITIATINGPARTYNAME")]
        public String IniatingPartyName { get; set; }

        [Column("ORGID")]
        public String OrgId { get; set; }

        [Column("COMPANYORGID")]
        public String CompanyOrgId { get; set; }

        [Column("ISSR")]
        public String Issr { get; set; }

        [Column("STATE")]
        public Int32 State { get; set; }

        [Column("CREATED")]
        public DateTime Created { get; set; }        

        [Column("BLOCCATO_IL")]
        public DateTime BloccatoIl { get; set; }

        [Column("BLOCCATO_DA")]
        public String BloccatoDa { get; set; }

        [Column("SBLOCCATO_IL")]
        public DateTime SbloccatoIl { get; set; }

        [Column("SBLOCCATO_DA")]
        public String SbloccatoDa { get; set; }

        [Column("AUTORE")]
        public String Autore { get; set; }

        public object EntityId
        {
            get { return string.Format("{0}", this.ID); }
        }

        public string DisplayText
        {
            get { return string.Format("{0}-{1}", this.Created, this.IniatingPartyName); }
        }
    }
}
