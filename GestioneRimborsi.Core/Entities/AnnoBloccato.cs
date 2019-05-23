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
    [TableName("GRI_ANNI_BLOCCATI_FS")]
    public class AnnoBloccato : IEntity
    {
        [Column("ANNO_COMPETENZA")]
        public Int32 ANNO_COMPETENZA { get; set; }

        [Column("DATA_BLOCCO")]
        public DateTime DATA_BLOCCO { get; set; }

        [Column("UTENTE_INSERIMENTO")]
        public String UTENTE_INSERIMENTO { get; set; }

        [Column("DATA_INSERIMENTO")]
        public DateTime DATA_INSERIMENTO { get; set; }

        [Column("UTENTE_MODIFICA")]
        public String UTENTE_MODIFICA { get; set; }

        [Column("DATA_MODIFICA")]
        public DateTime DATA_MODIFICA { get; set; }

        [Ignore]
        public object EntityId
        {
            get { return string.Format("{0}-{1}", this.ANNO_COMPETENZA.ToString(), this.DATA_BLOCCO); }
        }
        [Ignore]
        public string DisplayText
        {
            get { return string.Format("Anno bloccato {0} in data {1}", this.ANNO_COMPETENZA.ToString(), this.DATA_BLOCCO); }
        }
    }
}