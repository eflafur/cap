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
    [TableName("GRI_BI_LOTS")]    
    public class Lotto : IEntity
    {
        [Column("BI_LOT_ID")]
        public String Id { get; set; }

        [Column("BI_LOT_DES")]
        public String Desc { get; set; }

        [Column("BI_LOT_CREATED_BY")]
        public string CreateByName { get; set; }

        [Column("BI_LOT_CREATION_MOMENT")]
        public DateTime DateCreation { get; set; }

        [Column("BI_MODIFIED_BY")]
        public string ModifiedBy { get; set; }

        [Column("BI_MODIFIED_ON")]
        public DateTime DateModified { get; set; }

        [Ignore]
        public object EntityId
        {
            get { return this.Id; }
        }
        [Ignore]
        public string DisplayText
        {
            get { return string.Format("Utente {1} - RagioneSociale : {0}", this.CreateByName, this.Desc); }
        }
    }
}

//    [TableName("GRI_BI_TEST")]
//    public class Lotto : IEntity
//    {
//        [Column("BI_REQ_ID")]
//        public int Id { get; set; }

//        [Column("BI_REQ_NAME")]
//        public string Name { get; set; }

//        [Column("BI_REQ_CITTA")]
//        public string Citta { get; set; }
//        [Column("BI_REQ_ETA")]
//        public int Eta { get; set; }
//        [Column("BI_REQ_DATA")]
//        public DateTime Data { get; set; }

//        [Ignore]
//        public object EntityId
//        {
//            get { return this.Id; }
//        }
//        [Ignore]
//        public string DisplayText
//        {
//            get { return string.Format("Utente {1} - RagioneSociale : {0}", this.Citta, this.Data); }
//        }
//    }
//}


