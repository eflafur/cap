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

    public class AllegatoRimborso : IEntity
    {
        [Column("ANNO_DOCUMENTO")]
        public String AnnoDocumento { get; set; }

        [Column("NUMERO_DOCUMENTO")]
        public String NumeroDocumento { get; set; }

        [Column("PROGRESSIVO")]
        public Int16 Progressivo { get; set; }

        [Column("NOME_FILE")]
        public String NomeFile { get; set; }

        [Column("ESTENSIONE")]
        public String TipoFile { get; set; }

        [Column("DIMENSIONE")]
        public String Dimensione { get; set; }

        [Column("DATA_FILE")]
        public DateTime? DataFile { get; set; }

        [Column("NOTE")]
        public String Note { get; set; }

        [Column("DATA_INSERIMENTO")]
        public DateTime? DataInserimento { get; set; }

        [Column("UTENTE_INSERIMENTO")]
        public String UtenteInserimento { get; set; }

        [Column("DATA_ULT_VARIAZIONE")]
        public DateTime? DataUltimaVariazione { get; set; }

        [Column("UTENTE_ULT_VARIAZIONE")]
        public String UtenteUltimaVariazione { get; set; }

        public object EntityId
        {
            get { return this.NomeFile; }
        }

        public string DisplayText
        {
            get { return string.Format("Nome File: {0}_{1}_{2}", this.AnnoDocumento, this.NumeroDocumento, this.Progressivo); }
        }
    }
}
