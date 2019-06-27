using GruppoCap.Core;
using GruppoCap.Core.Data;
using GruppoCap.DAL;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap;

namespace GestioneRimborsi.Core
{
    public class RimborsoRepo : RepositoryBase<GestioneRimborso>, IRimborsoRepo
    {
        public GestioneRimborso GetRimborso(String CodCliente, String Utente, Int16 Anno, String NumeroDocumento)
        {
            var list = BaseElencoRimborsi(CodCliente, Utente, Anno, NumeroDocumento);
            if (list.Items.Count > 0)
                return list.Items.First();
            else
                return null;
        }

        private ISubCollection<GestioneRimborso> BaseElencoRimborsi(String CodCliente, String Utente)
        {
            return BaseElencoRimborsi(CodCliente, Utente, 0, "");
        }
        private ISubCollection<GestioneRimborso> BaseElencoRimborsi(String CodCliente, String Utente, Int16 Anno, String NumeroDocumento)
        {
            ISubCollection<GestioneRimborso> _list = null;
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GRI_RIMBORSI_APERTI_V WHERE CODICE_CLIENTE = @0 AND (ANNO_DOCUMENTO = @1 OR @1=0) AND (NUMERO_DOCUMENTO = @2 OR @2 IS NULL)", CodCliente, Anno, NumeroDocumento);
                _list = db.Query<GestioneRimborso>(sql).ToSubCollection<GestioneRimborso>();
            }

            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in BaseElencoRimborsi: " + ex.Message);
            }
            return _list;
        }

        public ISubCollection<GestioneRimborso> GetElencoRimborsi(String CodCliente, String Utente)
        {
            return BaseElencoRimborsi(CodCliente, Utente);
        }

        public ISubCollection<GestioneRimborso> GetRimborsiFiltered(String CodCliente, String Utente, List<String> Permission)
        {
            ISubCollection<GestioneRimborso> _list = null;
            try
            {
                if (Permission.FirstOrDefault() == "Privileged")
                    return BaseElencoRimborsi(CodCliente, Utente);

                var sql = Sql.Builder.Append(string.Format("select * from gri_rimborsi_aperti_v where codice_cliente = @0 and TIPO_DOCUMENTO in (select desc_rimborso from GRI_CAPGROUPING_ON_RIMBORSI where capgrouping_code in ({0})) ", string.Join(",", Permission.Select(ee => string.Format("'{0}'", ee)).ToArray())), CodCliente);
                _list = db.Query<GestioneRimborso>(sql).ToSubCollection<GestioneRimborso>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRimborsiFiltered:" + ex.Message);
            }
            return _list;
        }

        public String GetCodicePuntoFornitura(String CodiceCliente, String NumeroDocumento, String AnnoDocumento)
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT CODICE_PUNTO_FORNITURA FROM GRI_RIMBORSI_APERTI_V WHERE ANNO_DOCUMENTO = @0 AND NUMERO_DOCUMENTO = @1 AND CODICE_CLIENTE = @2", AnnoDocumento, NumeroDocumento, CodiceCliente);
                String result = db.SingleOrDefault<String>(sql);
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCodicePuntoFornitura:" + ex.Message);
            }
        }

        public GestioneRimborso GetRimborsoTestata(String AnnoDocumento, String NumeroDocumento)
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM UT_RIMB_TEST WHERE ANNO_DOCUMENTO = @0 AND NUMERO_DOCUMENTO = @1", AnnoDocumento, NumeroDocumento);
                return db.SingleOrDefault<GestioneRimborso>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRimborsoTestata:" + ex.Message);
            }
        }

        public ISubCollection<GestioneRimborso> GetTestataBozzaRimborsi(List<string> ClienteAnnoNumeroDocumento, string Utente)
        {
            try
            {
                ISubCollection<GestioneRimborso> _list;

                var sql = Sql.Builder.Append(" SELECT DISTINCT * FROM GRI_RIMBORSI_TEST_V WHERE STATO_DOCUMENTO = 1");
                if (ClienteAnnoNumeroDocumento.Count != 0)
                {
                    int contatore = 0;
                    sql.Append(" AND (");
                    foreach (var item in ClienteAnnoNumeroDocumento)
                    {
                        var items = item.Split(';');
                        contatore++;
                        sql.Append(" (DESC_RIMBORSO = @0 AND ANNO_DOCUMENTO = @1 AND NUMERO_DOCUMENTO = @2)", items[0].ToString(), items[1].ToString(), items[2].ToString());
                        if (ClienteAnnoNumeroDocumento.Count > contatore)
                            sql.Append(" OR");
                    }
                    sql.Append(" )");
                }

                _list = db.Query<GestioneRimborso>(sql).ToSubCollection<GestioneRimborso>();

                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetTestataBozzaRimborsi: " + ex.Message);
            }
        }

        public ISubCollection<DettaglioRimborso> GetRimborsoDettaglio(String AnnoDocumento, String NumeroDocumento)
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GRI_RIMBORSI_DETT_V WHERE ANNO_DOCUMENTO = @0 AND NUMERO_DOCUMENTO = @1", AnnoDocumento, NumeroDocumento);
                return db.Query<DettaglioRimborso>(sql).ToSubCollection<DettaglioRimborso>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRimborsoDettaglio: " + ex.Message);
            }
        }

        public ISubCollection<GestioneRimborso> GetRimborsiConfermabili(String Utente, bool Amministratore)
        {
            ISubCollection<GestioneRimborso> _list;
            try
            {
                if (Amministratore == true)
                {
                    //var sql = Sql.Builder.Append("SELECT * FROM GRI_RIMBORSI_IN_CORSO_V WHERE UTENTE_INSERIMENTO = 'cristina.tosello'");
                    var sql = Sql.Builder.Append("SELECT * FROM GRI_RIMBORSI_IN_CORSO_V");
                    _list = db.Query<GestioneRimborso>(sql).ToSubCollection<GestioneRimborso>();
                }
                else
                {
                    var sql = Sql.Builder.Append("SELECT * FROM GRI_RIMBORSI_IN_CORSO_V WHERE UTENTE_INSERIMENTO = @0", Utente);
                    _list = db.Query<GestioneRimborso>(sql).ToSubCollection<GestioneRimborso>();
                }
                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRimborsiConfermabili: " + ex.Message);
            }
        }

        List<string> IRimborsoRepo.UsersOfRimborsi()
        {
            var sql = Sql.Builder.Append("SELECT DISTINCT UTENTE_INSERIMENTO FROM UT_RIMB_TEST");
            return db.Query<string>(sql).ToList();
        }

        public ISubCollection<GestioneRimborso> GetRimborsiAnnullabili(String Utente, bool Amministratore)
        {
            ISubCollection<GestioneRimborso> _list;
            try
            {
                if (Amministratore == true)
                {
                    var sql = Sql.Builder.Append("SELECT * FROM GRI_RIMBORSI_IN_CORSO_V");
                    _list = db.Query<GestioneRimborso>(sql).ToSubCollection<GestioneRimborso>();
                }
                else
                {
                    var sql = Sql.Builder.Append("SELECT * FROM GRI_RIMBORSI_IN_CORSO_V WHERE UTENTE_INSERIMENTO = @0", Utente);
                    _list = db.Query<GestioneRimborso>(sql).ToSubCollection<GestioneRimborso>();
                }
                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRimborsiAnnullabili: " + ex.Message);
            }
        }

        public ISubCollection<GestioneRimborso> GetRimborsiConfermati(String CodCliente, String Utente)
        {
            ISubCollection<GestioneRimborso> _list;
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GRI_RIMBORSI_CHIUSI_V WHERE CODICE_CLIENTE = @0", CodCliente);
                _list = db.Query<GestioneRimborso>(sql).ToSubCollection<GestioneRimborso>();
                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRimborsiConfermati: " + ex.Message);
            }
        }

        public bool AnnullaRimborso(List<String> ClienteAnnoNumeroDocumento, String Utente)
        {
            try
            {
                db.BeginTransaction();

                foreach (var item in ClienteAnnoNumeroDocumento)
                {
                    var items = item.Split(';');
                    var rimborsoTest = GetRimborso(items[2].ToString(), Utente, Convert.ToInt16(items[1]), items[0].ToString());
                    if (rimborsoTest != null)
                    {
                        var sqlDelRimbTest = Sql.Builder.Append("DELETE FROM UT_RIMB_TEST WHERE ANNO_DOCUMENTO = @0 AND NUMERO_DOCUMENTO = @1 ", items[1].ToString(), items[0].ToString());
                        db.Execute(sqlDelRimbTest);
                        var sqlDelRimbDett = Sql.Builder.Append("DELETE FROM UT_RIMB_DETT WHERE ANNO_DOCUMENTO = @0 AND NUMERO_DOCUMENTO = @1 ", items[1].ToString(), items[0].ToString());
                        db.Execute(sqlDelRimbDett);
                    }
                }

                db.CompleteTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                return false;
            }
        }

        public string InserisciRimborso(GestioneRimborso RimborsoNativo, RimborsoGestito RimborsoGestito)
        {
            try
            {
                db.BeginTransaction();

                var sqlRimbTest = Sql.Builder.Append("DELETE FROM UT_RIMB_TEST WHERE ANNO_DOCUMENTO = @0 AND NUMERO_DOCUMENTO = @1 AND CODICE_CLIENTE = @2", RimborsoGestito.AnnoDocumento, RimborsoGestito.NumeroDocumento, RimborsoGestito.CodiceCliente);
                db.Execute(sqlRimbTest);

                var sqlRimbDett = Sql.Builder.Append("DELETE FROM UT_RIMB_DETT WHERE ANNO_DOCUMENTO = @0 AND NUMERO_DOCUMENTO = @1", RimborsoGestito.AnnoDocumento, RimborsoGestito.NumeroDocumento);
                db.Execute(sqlRimbDett);

                DateTime Moment = System.DateTime.Now;
                var Rimborso = new TestataRimborso()
                {
                    AnnoDocumento = RimborsoGestito.AnnoDocumento,
                    NumeroDocumento = RimborsoGestito.NumeroDocumento,
                    TipoDocumento = DecodeTipoDocumento(RimborsoGestito.TipoDocumento),
                    StatoDocumento = "1",
                    ImportoTotaleRimborso = RimborsoGestito.RigheRimborso.Sum(X => X.Importo),
                    CodiceCliente = RimborsoGestito.CodiceCliente,
                    TipoRimborso = RimborsoGestito.TipoRimborso,
                    CodicePuntoFornitura = RimborsoGestito.CodicePuntoFornitura,
                    Beneficiario = RimborsoGestito.Beneficiario,
                    ABI = RimborsoGestito.IBAN != null ? RimborsoGestito.IBAN.Substring(5, 5) : null,
                    CAB = RimborsoGestito.IBAN != null ? RimborsoGestito.IBAN.Substring(10, 5) : null,
                    ContoCorrente = RimborsoGestito.IBAN != null ? RimborsoGestito.IBAN.Substring(15) : null,
                    CIN = RimborsoGestito.IBAN != null ? RimborsoGestito.IBAN.Substring(4, 1) : null,
                    IndirizzoAssegno = (RimborsoGestito.TipoRimborso == "BOD" ? String.Format("{0} {1}", RimborsoGestito.IndirizzoAlt, RimborsoGestito.CivicoAlt) : RimborsoGestito.IndirizzoAssegno),
                    LocalitaAssegno = (RimborsoGestito.TipoRimborso == "BOD" ? RimborsoGestito.LocalitaAlt : RimborsoGestito.LocalitaAssegno),
                    CapAssegno = (RimborsoGestito.TipoRimborso == "BOD" ? RimborsoGestito.CAPAlt : RimborsoGestito.CAPAssegno),
                    ProvinciaAssegno = (RimborsoGestito.TipoRimborso == "BOD" ? RimborsoGestito.ProvinciaAlt : RimborsoGestito.ProvinciaAssegno),
                    Intestazione = RimborsoGestito.IntestazioneAlt,
                    Strada = RimborsoGestito.IndirizzoAlt,
                    Civico = RimborsoGestito.CivicoAlt,
                    Localita = RimborsoGestito.LocalitaAlt,
                    CAP = RimborsoGestito.CAPAlt,
                    Provincia = RimborsoGestito.ProvinciaAlt,
                    DataInserimento = DateTime.Now,
                    UtenteInserimento = RimborsoGestito.UtenteInserimento,
                    CodiceAzienda = RimborsoGestito.CodiceAzienda,
                    FlagGenerazioneFile = "N",
                    IDSafo = RimborsoNativo.IDSafo
                };

                db.Insert(Rimborso);

                RimborsoGestito.RigheRimborso.ForEach(r =>
                {
                    var sqlInsertDett = Sql.Builder.Append("INSERT INTO UT_RIMB_DETT ( ANNO_DOCUMENTO, NUMERO_DOCUMENTO, TIPO_DOCUMENTO, TIPO_RIMBORSO, IMPORTO, NUMERO_BOLLETTA, COD_AZIENDA)");
                    sqlInsertDett.Append("VALUES (@0,@1,@2,@3,@4,@5,@6)", RimborsoGestito.AnnoDocumento, RimborsoGestito.NumeroDocumento, DecodeTipoDocumento(RimborsoGestito.TipoDocumento),
                        r.TipoRimborso, r.Importo, r.CodiceBolletta, RimborsoGestito.CodiceAzienda);
                    db.Execute(sqlInsertDett);

                });

                if (RimborsoGestito.TipoRimborso == "BON" && RimborsoGestito.IBAN != null)
                {
                    var sqlInsertIBAN = Sql.Builder.Append("INSERT INTO GRI_IBAN (CODICE_CLIENTE, IBAN, DATA_INSERIMENTO, UTENTE_INSERIMENTO) VALUES (@0,@1,@2,@3)", RimborsoGestito.CodiceCliente, RimborsoGestito.IBAN, DateTime.Now, RimborsoGestito.UtenteInserimento);
                    db.Execute(sqlInsertIBAN);
                }

                db.CompleteTransaction();
            }
            catch (Exception Ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in InserisciRimborso: " + Ex.Message);
            }
            return null;
        }



        public string RegistraRimborso(GestioneRimborso RimborsoNativo,
            RimborsoGestito RimborsoGestito,
            RecapitoClienteRimborso Cliente)
        { return null; }

        public ISubCollection<GestioneRimborso> GetRimborsiTestataByClienteAnnoNumeroDocumento(List<string> ClienteAnnoNumeroDocumento, string Utente)
        {
            try
            {
                ISubCollection<GestioneRimborso> _list;

                var sql = Sql.Builder.Append(" SELECT DISTINCT * FROM GRI_DESTINATARI_V WHERE STATO_DOCUMENTO = 2 ");
                if (ClienteAnnoNumeroDocumento.Count != 0)
                {
                    int contatore = 0;
                    sql.Append(" AND (");
                    foreach (var item in ClienteAnnoNumeroDocumento)
                    {
                        var items = item.Split(';');
                        contatore++;
                        sql.Append(" (DESC_RIMBORSO = @0 AND ANNO_DOCUMENTO = @1 AND NUMERO_DOCUMENTO = @2)", items[0].ToString(), items[1].ToString(), items[2].ToString());
                        if (ClienteAnnoNumeroDocumento.Count > contatore)
                            sql.Append(" OR");
                    }
                    sql.Append(" )");
                }
                

                _list = db.Query<GestioneRimborso>(sql).ToSubCollection<GestioneRimborso>();

                if (_list != null)
                {
                    foreach (var item in _list.Items)
                    {
                        if (item.TipoDocumento == "BONU")
                        {
                            var dataBonus = Sql.Builder.Append("SELECT DATA_INS FROM BONUSIDRICO_RIMBORSI WHERE CODICEBONUS = @0", item.NumeroDocumento);
                            item.DataEmissione = db.FirstOrDefault<DateTime>(dataBonus);
                        }

                    }
                }

                return _list;
            }
            catch (Exception Ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRimborsiTestataByClienteAnnoNumeroDocumento: " + Ex.Message);
            }
        }

        public String ConfermaRimborsi(List<String> ClienteAnnoNumeroDocumento, String Utente, String NumeroProtocollo, String UtenteProtocollo, String DataProtocollo)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                db.BeginTransaction();
                foreach (var item in ClienteAnnoNumeroDocumento)
                {
                    var items = item.Split(';');

                    String pAzienda = GetRimborsoTestata(items[1].ToString(), items[2].ToString()).CodiceAzienda;

                    PetaPocoParameter[] conferma = { new PetaPocoInputParameter("pAzienda", System.Data.DbType.String, pAzienda),
                                                     new PetaPocoInputParameter("pAnnoDoc", System.Data.DbType.String, items[1].ToString()),
                                                     new PetaPocoInputParameter("pNumeroDoc", System.Data.DbType.String, items[2].ToString()),
                                                     new PetaPocoInputParameter("pTipoDoc", System.Data.DbType.String, DecodeTipoDocumento(items[3].ToString())),
                                                     new PetaPocoInputParameter("pCliente", System.Data.DbType.String, items[0].ToString()),
                                                     new PetaPocoInputParameter("pUtente", System.Data.DbType.String, Utente),
                                                     new PetaPocoInputParameter("pUsr_Prot", System.Data.DbType.String, UtenteProtocollo),
                                                     new PetaPocoInputParameter("pNum_Prot", System.Data.DbType.String, NumeroProtocollo),
                                                     new PetaPocoInputParameter("pData_Prot", System.Data.DbType.String, DataProtocollo),
                                                     new PetaPocoOutputParameter("RES", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("MSG", System.Data.DbType.String, 2500) 
                                                   };


                    var resultConferma = db.ExecuteProcedure("PKG_CONTABILIZZA_RIMBORSI.CONFERMA_RIMBORSO", conferma);
                    /**********************/
                    if (resultConferma == null)
                    {
                        throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                    }
                    if (resultConferma.Result)
                        if (resultConferma.OutputParams != null)
                        {
                            PetaPocoOutputParameter _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "RES", StringComparison.CurrentCultureIgnoreCase));
                            if (_p != null)
                            {
                                if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                                {
                                    _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "MSG", StringComparison.CurrentCultureIgnoreCase));
                                    if (_p != null)
                                    {
                                        throw new ApplicationException(_p.Value.ToString());
                                    }
                                    else
                                    {
                                        throw new ApplicationException("Valore di Output non trovato");
                                    }
                                }
                            }
                            else
                            {
                                throw new ApplicationException("2. Errore generico durante l'esecuzione della procedura");
                            }
                        }
                        else
                        {
                            throw new ApplicationException("4. Errore generico durante l'esecuzione della procedura. Nessun parametro di output");
                        }
                    else
                    {
                        throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultConferma.ErrorMessage));
                    }
                    /**********************/

                    PetaPocoParameter[] contabilizza = { new PetaPocoInputParameter("pAnnoDoc", System.Data.DbType.String, items[1].ToString()),
                                                           new PetaPocoInputParameter("pNumeroDoc", System.Data.DbType.String, items[2].ToString()),
                                                           new PetaPocoInputParameter("pTipoDoc", System.Data.DbType.String, DecodeTipoDocumento(items[3].ToString())),
                                                           new PetaPocoInputParameter("pUtente", System.Data.DbType.String, Utente),
                                                           new PetaPocoOutputParameter("pRetc", System.Data.DbType.Int32),
                                                           new PetaPocoOutputParameter("pMessaggio", System.Data.DbType.String,2500) 
                                                       };


                    var resultContabilizza = db.ExecuteProcedure("PKG_CONTABILIZZA_RIMBORSI.CONTABILIZZA_RIMBORSO", contabilizza);

                    /**********************/
                    if (resultContabilizza == null)
                    {
                        throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                    }
                    if (resultContabilizza.Result)
                        if (resultContabilizza.OutputParams != null)
                        {
                            PetaPocoOutputParameter _p = resultContabilizza.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "pRetc", StringComparison.CurrentCultureIgnoreCase));
                            if (_p != null)
                            {
                                if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                                {
                                    _p = resultContabilizza.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "pMessaggio", StringComparison.CurrentCultureIgnoreCase));
                                    if (_p != null)
                                    {
                                        throw new ApplicationException(_p.Value.ToString());
                                    }
                                    else
                                    {
                                        throw new ApplicationException("Valore di Output non trovato");
                                    }
                                }
                            }
                            else
                            {
                                throw new ApplicationException("2. Errore generico durante l'esecuzione della procedura");
                            }
                        }
                        else
                        {
                            throw new ApplicationException("4. Errore generico durante l'esecuzione della procedura. Nessun parametro di output");
                        }
                    else
                    {
                        throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultContabilizza.ErrorMessage));
                    }
                    /**********************/
                }
            }
            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in ConfermaRimborsi: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;
        }

        public ISubCollection<GestioneRimborso> GetRimborsiRistampaMassiva(DateTime DataInizio, DateTime DataFine, String Utente)
        {
            ISubCollection<GestioneRimborso> _list;
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GRI_RIMBORSI_CHIUSI_V WHERE DATA_CONFERMA >= @0 AND DATA_CONFERMA <= @1", DataInizio, DataFine);
                _list = db.Query<GestioneRimborso>(sql).ToSubCollection<GestioneRimborso>();
                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRimborsiRistampaMassiva: " + ex.Message);
            }
        }

        public String AggiungiFile(System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String AnnoDocumento, String NumeroDocumento, String FileDescription, String Utente)
        {
            try
            {
                db.BeginTransaction();

                //select PROGRESSIVO con SELECT MAX
                var sqlProgressivo = Sql.Builder.Append("SELECT nvl(max(Progressivo),0)+1 AS PROGRESSIVO FROM GRI_RIMB_DOC WHERE ANNO_DOCUMENTO = @0 AND NUMERO_DOCUMENTO = @1", AnnoDocumento, NumeroDocumento);
                var Progressivo = db.SingleOrDefault<Int16>(sqlProgressivo);

                String NomeFile = String.Format("{0}_{1}_{2}", AnnoDocumento, NumeroDocumento, Progressivo);
                var sqlRimbDoc = Sql.Builder.Append("INSERT INTO GRI_RIMB_DOC(ANNO_DOCUMENTO,NUMERO_DOCUMENTO,PROGRESSIVO,NOME_FILE,ESTENSIONE,DIMENSIONE,NOTE,DATA_INSERIMENTO,UTENTE_INSERIMENTO)")
                .Append(" VALUES (@0,@1,@2,@3,@4,@5,@6,@7,@8)", AnnoDocumento, NumeroDocumento, Progressivo, NomeFile, Extension, file.Length, FileDescription, DateTime.Now, Utente);
                db.Execute(sqlRimbDoc);


                string percorso = ServerPath + NomeFile + Extension;
                byte[] bytesInStream = new byte[file.Length];
                file.Read(bytesInStream, 0, bytesInStream.Length);

                if (System.IO.File.Exists(percorso + NomeFile + Extension))
                {
                    System.IO.File.Delete(percorso + NomeFile + Extension);
                }
                var sr1 = new System.IO.FileStream(percorso, System.IO.FileMode.Create);
                sr1.Write(bytesInStream, 0, bytesInStream.Length);
                sr1.Close();
                sr1.Dispose();

                db.CompleteTransaction();
                return String.Empty;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in AggiungiFile: " + ex.Message);
            }
        }

        public bool DeleteFile(String NomeFile, String ServerPath, String TipoFile)
        {
            try
            {
                db.BeginTransaction();

                var sql = Sql.Builder.Append("DELETE FROM GRI_RIMB_DOC WHERE NOME_FILE = @0", NomeFile);
                db.Execute(sql);

                System.IO.File.Delete(ServerPath + NomeFile + TipoFile);

                db.CompleteTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                return false;
            }
        }

        public ISubCollection<AllegatoRimborso> GetElencoDocumenti(String AnnoDocumento, String NumeroDocumento)
        {
            ISubCollection<AllegatoRimborso> _list;
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GRI_RIMB_DOC WHERE ANNO_DOCUMENTO=@0 AND NUMERO_DOCUMENTO=@1", AnnoDocumento, NumeroDocumento);
                _list = db.Query<AllegatoRimborso>(sql).ToSubCollection<AllegatoRimborso>();
                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetElencoDocumenti: " + ex.Message);
            }
        }
        public static string DecodeTipoDocumento(string TipoDocumento)
        {
            if (TipoDocumento == "Pag. Ecc." || TipoDocumento == "PGEN")
                return "PGEN";
            else if (TipoDocumento == "Boll. Neg." || TipoDocumento == "BNEG")
                return "BNEG";
            else if (TipoDocumento == "Nota Acc." || TipoDocumento == "NACC")
                return "NACC";
            else if (TipoDocumento == "BIIN")
                return "BIIN";
            else if (TipoDocumento == "Bonus" || TipoDocumento == "BONU")
                return "BONU";
            else if (TipoDocumento == "Indennizzo" || TipoDocumento == "INDE")
                return "INDE";

            return "";
        }
        public String GetManagerInfo(String CodGruppo)
        {
            string managermail = string.Empty;

            try
            {
                var sql = Sql.Builder.Append("SELECT MANAGER_EMAIL FROM GRI_CAPGROUPING_ON_STANDARD WHERE COD_PRESTAZIONE = @0", CodGruppo);
                managermail= db.Query<String>(sql).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile completare l'operazione in GetManagerInfo: " + ex.Message);
            }

            return managermail;
        }
        public String GetManagerMail_Iban(String CodGruppo)
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT MANAGER_EMAIL_IBAN FROM GRI_CAPGROUPING_ON_STANDARD WHERE COD_PRESTAZIONE = @0", CodGruppo);
                return db.Query<String>(sql).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile completare l'operazione in GetManagerInfo: " + ex.Message);
            }
        }
        public ClienteBonusIdrico GetClienteBonusIdricoRimborsi(string CodiceCliente)
        {
            ClienteBonusIdrico _clienteBonus = null;

            try
            {
                var sql = Sql.Builder.Append(string.Format("Select DATA_EMISSIONE,IMP_BONUS From BONUSIDRICO_RIMBORSI Where  cod_cliente='{0}' and rownum=1 Order By DATA_EMISSIONE desc", CodiceCliente));
                _clienteBonus = db.SingleOrDefault<ClienteBonusIdrico>(sql);
                return _clienteBonus;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile completare l'operazione in GetClienteBonusIdricoRimborsi: " + ex.Message);
            }

        }

    }
}