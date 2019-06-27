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
    [TableName("GRI_BI_CONFERMATI_V")]
    public class BIConfermato : IEntity
    {
        [Column("ANNO_DOCUMENTO")]
        public String ANNO_DOCUMENTO { get; set; }
        [Column("NUMERO_DOCUMENTO")]
        public String NUMERO_DOCUMENTO { get; set; }
        [Column("TIPO_DOCUMENTO")]
        public String TIPO_DOCUMENTO { get; set; }
        [Column("STATO_DOCUMENTO")]
        public String STATO_DOCUMENTO { get; set; }
        [Column("DATA_INSERIMENTO")]
        public DateTime? DATA_INSERIMENTO { get; set; }       
        [Column("UTENTE_INSERIMENTO")]
        public String UTENTE_INSERIMENTO { get; set; }
        [Column("IMP_TOT_RIMB")]
        public Decimal IMP_TOT_RIMB { get; set; }
        [Column("ANNO_RIMBORSO_INTEGRA")]
        public String ANNO_RIMBORSO_INTEGRA { get; set; }
        [Column("ID_RIMBORSO_INTEGRA")]
        public String ID_RIMBORSO_INTEGRA { get; set; }
        [Column("CODICE_CLIENTE")]
        public String CODICE_CLIENTE { get; set; }
        [Column("ABI")]
        public String ABI { get; set; }
        [Column("CAB")]
        public String CAB { get; set; }
        [Column("CONTO_CORRENTE")]
        public String CONTO_CORRENTE { get; set; }
        [Column("CIN")]
        public String CIN { get; set; }
        [Column("IBAN")]
        public String IBAN { get; set; }
        [Column("LOTCAPID")]
        public Int32 LOTCAPID { get; set; }
        [Column("TIPOLOGIARICHIESTA")]
        public String TIPOLOGIARICHIESTA { get; set; }
        [Column("COD_PUNTO_FORNITURA")]
        public String COD_PUNTO_FORNITURA { get; set; }

        public object EntityId
        {
            get { return this.NUMERO_DOCUMENTO; }
        }

        public string DisplayText
        {
            get { return string.Format("Anno {1} - Numero : {0}", this.ANNO_DOCUMENTO, this.NUMERO_DOCUMENTO); }
        }
    }
}
