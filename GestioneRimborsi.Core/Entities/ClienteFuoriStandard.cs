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
    public class ClienteFuoriStandard : IEntity
    {
        [Column("COD_CLIENTE")]
        public String CodCliente { get; set; }

        [Column("COD_CLIENTE_INTEGRA")]
        public String CodClienteIntegra { get; set; }

        [Column("DES_RAGIONE_SOCIALE")]
        public String RagioneSociale { get; set; }

        [Column("DES_CELLULARE")]
        public String Cellulare { get; set; }        


        public object EntityId
        {
            get { return string.Format("{0}-{1}", this.CodCliente.ToString()); }
        }

        public string DisplayText
        {
            get { return string.Format("Cliente num: {0}-{1}", this.CodCliente.ToString(), this.RagioneSociale); }
        }

    }
}

