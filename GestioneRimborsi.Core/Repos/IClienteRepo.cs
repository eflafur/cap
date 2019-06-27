using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface IClienteRepo : IRepository<Cliente>
    {
        String ClienteByID(String CodCliente);
        Cliente InfoCliente(String CodCliente);
        String GetCodiceCliente(String CodiceCliente);
        RecapitoClienteRimborso InfoRecapito(String CodPuntoFornitura, String NumeroDocumento, String TipoDocumento, String CodCliente);
        ISubCollection<Cliente> FilterByTerm(String term);
        ISubCollection<InsolutoBolletta> GetInsoluti(String CodCliente, String AnnoDocumento, String NumeroDocumento, String TipoDocumento);
        ISubCollection<InsolutoBolletta> GetStampaInsoluti(String CodCliente, String AnnoDocumento, String NumeroDocumento, String TipoDocumento);
        CoordinateBancarie GetDatiIBAN(String CodCliente, String CodPuntoFornitura, String NumeroIncasso, String TipoDocumento);
        IBAN GetIBAN(String CodCliente);
        CoordinateBancarie GetIBANCliente(String CodCliente);
        bool RegistraIBAN(String CodiceCliente, String IBAN, DateTime DataInserimento, String UtenteInserimento);
    }
}
