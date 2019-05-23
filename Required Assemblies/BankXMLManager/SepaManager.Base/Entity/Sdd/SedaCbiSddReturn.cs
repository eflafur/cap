using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SepaManager.Base.Entity.Sdd
{
    /// <summary>
    /// Classe contenente il risultato dell'elaborazione del worker SepaSddWorkerManager
    /// </summary>
    public class SedaCbiSddReturn
    {
        /// <summary>
        /// Risultato della generazione del file. Se FALSE il campo Stream non sarà valorizzato e LastError si. Se TRUE sarà il contrario
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// Eventuali messaggi da passare al client
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Eventuale errore
        /// </summary>
        public string LastError { get; set; }
        /// <summary>
        /// Eventuale file generato. 
        /// </summary>
        public MemoryStream Stream { get; set; }
        /// <summary>
        /// Estensione del file eventualmente generato
        /// </summary>
        public string FileExtension { get; set; }
    }
}
