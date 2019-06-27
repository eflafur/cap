using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core.Models
{
    public abstract class QueryOptionBase
    {
        public abstract string fieldName { get; }
        public abstract string matchingVerb { get; }
    }

    public class QueryOptionLotId : QueryOptionBase
    {
        public override string fieldName
        {
            get
            {
                return "BI_LOTCAP_ID";
            }
        }

        public override string matchingVerb
        {
            get
            {
                return "{0}={1}";
            }
        }
    }

    public class QueryOptionUserType : QueryOptionBase
    {
        public override string fieldName
        {
            get
            {
                return "BI_TIPOLOGIA_UTENTE";
            }
        }

        public override string matchingVerb
        {
            get
            {
                return "{0} = {1}";
            }
        }
    }
    public class QueryOptionRequestStatus : QueryOptionBase
    {
        public override string fieldName
        {
            get
            {
                return "BI_PROCESSATO";
            }
        }

        public override string matchingVerb
        {
            get
            {
                return "{0} = {1}";
            }
        }
    }
    public class QueryOptionOutcome : QueryOptionBase
    {
        public override string fieldName
        {
            get
            {
                return "BI_ESITO";
            }
        }

        public override string matchingVerb
        {
            get
            {
                return "lower(NVL(BI_ESITO_MANVAL, BI_ESITO_AUTOVAL)) like lower('%{1}%')";
            }
        }
    }
    public class QueryOptionRequestId : QueryOptionBase
    {
        public override string fieldName
        {
            get
            {
                return "BI_REQ_CAP_ID";
            }
        }

        public override string matchingVerb
        {
            get
            {
                return "{0}={1}";
            }
        }
    }

    public class QueryOptions
    {
        protected Dictionary<QueryOptionBase, string> _conditionCriterias = new Dictionary<QueryOptionBase, string>();

        public Dictionary<QueryOptionBase, string> ConditionCriterias { get { return _conditionCriterias; } }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        internal int getUpperBound()
        {
            return PageSize * (PageIndex + 1); 
        }

        internal int getLowerBound()
        {
            return PageIndex * PageSize;
        }
    }
}
