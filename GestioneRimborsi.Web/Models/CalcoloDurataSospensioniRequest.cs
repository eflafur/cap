using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestioneRimborsi.Web.Models
{
    public class CalcoloDurataSospensioniRequest
    {
        public inputData InputData { get; set; }
    }

    public class inputData
    {
        public DateTime finePrestazione { get; set; }
        public DateTime inizioPrestazione { get; set; }
        public string numeroPrestazione { get; set; }
        public sospList sospList { get; set; }
    }

    public class sospList
    {
        public List<Sospensione> sospRequest { get; set; }
    }

    public class Sospensione
    {
        public string codiceSospensione { get; set; }
        public DateTime inizioSospensione { get; set; }
        public DateTime fineSospensione { get; set; }
    }
}