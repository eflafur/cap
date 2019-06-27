using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestioneRimborsi.Core;

namespace GestioneRimborsi.Core
{
    public class ClienteService : IClienteService
    {
        IClienteRepo _clienteRepo = null;

        public ClienteService(IClienteRepo ClienteRepo)
        {
            _clienteRepo = ClienteRepo;
        }
        public String ClienteByID(String CodCliente)
        {
            return _clienteRepo.ClienteByID(CodCliente);
        }
        public Cliente InfoCliente(String CodCliente)
        {
            return _clienteRepo.InfoCliente(CodCliente);
        }
        public String GetCodiceCliente (String CodiceCliente)
        {
            return _clienteRepo.GetCodiceCliente(CodiceCliente);
        }
        public RecapitoClienteRimborso InfoRecapito(String CodPuntoFornitura, String NumeroDocumento, String TipoDocumento, String CodCliente)
        {
            return _clienteRepo.InfoRecapito(CodPuntoFornitura, NumeroDocumento, TipoDocumento, CodCliente);
        }
        public ISubCollection<Cliente> FilterByTerm(String term)
        {
            return _clienteRepo.FilterByTerm(term);
        }
        public ISubCollection<InsolutoBolletta> GetInsoluti(String CodCliente, String AnnoDocumento, String NumeroDocumento, String TipoDocumento)
        {
            return _clienteRepo.GetInsoluti(CodCliente, AnnoDocumento, NumeroDocumento, TipoDocumento);
        }
        public ISubCollection<InsolutoBolletta> GetStampaInsoluti(String CodCliente, String AnnoDocumento, String NumeroDocumento, String TipoDocumento)
        {
            return _clienteRepo.GetStampaInsoluti(CodCliente, AnnoDocumento, NumeroDocumento, TipoDocumento);
        }
        public CoordinateBancarie GetDatiIBAN(String CodCliente, String CodPuntoFornitura, String NumeroIncasso, String TipoDocumento)
        {
            return _clienteRepo.GetDatiIBAN(CodCliente, CodPuntoFornitura, NumeroIncasso, TipoDocumento);
        }
        public IBAN GetIBAN(String CodCliente)
        {
            return _clienteRepo.GetIBAN(CodCliente);
        }
        public CoordinateBancarie GetIBANCliente(String CodCliente)
        {
            return _clienteRepo.GetIBANCliente(CodCliente);
        }
        public bool RegistraIBAN(String CodiceCliente, String IBAN, DateTime DataInserimento, String UtenteInserimento)
        {
            return _clienteRepo.RegistraIBAN(CodiceCliente, IBAN, DataInserimento, UtenteInserimento);
        }
    }
}
