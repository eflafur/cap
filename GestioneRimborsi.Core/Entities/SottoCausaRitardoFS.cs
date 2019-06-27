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
    public class SottoCausaRitardoFS : IEntity
    {
        [Column("CODICE_SOTTOCAUSA")]
        public String CodiceSottoCausa { get; set; }

        [Column("CODICE_CAUSA")]
        public String CodiceCausa { get; set; }

        [Column("DESCRIZIONE")]
        public String Descrizione { get; set; }        

        [Column("IS_ACTIVE")]
        public Int32 IsActive { get; set; }

        [Column("CREATION_MOMENT")]
        public DateTime? CreatoIl { get; set; }

        [Column("CREATION_USER_ID")]
        public String UtenteCreazione { get; set; }

        [Column("LAST_UPDATE_MOMENT")]
        public DateTime? DataUltimoAggiornamento { get; set; }

        [Column("LAST_UPDATE_USER_ID")]
        public String UtenteAggiornamento { get; set; }

        public object EntityId
        {
            get { return string.Format("{0}", this.CodiceSottoCausa.ToString()); }
        }

        public string DisplayText
        {
            get { return string.Format("SottoCategoria FuoriStandard: {0}-{1}", this.CodiceSottoCausa.ToString(), this.Descrizione); }
        }
    }
}
