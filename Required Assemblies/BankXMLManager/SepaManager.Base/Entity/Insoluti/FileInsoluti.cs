using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SepaManager.Base.Entity.Insoluti
{
    public class FileInsoluti
    {
        public string XmlString { get; set; }
        public decimal ImportoCalcolato
        {
            get
            {
                if (Transazioni == null || (!(Transazioni.Count() > 0)))
                {
                    return 0;
                }
                else
                    return Transazioni.Sum(x => x.IMPO_BOLLETTA);
            }
        }
        public int NumeroTransazioniCalcolate
        {
            get
            {
                if (Transazioni == null || (!(Transazioni.Count() > 0)))
                {
                    return 0;
                }
                else
                    return Transazioni.Count();
            }
        }
        public IEnumerable<InsolutoObject> Transazioni { get; set; }

        public bool SaveFile()
        {




            return true;
        }
    }
}
