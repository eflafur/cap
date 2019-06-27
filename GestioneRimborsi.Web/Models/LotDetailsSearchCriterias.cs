using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestioneRimborsi.Web.Models
{
    public class LotDetailsSearchCriterias
    {
        /// <summary>
        /// Identifica lo stato di una richiesta
        /// </summary>
        /// <remarks>
        /// Valori possibili:
        /// <list type="bullet">
        /// <item>0 => Tutte le richieste</item>
        /// <item>1 => Richieste Processate</item>
        /// <item>2 => Richieste Non processate</item>
        /// </list>
        /// </remarks>
        public Int16 RequestStatus { get; set; }
        /// <summary>
        /// Identifica lo stato di una utenza
        /// </summary>
        /// <remarks>
        /// Valori possibili:
        /// <list type="bullet">
        /// <item>0 => Tutte le richieste</item>
        /// <item>1 => Richieste Individuali</item>
        /// <item>2 => Richieste Centralizzate</item>
        /// </list>
        /// </remarks>
        public Int16 CustomerType { get; set; }

        /// <summary>
        /// Corrisponde all'esito della richiesta
        /// </summary>
        public string Outcome { get; set; }

        /// <summary>
        /// Corrisponde ad un Identificativo richiesta
        /// </summary>
        public int RequestId { get; set; }
    }

    /// <summary>
    /// Oggetto che contiene i risultati di una ricerca eseguita lato server.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchResults<T>
    {

        /// <summary>
        /// E' il numero di record prodotti dalla ricerca
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// E' il numero di record contenuti in una pagina del risultato
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// E' il numero di pagine totali
        /// </summary>
        public int PageCount { get {
                if(RecordCount % PageSize>0)
                return RecordCount / PageSize+1;
                return RecordCount / PageSize;

            }
        }

        public int PageIndex { get; set; }

        public List<T> Results { get; set; }

    }
}