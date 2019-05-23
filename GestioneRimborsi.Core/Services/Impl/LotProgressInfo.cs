using PetaPoco;

namespace GestioneRimborsi.Core
{
    [TableName("GRI_BI_LOTPROGRESS")]
    public class LotProgressInfo
    {
        [Column("LOTNAME")]
        public string lotName { get; set; }
        [Column("LOTID")]
        public int lotId { get; set; }
        [Column("PROGRESS")]
        public int progress { get; set; }
        [Column("Total")]
        public int total { get; set; }
    }
}