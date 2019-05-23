using GruppoCap.Core;
using GruppoCap;
using GruppoCap.Core.Data;
using GruppoCap.DAL;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core
{
    public class ClienteRepo : RepositoryBase<Cliente>, IClienteRepo
    {
        public String ClienteByID(String CodCliente)
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT DISTINCT DES_RAGIONE_SOCIALE FROM GRI_RAGIONE_SOCIALE_CLIENTI_V WHERE COD_CLIENTE = @0 AND ROWNUM = 1", CodCliente);
                return db.SingleOrDefault<String>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in ClienteByID: " + ex.Message);
            }
        }

        public Cliente InfoCliente(String CodCliente)
        {
            Cliente cliente = null;
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM {0} WHERE COD_CLIENTE = @0 AND FLG_LAST = 'Y' AND FLG_TIPO = 'I' AND ROWNUM = 1".FormatWith(EntityTableName), CodCliente);
                cliente = db.SingleOrDefault<Cliente>(sql);

                if (cliente == null)
                {
                    sql = Sql.Builder.Append("SELECT BI_CLIENTE_NEW COD_CLIENTE, BI_NOME || ' ' || BI_COGNOME DES_RAGIONE_SOCIALE, BI_REQ_CF COD_CODICE_FISCALE, comuni.DENOMINAZIONE DES_COMUNE, req.BI_CENTR_AREACIR DES_STRADA, BI_CENTR_CIVICO DES_NUMERO_CIVICO, BI_CENTR_CAP DES_CAP_SPEDIZIONE, comuni.PROVINCIA DES_PROVINCIA, BI_DATA_PRESENTAZIONE DTA_INS FROM GRI_BI_REQUEST_CAP reqc INNER JOIN GRI_BI_REQUEST req ON req.BI_REQ_ID = reqc.BI_REQ_CAP_ID LEFT JOIN aque.comuni comuni ON comuni.istat = req.bi_centr_istatcomune WHERE BI_CLIENTE_NEW = @0 AND ROWNUM = 1", CodCliente);
                    cliente = db.SingleOrDefault<Cliente>(sql);
                }

                return cliente;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in InfoCliente: " + ex.Message);
            }
        }

        public String GetCodiceCliente(String CodiceCliente)
        {
            try
            {

                var sql = Sql.Builder.Append("SELECT NVL(COD_CLIENTE, COD_CLIENTE_INTEGRA) FROM {0} WHERE COD_CLIENTE_INTEGRA = @0 AND FLG_LAST = 'Y' AND FLG_TIPO = 'I' ".FormatWith(EntityTableName), CodiceCliente);
                return db.SingleOrDefault<String>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCodiceCliente: " + ex.Message);
            }
        }

        public RecapitoClienteRimborso InfoRecapito(String CodPuntoFornitura, String NumeroDocumento, String TipoDocumento, String CodCliente)
        {
            try
            {
                if (TipoDocumento != "Pag. Ecc.")
                {
                    var sql = Sql.Builder.Append("SELECT * FROM GRI_INDIRIZZO_RECAPITO_V WHERE COD_CLIENTE = @0 AND ID_INCASSO = 0", CodCliente);

                    if (CodPuntoFornitura != null)
                        sql.Append(" AND COD_PUF = @0", CodPuntoFornitura);

                    sql.Append(" AND ROWNUM = 1");
                    return db.SingleOrDefault<RecapitoClienteRimborso>(sql);
                }
                else
                {
                    var sql = Sql.Builder.Append("SELECT * FROM GRI_INDIRIZZO_RECAPITO_V WHERE ID_INCASSO = @0 AND ROWNUM = 1 ", NumeroDocumento);
                    RecapitoClienteRimborso result = db.SingleOrDefault<RecapitoClienteRimborso>(sql);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in InfoRecapito: " + ex.Message);
            }
        }

        public ISubCollection<Cliente> FilterByTerm(String term)
        {
            ISubCollection<Cliente> _list;
            try
            {
                var sql = Sql.Builder.Append("");
                if (String.IsNullOrEmpty(term))
                    sql = Sql.Builder.Append("SELECT DES_RAGIONE_SOCIALE, COD_CLIENTE, COD_PARTITA_IVA, COD_CODICE_FISCALE FROM {0} WHERE 1=0".FormatWith(EntityTableName));
                else
                    sql = Sql.Builder.Append(@"SELECT DES_RAGIONE_SOCIALE, COD_CLIENTE, COD_PARTITA_IVA, COD_CODICE_FISCALE FROM
                                              (SELECT DES_RAGIONE_SOCIALE, COD_CLIENTE, COD_PARTITA_IVA, COD_CODICE_FISCALE FROM {0}".FormatWith(EntityTableName))
                                     .Append(" WHERE (COD_CLIENTE LIKE @0 OR UPPER(DES_RAGIONE_SOCIALE) LIKE @0 OR UPPER(COD_CODICE_FISCALE) LIKE @0 OR UPPER(COD_PARTITA_IVA) LIKE @0) AND FLG_LAST = 'Y' AND FLG_TIPO = 'I' ", "%" + term.ToUpper() + "%")
                                     .Append(" UNION ALL SELECT BI_NOME || ' ' || BI_COGNOME DES_RAGIONE_SOCIALE, BI_CLIENTE_NEW COD_CLIENTE, NULL PARTITA_IVA, BI_REQ_CF COD_CODICE_FISCALE FROM GRI_BI_REQUEST_CAP reqc INNER JOIN GRI_BI_REQUEST req ON req.BI_REQ_ID = reqc.BI_REQ_CAP_ID ")
                                     .Append(" WHERE (BI_CLIENTE_NEW LIKE @0 OR UPPER(BI_NOME || ' ' || BI_COGNOME) LIKE @0 OR UPPER(BI_REQ_CF) LIKE @0)) WHERE ROWNUM  <= 100", "%" + term.ToUpper() + "%");
                _list = db.Query<Cliente>(sql).ToSubCollection<Cliente>();

                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in FilterByTerm: " + ex.Message);
            }
        }

        public ISubCollection<InsolutoBolletta> GetStampaInsoluti(String CodCliente, String AnnoDocumento, String NumeroDocumento, String TipoDocumento)
        {
            ISubCollection<InsolutoBolletta> _list;
            List<InsolutoBolletta> _listaSvuotata = new List<InsolutoBolletta>();
            try
            {
                var sql = Sql.Builder.Append("select * from GRI_INSOLUTI_V WHERE COD_CLIENTE_ODL = @0", CodCliente);
                _list = db.Query<InsolutoBolletta>(sql).ToSubCollection<InsolutoBolletta>();
                var codBolletta = "";

                foreach (var item in _list.Items)
                {
                    if (item.CodiceBolletta == codBolletta)
                    {
                        _listaSvuotata.Add(item);
                    }
                    codBolletta = item.CodiceBolletta;
                }

                foreach (var item in _listaSvuotata)
                {
                    _list.Items.Remove(item);
                }

                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetStampaInsoluti: " + ex.Message);
            }
        }

        public ISubCollection<InsolutoBolletta> GetInsoluti(String CodCliente, String AnnoDocumento, String NumeroDocumento, String TipoDocumento)
        {
            ISubCollection<InsolutoBolletta> _list;
            List<InsolutoBolletta> _listaSvuotata = new List<InsolutoBolletta>();
            try
            {
                var sql = Sql.Builder.Append("select * from GRI_INSOLUTI_V WHERE COD_CLIENTE_ODL = @0", CodCliente);
                _list = db.Query<InsolutoBolletta>(sql).ToSubCollection<InsolutoBolletta>();
                var codBolletta = "";
                var statoDocumento = "";
                InsolutoBolletta partialItem = new InsolutoBolletta();

                foreach (var item in _list.Items)
                {
                    if (!String.IsNullOrEmpty(item.NumeroDocumento))
                    {
                        if (item.NumeroDocumento != NumeroDocumento)
                        {
                            if (item.CodiceBolletta == codBolletta)
                            {
                                _listaSvuotata.Add(item);
                                var getStato = Sql.Builder.Append("SELECT stato_documento FROM ut_rimb_test WHERE anno_documento = @0 AND numero_documento = @1", item.AnnoDocumento, item.NumeroDocumento);
                                statoDocumento = db.SingleOrDefault<String>(getStato);
                                if (statoDocumento != "2" && partialItem.CodiceBolletta == codBolletta)
                                    _listaSvuotata.Add(partialItem);
                            }
                            else
                            {
                                var getStato = Sql.Builder.Append("SELECT stato_documento FROM ut_rimb_test WHERE anno_documento = @0 AND numero_documento = @1", item.AnnoDocumento, item.NumeroDocumento);
                                statoDocumento = db.SingleOrDefault<String>(getStato);
                                if (statoDocumento != "2")
                                    _listaSvuotata.Add(item);
                                else
                                {
                                    item.Gestito = 0;
                                    item.ImportoDaCompensare = 0;
                                    partialItem = item;
                                }
                            }
                        }
                        else if (partialItem.CodiceBolletta == item.CodiceBolletta)
                        {
                            if (!_listaSvuotata.Contains(partialItem))
                                _listaSvuotata.Add(partialItem);
                        }
                    }
                    codBolletta = item.CodiceBolletta;
                }

                foreach (var item in _listaSvuotata)
                    _list.Items.Remove(item);

                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetInsoluti: " + ex.Message);
            }
        }

        public CoordinateBancarie GetDatiIBAN(String CodCliente, String CodPuntoFornitura, String NumeroIncasso, String TipoDocumento)
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GRI_COORDINATE_BANCARIE_V WHERE CODICE_CLIENTE = @0 ", CodCliente);
                CoordinateBancarie coordinate = db.FirstOrDefault<CoordinateBancarie>(sql);
                return coordinate;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetDatiIBAN: " + ex.Message);
            }
        }

        public IBAN GetIBAN(String CodCliente)
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM (SELECT * FROM GRI_IBAN WHERE CODICE_CLIENTE = @0 ORDER BY DATA_INSERIMENTO DESC) WHERE ROWNUM = 1", CodCliente);
                return db.SingleOrDefault<IBAN>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIBAN: " + ex.Message);
            }
        }

        public CoordinateBancarie GetIBANCliente(String CodCliente)
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GRI_COORDINATE_BANCARIE_V WHERE CODICE_CLIENTE = @0 ", CodCliente);
                CoordinateBancarie coordinate = db.FirstOrDefault<CoordinateBancarie>(sql);
                return coordinate;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIBANCliente: " + ex.Message);
            }
        }

        public bool RegistraIBAN(String CodiceCliente, String IBAN, DateTime DataInserimento, String UtenteInserimento)
        {
            try
            {
                db.BeginTransaction();
                var sql = Sql.Builder.Append("INSERT INTO GRI_IBAN (CODICE_CLIENTE, IBAN, DATA_INSERIMENTO, UTENTE_INSERIMENTO) VALUES (@0,@1,@2,@3)", CodiceCliente, IBAN, DateTime.Now, UtenteInserimento);
                db.Execute(sql);

                db.CompleteTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                return false;
            }
        }

    }
}
