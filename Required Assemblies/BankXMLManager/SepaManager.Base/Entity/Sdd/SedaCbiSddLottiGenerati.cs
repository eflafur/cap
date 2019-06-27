using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using System.IO;

namespace SepaManager.Base.Entity.Sdd
{
    [TableName("SDD_LOTTIGENERATI")]
    [PrimaryKey("BATCHONLINEID")]
    public class SedaCbiSddLottiGenerati
    {
        [Column("BATCHONLINEID")]
        public int Batch { get; set; }
        [Column("CREATOIL")]
        public DateTime CreatoIl { get; set; }
        [Column("CREATODA")]
        public string CreatoDa { get; set; }
        [Column("DATAEMISSIONE")]
        public DateTime Data { get; set; }
        [Column("TOTALEEURO")]
        public int TotaleEuro { get; set; }
        [Column("TOTALEBOLLETTE")]
        public int TotaleBollette { get; set; }
        [Column("FILEGENERATO")]
        public string FileBase64 { get; set; }

        public MemoryStream Stream { get { return new MemoryStream(Convert.FromBase64String(FileBase64)); } }
    }
}
