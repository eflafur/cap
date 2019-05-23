using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using GruppoCap.Core;


namespace GestioneRimborsi.Core
{
    [TableName("GRI_BI_REQUEST_VALIDATE")]
    [PrimaryKey("BI_REQUEST_ID")]
    public class BIRequestValidate:IEntity
    {
        [Column("BI_REQUEST_ID")]
        public int Id { get; set; }

        [Column("BI_LOTCAP_ID")]
        public int CapId { get; set; }

        [Column("BI_REQUEST_TYPE")]
        public int ReqType { get; set; }

        [Column("BI_ORIGINAL_CUST_ID")]
        public string  OriginalCustId{ get; set; }

        [Column("BI_EXPECTED_CUST_ID")]
        public string  ExpectedCustId { get; set; }

        [Column("BI_FORNPOINT_CHECK_FLG")]
        public int FornPointCheckFlag { get; set; }

        [Column("BI_CF_CHECK_FLG")]
        public int CfCheckFlag { get; set; }

        [Column("BI_RAGSOC_CHECK_FLG")]
        public int RagSocCheckFlag { get; set; }

        [Column("BI_USE_CHECK_FLG")]
        public int UseCheckFlag { get; set; }

        [Column("BI_DOMESTIC_CHECK_FLG")]
        public int DomesticCheckFlag { get; set; }

        [Column("BI_ADDRESS_CHECK_FLG")]
        public int AddresscCheckFlag { get; set; }

        [Column("BI_PEOPLE_CHECK_FLG")]
        public int PeopleCheckFlag { get; set; }

        [Column("BI_CUSTOMER_ACTIVE_CHECK_FLG")]
        public int CustomerActiveCheckFlag { get; set; }

        [Column("BI_FACILITATION_START")]
        public DateTime FacilitationStart{ get; set; }

        [Column("BI_OUTCOME")]
        public string  Outcome { get; set; }

        [Column("BI_EXT_OUTCOME")]
        public string ExtOutcome{ get; set; }


        [Ignore]
        public object EntityId
        {
            get { return this.Id; }
        }

        [Ignore]
        public string DisplayText
        {
            get { return string.Format("Utente {1} - RagioneSociale : {0}", this.Id, this.OriginalCustId); }
        }
    }
}
