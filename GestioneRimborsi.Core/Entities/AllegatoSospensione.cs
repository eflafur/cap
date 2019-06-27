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
    [TableName("GRI_SOSPENSIONI_ALLEGATI")]
    public class AllegatoSospensione : IEntity
    {
        [Column("PROGRESSIVO")]
        public Int16 Progressivo { get; set; }

        [Column("NOME_FILE")]
        public String NomeFile { get; set; }

        [Column("ESTENSIONE")]
        public String TipoFile { get; set; }

        [Column("DIMENSIONE")]
        public String Dimensione { get; set; }        

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

        [Column("IDFS")]
        public String IdFS { get; set; }

        [Column("NOME_ORIGINALE")]
        public String NomeOriginale { get; set; }

        [Column("ID_SOSPENSIONE")]
        public long IdSospensione { get; set; }

        public object EntityId
        {
            get { return this.NomeFile; }
        }

        public string DisplayText
        {
            get { return string.Format("Nome File: {0}_{1}_{2}", this.IdSospensione, this.NomeFile, this.DataInserimento); }
        }
    }
}