using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SepaManager.Base.Entity
{
    [Serializable]
    [TableName("SEPACREDITTRANSFERTRANSACTION")]
    [PrimaryKey("Id", autoIncrement = true, sequenceName = "SEPACREDITTRANSFERTRANSACT_SEQ")]
    public partial class SepaCreditTransferTransaction
    {
        public Int64 ID { get; set; }
        public Int64 IDPayment { get; set; }
        public String InstructedAmount { get; set; }
        public String CreditorBIC { get; set; }
        public String CreditorName { get; set; }
        [Column(Name = "CreditorPostAddressCountryCode" )]
        public String CreditorPostalAddressCountryCode { get; set; }
        public String CreditorPostalAddress { get; set; }
        public String CreditorTaxID { get; set; }
        public String CreditorIBAN { get; set; }
        public String CreditorCF { get; set; }
        public String UltimateCreditorName { get; set; }
        public String UltimateCreditorTaxID { get; set; }
        public String Purpose { get; set; }
        public DateTime Created { get; set; }
        public String MmbId_ABIDest { get; set; }

        [Column(Name = "ChequeInstructionChqTp")]
        public String ChequeInstructionChequeType { get; set; }

        [Column(Name = "PmtTpInfServiceLevel")]
        public string PaymentTypeInformationServiceLevel { get; set; }

        [Column(Name = "PmtTpInfCategoryPurpose")]
        public string PaymentTypeInformationCategoryPurpose { get; set; }

        [Column(Name = "CdtrPstlAdrStreet")]
        public string CreditorPostalAddressStreet { get; set; }

        [Column(Name = "CdtrPstlAdrPostalCode")]
        public string CreditorPostalAddressPostalCode { get; set; }

        [Column(Name = "CdtrPstlAdrTownName")]
        public string CreditorPostalAddressTownName { get; set; }

        [Column(Name = "CdtrPstlAdrProvince")]
        public string CreditorPostalAddressProvince { get; set; }

        [Column(Name = "RmtInfUnstructured")]
        public string RemittanceInformationUnstructured { get; set; }
        
        public string PurposeCode { get; set; }
        
        public string PurposeProprietary { get; set; }

        [Ignore]
        public bool IsAssegno
        {
            get
            {
                return this.PurposeCode == "CASH";
            }
            
        }
        //TODO: aggiungere isBonificoDomiciliato se BOD

    }
}