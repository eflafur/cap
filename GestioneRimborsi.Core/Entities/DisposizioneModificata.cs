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
    public class DisposizioneModificata : IEntity
    {
        [Column("INTERNAL_ID")]
        public Int32 InternalId { get; set; }

        [Column("ID_TRANSAZIONE")]
        public long TransazioneId { get; set; }

        [Column("AUTORE")]
        public String Autore { get; set; }

        [Column("VECCHIO_IBAN")]
        public String VecchioIBAN { get; set; }

        [Column("NUOVO_IBAN")]
        public String NuovoIBAN { get; set; }

        [Column("VECCHIO_BENEFICIARIO")]
        public String VecchioBeneficiario { get; set; }

        [Column("NUOVO_BENEFICIARIO")]
        public String NuovoBeneficiario { get; set; }

        [Column("MODIFICATO_IL")]
        public DateTime ModificatoIl { get; set; }

        [Column("MOTIVAZIONE")]
        public String Motivazione { get; set; }        

        [Ignore]
        public object EntityId
        {
            get { return this.TransazioneId; }
        }

        [Ignore]
        public string DisplayText
        {
            get { return string.Format("Nome File: {0}_{1}_{2}", this.TransazioneId, this.Autore, this.ModificatoIl); }
        }
    }
}

