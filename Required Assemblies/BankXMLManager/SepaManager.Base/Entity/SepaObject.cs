using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SepaManager.Base.Entity
{
    public class SepaObject 
    {
        public string CUC { get; set; }
        public string ABIOrd { get; set; }
        public DateTime DataOp { get; set; }
        public string pkOperazione { get; set; }
        public string NomeSupporto { get; set; }
        public int NumRiep { get; set; }
        public int NumRighe { get; set; }
        public decimal ImportoTot { get; set; }
        public string protocollo { get; set; }
        public DateTime DataPag { get; set; }
        public decimal Importo { get; set; }
        public string CABOrd { get; set; }
        public string CCOrd { get; set; }
        public string IBANOrd { get; set; }
        public string ABIDest { get; set; }
        public string CABDest { get; set; }
        public string CCDest { get; set; }
        public string IBANDest { get; set; }
        public string CINDest { get; set; }
        public string RAGIONESOCIALE { get; set; }
        public string INDIRIZZO { get; set; }
        public string CITTA { get; set; }
        public string PROVINCIA { get; set; }
        public string CAP { get; set; }
        public string CFIVA { get; set; }
        public string CODICE_FISCALE { get; set; }
        public string DescrDest { get; set; }
        public string Descr50 { get; set; }
        public string CodiceUTF { get; set; }
        public string OrgId { get; set; }

        [Column(Name = "Codice_Cliente")]
        public string IdCliente { get; set; }

        [Column(Name = "ChequeInstructionChqTp")]
        public string ChequeInstructionChequeType { get; set; }

        [Column(Name = "PmtInfPaymentMethod")]
        public String PaymentInformationPaymentMethod { get; set; }

        [Column(Name = "PmtInfBatchBooking")]
        public String PaymentInformationBatchBooking { get; set; }

        [Column(Name = "PmtTpInfServiceLevel")]
        public string PaymentTypeInformationServiceLevel { get; set; }

        [Column(Name = "PmtTpInfCategoryPurpose")]
        public string PaymentTypeInformationCategoryPurpose { get; set; }


        public string PurposeCode { get; set; }


        public string PurposeProprietary { get; set; }

        [Column(Name = "RmtInfUnstructured")]
        public string RemittanceInformationUnstructured { get; set; }

    }
}