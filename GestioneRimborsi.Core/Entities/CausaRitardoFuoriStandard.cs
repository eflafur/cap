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
    public class CausaRitardoFuoriStandard : IEntity
    {
        [Column("CODICE_CAUSA")]
        public String CodiceCausa { get; set; }

        [Column("DESCRIZIONE")]
        public String Descrizione { get; set; }

        [Column("FUORI_STANDARD_APPROVATO")]
        public Int32 FuoriStandardApprovato { get; set; }

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
            get { return string.Format("{0}", this.CodiceCausa.ToString()); }
        }

        public string DisplayText
        {
            get { return string.Format("Categoria FuoriStandard: {0}-{1}", this.CodiceCausa.ToString(), this.Descrizione); }
        }
    }
}
