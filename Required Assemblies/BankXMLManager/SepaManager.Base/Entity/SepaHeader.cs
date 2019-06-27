using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SepaManager.Base.Entity
{
    [Serializable]
    [TableName("SepaHeader")]
    [PrimaryKey("Id", autoIncrement = true, sequenceName = "SEPAHEADER_SEQ")]
    public partial class SepaHeader 
    {
        public Int64 ID { get; set; }
        public String CompanyNumber { get; set; }
        public String BankAccountNumber { get; set; }
        public String InitiatingPartyName { get; set; }
        public String OrgId { get; set; }
        public String CompanyOrgId { get; set; }
        public String Issr { get; set; }
        public Int32 State { get; set; }
        public DateTime Created { get; set; }
        public String Autore { get; set; }
        private List<SepaPaymentElement> _SepaPaymentElements;
        [Ignore]
        public List<SepaPaymentElement> SepaPaymentElements
        {
            get
            {
                if (_SepaPaymentElements == null)
                    _SepaPaymentElements = new List<SepaPaymentElement>();
                return _SepaPaymentElements;
            }
            set
            {
                _SepaPaymentElements = value;
            }
        }
        public long GetTransactionsNo()
        {
            long transNo = 0;
            foreach (SepaPaymentElement element in this.SepaPaymentElements)
                foreach (SepaCreditTransferTransaction transaction in element.SepaCreditTransferTransactions)
                    ++transNo;
            return transNo;
        }

        public double GetTotalAmount()
        {
            double totalAmount = 0;
            foreach (SepaPaymentElement element in this.SepaPaymentElements)
                foreach (SepaCreditTransferTransaction transaction in element.SepaCreditTransferTransactions)
                    totalAmount += double.Parse(transaction.InstructedAmount.Replace(",", "."), new System.Globalization.CultureInfo("en-US"));
            return totalAmount;
        }

    }
}