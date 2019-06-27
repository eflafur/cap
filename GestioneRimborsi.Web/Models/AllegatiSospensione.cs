using GestioneRimborsi.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestioneRimborsi.Web.Models
{
    public class AllegatiSospensione
    {
        public ISubCollection<AllegatoSospensione> allegatiSospensione { get; set; }
        public long IdSospensione { get; set; }
        public string IdFs { get; set; }
        public long RowId { get; set; }
        public bool SolaLettura { get; set; }
    }
}