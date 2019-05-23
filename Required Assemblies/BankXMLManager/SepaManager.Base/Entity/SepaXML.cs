using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SepaManager.Base.Entity
{
    [TableName("SEPAXML")]
    [PrimaryKey("Id", autoIncrement = true, sequenceName = "SEPAXML_SEQ")]
    public class SepaXML
    {
        public Int64 ID { get; set; }
        [Column("ViewName")]
        public string FileName { get; set; }
        public Int64 SepaHeaderID { get; set; }
        public string XmlFile { get; set; }
        public int State { get; set; }
        public DateTime Created { get; set; }
    }
}