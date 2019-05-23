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
    [TableName("SEPACREDITTRANSFERTRANSACTION")]
    public class SepaCreditTransaction : IEntity
    {

        [Column("ID")]
        public Int32 ID { get; set; }

        [Column("IDPAYMENT")]
        public Int32 IdPayment { get; set; }

        [Column("INSTRUCTEDAMOUNT")]
        public String InstructedAmount { get; set; }

        [Column("CREDITORBIC")]
        public String CreditorBic { get; set; }

        [Column("MMBID_ABIDEST")]
        public String MmbidAbidest { get; set; }

        [Column("CREDITORNAME")]
        public String CreditorName { get; set; }

        [Column("CREDITORPOSTADDRESSCOUNTRYCODE")]
        public String CreditorPostAddressCountryCode { get; set; }

        [Column("CREDITORPOSTALADDRESS")]
        public String CreditorPostalAddress { get; set; }

        [Column("CREDITORTAXID")]
        public String CreditorTaxId { get; set; }

        [Column("CREDITORIBAN")]
        public String CreditorIban { get; set; }

        [Column("CREDITORCF")]
        public String CreditorCF { get; set; }

        [Column("ULTIMATECREDITORNAME")]
        public String UltimateCreditorName { get; set; }

        [Column("ULTIMATECREDITORTAXID")]
        public String UltimateCreditorTaxId { get; set; }

        [Column("PURPOSE")]
        public String Purpose { get; set; }

        [Column("CREATED")]
        public DateTime Created { get; set; }

        [Column("MODIFICATO_IL")]
        public DateTime ModificatoIl { get; set; }

        [Column("MODIFICATO_DA")]
        public String ModificatoDa { get; set; }

        [Column("ELIMINATO_IL")]
        public DateTime EliminatoIl { get; set; }

        [Column("ELIMINATO_DA")]
        public String EliminatoDa { get; set; }

        [Column("RECUPERATO_IL")]
        public DateTime RecuperatoIl { get; set; }

        [Column("RECUPERATO_DA")]
        public String RecuperatoDa { get; set; }

        [Column("CHEQUEINSTRUCTIONCHQTP")]
        public String ChequeInstructionChqtp { get; set; }

        [Column("PMTTPINFSERVICELEVEL")]
        public String PmttpInfServiceLevel { get; set; }

        [Column("PMTTPINFCATEGORYPURPOSE")]
        public String PmttpInfCategoryPurpose { get; set; }

        [Column("CDTRPSTLADRSTREET")]
        public String CdtrpStladrStreet { get; set; }

        [Column("CDTRPSTLADRPOSTALCODE")]
        public String CdtrpStldrPostalCode { get; set; }

        [Column("CDTRPSTLADRTOWNNAME")]
        public String CdtrpStladrTownName { get; set; }

        [Column("CDTRPSTLADRPROVINCE")]
        public String CdtrpStladrProvince { get; set; }

        [Column("PURPOSECODE")]
        public String PurposeCode { get; set; }    

        [Column("PURPOSEPROPRIETARY")]
        public String PurposeProprietary { get; set; }

        [Column("RMTINFUNSTRUCTURED")]
        public String RmtInfunstructured { get; set; }    


        public object EntityId
        {
            get { return string.Format("{0}", this.ID); }
        }

        public string DisplayText
        {
            get { return string.Format("{0}-{1}", this.CreditorName); }
        }
    }
}
