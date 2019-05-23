using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SepaManager.Base.Entity
{
    [Serializable]
    [TableName("SEPAPAYMENTELEMENTS")]
    [PrimaryKey("Id", autoIncrement = true, sequenceName = "SEPAPAYMENTELEMENTS_SEQ")]
    public class SepaPaymentElement
    {
        public Int64 ID { get; set; }
        public Int64 IDHeader { get; set; }
        public Int64 RequestedExecutionDate { get; set; }
        public string DebtorName { get; set; }
        public string DebtorCountry { get; set; }
        [Ignore]
        public string DebtorPostalAddressAndCountry { get; set; }
        public string DebtorPostalAddressLines { get; set; }
        public string DebtorTaxID { get; set; }
        public string DebtorIBAN { get; set; }
        public string DebtorABI { get; set; }
        public string DebtorBIC { get; set; }
        public string UltimateDebtorName { get; set; }
        public string UltimateDebtorTaxID { get; set; }
        public DateTime Created { get; set; }
        private List<SepaCreditTransferTransaction> _SepaCreditTransferTransactions;
       [Ignore]
       public List<SepaCreditTransferTransaction> SepaCreditTransferTransactions
        {
            get
            {
                if (_SepaCreditTransferTransactions == null)
                    _SepaCreditTransferTransactions = new List<SepaCreditTransferTransaction>();

                return _SepaCreditTransferTransactions;
            }
            set
            {
                _SepaCreditTransferTransactions = value;
            }
        }

        [Column(Name = "PmtInfPaymentMethod")]
        public String PaymentInformationPaymentMethod { get; set; }

        [Column(Name = "PmtInfBatchBooking")]
        public String PaymentInformationBatchBooking { get; set; }

        [Ignore]
        public bool IsBonificoDomiciliato
        {
            get
            {
                return this.PaymentInformationPaymentMethod == "CIR";
            }

        }

        //[Ignore]
        //public  bool  IsAssegno
        //{
        //    get
        //    {
        //        return this.PaymentInformationPaymentMethod == "CHK";
        //    }

        //}
    }
}
