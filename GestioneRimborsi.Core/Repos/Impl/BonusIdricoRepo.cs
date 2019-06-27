using GruppoCap.Core;
using GruppoCap.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestioneRimborsi.Core.Models;
using PetaPoco;
using GruppoCap.Core.Data;
using GruppoCap;
using GruppoCap.Logging;

namespace GestioneRimborsi.Core
{
    public class BonusIdricoRepo : RepositoryBase<Lotto>, IBonusIdricoRepo
    {

        //IBiInfoRepo _infoRepo = null;
        //ILogger _logger = null;

        //public BonusIdricoRepo(IBiInfoRepo infoRepository, ILogger ILogger)
        //{
        //    _logger = ILogger;
        //    _infoRepo = infoRepository;            
        //}

        public IEnumerable<BICapLotto> getRequestLots(int? id)
        {
            IEnumerable<BICapLotto> data = null;
            try
            {
                if (id == null)
                {
                    data = db.Query<BICapLotto>("select * from GRI_BI_LOTS_CAP ");
                }
                else
                {
                    data = new List<BICapLotto>();
                    ((List<BICapLotto>)data).Add(db.FirstOrDefault<BICapLotto>(string.Format("select * from GRI_BI_LOTS_CAP l where l.BI_CAP_ID={0}", id)));
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "impossibile recuperare i dati utente SGATE");
                throw e;
            }
            return data.OrderBy(x => x.Status).ThenBy(x => x.DataScadenza).ThenBy(X => X.DataCarico).ThenBy(x => x.Id);
        }

        public void ValidateLot(int lotId)
        {
            bool _hasTransactionRolledBack = false;
            db.BeginTransaction();
            try
            {
                PetaPocoParameter[] theLotId = { new PetaPocoInputParameter("lotid", System.Data.DbType.Int32, lotId) };

                var validazioneResult = db.ExecuteProcedure("GRI_BI_VALIDAZIONE.VALIDALOTTO_P", theLotId);
            }
            catch (Exception e)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                e.Data.Add("messaggio", "Validazione Lotto fallita");
                throw e;
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();

        }

        public void ConfirmLot(int lotId)
        {
            IEnumerable<BIConfermato> biConfermati = null;
            IEnumerable<BICapRequest> biCapRequest = null;
            String biClienteNew = String.Empty;
            db.BeginTransaction();
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GRI_BI_REQUEST_CAP WHERE BI_LOTCAP_ID = @0", lotId);
                biCapRequest = db.Query<BICapRequest>(sql);

                foreach (var item in biCapRequest)
                {
                    sql = Sql.Builder.Append("SELECT MAX(BI_CLIENTE_NEW) FROM GRI_BI_REQUEST req INNER JOIN GRI_BI_REQUEST_CAP reqc ON reqc.BI_REQ_CAP_ID = req.BI_REQ_ID WHERE req.BI_REQ_CF = (select req.BI_REQ_CF FROM GRI_BI_REQUEST req WHERE BI_REQ_ID = @0) AND BI_CLIENTE_NEW IS NOT NULL", item.Id);
                    biClienteNew = db.FirstOrDefault<String>(sql);
                    string queryCentralizzato = String.Empty;
                    if (!String.IsNullOrEmpty(biClienteNew))
                        queryCentralizzato = string.Format("UPDATE GRI_BI_REQUEST_CAP SET BI_CONFERMA = 1, BI_CLIENTE_NEW = {0} WHERE BI_REQ_CAP_ID = {1} AND UPPER(TRIM(BI_TIPOLOGIA_RICHIESTA)) = 'CENTRALIZZATA' AND (BI_ESITO_MANVAL LIKE '%OK%' OR BI_ESITO_AUTOVAL LIKE '%OK%')", biClienteNew, item.Id);
                    else
                        queryCentralizzato = string.Format("UPDATE GRI_BI_REQUEST_CAP SET BI_CONFERMA = 1, BI_CLIENTE_NEW = BI_CLIENTE_SEQ.NEXTVAL WHERE BI_REQ_CAP_ID = {0} AND UPPER(TRIM(BI_TIPOLOGIA_RICHIESTA)) = 'CENTRALIZZATA' AND (BI_ESITO_MANVAL LIKE '%OK%' OR BI_ESITO_AUTOVAL LIKE '%OK%')", item.Id);
                    db.Execute(queryCentralizzato);
                }

                string queryIndividuali = string.Format("UPDATE GRI_BI_REQUEST_CAP SET BI_CONFERMA = 1 WHERE BI_LOTCAP_ID = {0} AND UPPER(TRIM(BI_TIPOLOGIA_RICHIESTA)) = 'INDIVIDUALE' AND (BI_ESITO_MANVAL LIKE '%OK%' OR BI_ESITO_AUTOVAL LIKE '%OK%')", lotId);
                db.Execute(queryIndividuali);
                string queryConfermaLotto = string.Format("UPDATE GRI_BI_LOTS_CAP SET BI_STATUS = 6 WHERE BI_CAP_ID = {0}", lotId);
                db.Execute(queryConfermaLotto);
                
                sql = Sql.Builder.Append("SELECT * FROM GRI_BI_CONFERMATI_V WHERE LOTCAPID = @0", lotId);
                biConfermati = db.Query<BIConfermato>(sql);

                foreach (var item in biConfermati)
                {
                    if (item.TIPOLOGIARICHIESTA == "INDIVIDUALE")
                    {
                        var TestataRimborso = new TestataRimborso()
                        {
                            AnnoDocumento = item.ANNO_DOCUMENTO,
                            NumeroDocumento = item.NUMERO_DOCUMENTO,
                            TipoDocumento = item.TIPO_DOCUMENTO,
                            StatoDocumento = "1",
                            DataInserimento = item.DATA_INSERIMENTO,
                            ImportoTotaleRimborso = item.IMP_TOT_RIMB,
                            AnnoRimborso = item.ANNO_RIMBORSO_INTEGRA,
                            IDRimborsoIntegra = item.ID_RIMBORSO_INTEGRA,
                            CodiceCliente = item.CODICE_CLIENTE,
                            ABI = item.ABI,
                            CAB = item.CAB,
                            ContoCorrente = item.CONTO_CORRENTE,
                            CIN = item.CIN,
                            CodicePuntoFornitura = item.COD_PUNTO_FORNITURA
                        };                        
                        //db.Insert(TestataRimborso);

                        //var sqlInsertDett = Sql.Builder.Append("INSERT INTO UT_RIMB_DETT ( ANNO_DOCUMENTO, NUMERO_DOCUMENTO, TIPO_DOCUMENTO, TIPO_RIMBORSO, IMPORTO, NUMERO_BOLLETTA, COD_AZIENDA)");
                        //sqlInsertDett.Append("VALUES (@0,@1,@2,@3,@4,@5,@6)", item.ANNO_DOCUMENTO, item.NUMERO_DOCUMENTO, item.TIPO_DOCUMENTO, "MAND", item.IMP_TOT_RIMB, "", "01");
                        //db.Execute(sqlInsertDett);
                    }
                    else if (!String.IsNullOrEmpty(item.IBAN))
                    {                        
                        sql = Sql.Builder.Append("INSERT INTO GRI_IBAN (CODICE_CLIENTE, IBAN, DATA_INSERIMENTO, UTENTE_INSERIMENTO) VALUES (@0,@1,SYSDATE,'BONUSIDRICO')", item.CODICE_CLIENTE.Trim(), item.IBAN.Trim());
                        db.Execute(sql);
                    }
                }
                db.CompleteTransaction();
            }
            catch (Exception)
            {
                db.AbortTransaction();
                throw;
            }
        }

        public int InsertCapLotto(BICapLotto Lotto)
        {
            try
            {
                return Convert.ToInt32(db.Insert(Lotto));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void InsertCapReq(BICapRequest Lotto)
        {
            try
            {
                db.Insert(Lotto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void InsertSgateReq(SgateRichieste req)
        {
            try
            {
                var id = db.Insert(req);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateIntegraTable(SgateRichieste nodosgate, BICapRequest nodocap, BICapLotto lotto)
        {
            bool _hasTransactionRolledBack = false;
            db.BeginTransaction();
            try
            {
                UpdateSgateReq("GRI_BI_REQUEST", "BI_REQ_ID", nodosgate);
                UpdateCapReq("GRI_BI_REQUEST_CAP", "BI_REQ_CAP_ID", nodocap);
                UpdateCapLotto("GRI_BI_LOTS_CAP", "BI_CAP_ID", lotto);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                e.Data.Add("messaggio", "Validazione Utente fallita");
                throw e;
            }

            return;
        }

        public void UpdateSgateReq(string table, string pk, SgateRichieste nodo)
        {
            try
            {
                db.Update(table, pk, nodo);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "aggiornamento dati utente fallito");
                throw e;
            }
        }

        public void UpdateCapLotto(string table, string pk, BICapLotto nodo)
        {
            try
            {
                db.Update(table, pk, nodo);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "Aggiornamento Lotto fallito");
                throw e;
            }
        }

        public IEnumerable<SgateRichieste> getOriginalSGATeRequests(int? id, string sql = null)
        {

            IEnumerable<SgateRichieste> data = null;
            //using (var db = new PetaPoco.Database("test.application.db"))
            //{
            try
            {
                if (id == null && sql == null)
                    data = db.Query<SgateRichieste>("select * from gri_bi_request r join gri_bi_lots_cap l on r.BI_LOTCAP_ID = l.BI_CAP_ID ");
                else if (id != null)
                    data = db.Query<SgateRichieste>(string.Format("select * from gri_bi_request r where r.BI_LOTCAP_ID ={0}", id));
                else if (sql != null)
                    data = db.Query<SgateRichieste>(string.Format(sql));

            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw (new InvalidOperationException(e.Message));
            }

            return data;
        }

        public IEnumerable<BICapRequest> getCapRequestsByLot(int lotId)
        {
            IEnumerable<BICapRequest> res;
            try
            {
                return res = db.Query<BICapRequest>(string.Format("select * from GRI_BI_REQUEST_CAP where BI_LOTCAP_ID={0}", lotId));               
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "Richiesta non rilevata");
                throw e;
            }
        }

        public BICLienti GetCliente(string sql)
        {
            BICLienti data;
            try
            {
                data = db.FirstOrDefault<BICLienti>(sql);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw (new InvalidOperationException(e.Message));
            }
            return data;
        }

        public void DeleteCapReq(int lotto)
        {
            db.Delete<BICapRequest>("BI_LOTCAP_ID", lotto);
        }

        public BICapRequest getCapRequestById(int id)
        {
            try
            {
                return db.Single<BICapRequest>(id);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "Richiesta non rilevata");
                throw e;
            }
        }

        public List<BICapRequest> getCAPRequestById(int id)
        {
            List<BICapRequest> data = null;
            //using (var db = new PetaPoco.Database("test.application.db"))
            //{
            try
            {
                data = db.Query<BICapRequest>(string.Format("select * from gri_bi_request_cap r where r.bi_lotcap_id={0}", id)).ToList();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw (new InvalidOperationException(e.Message));
            }
            return data;
        }

        public List<BiInfoNuoviClienti> GetInfoNuoviClienti(int id)
        {
            List<BiInfoNuoviClienti> data = null;
            //using (var db = new PetaPoco.Database("test.application.db"))
            //{
            try
            {
                data = db.Query<BiInfoNuoviClienti>(string.Format("select * from gri_bi_clienti_new where id_lotto={0}", id)).ToList();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw (new InvalidOperationException(e.Message));
            }
            return data;
        }

        //oldversion
        //public List<BICapRequest> getCAPRequestById(int id)
        //{
        //    return db.Query<BICapRequest>("BI_LOTCAP_ID", id).ToList();
        //}

        public void UpdateCapReq(string table, string pk, BICapRequest nodo)
        {
            try
            {
                db.Update(table, pk, nodo);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "Aggiornamento dati utente fallito");
                throw e;
            }
        }

        public string GetContratto(string sql)
        {
            string res;
            try
            {
                res = db.FirstOrDefault<string>(sql);
            }
            catch (Exception e)
            {
                throw;
            }
            return res;
        }

        public BIPuf GetPuf(string sql)
        {
            BIPuf res;
            try
            {
                res = db.FirstOrDefault<BIPuf>(sql);
            }
            catch (Exception e)
            {
                throw;
            }
            return res;
        }

        public T GetPufGeneric<T>(T sql) where T : IEntity
        {
            T res;
            try
            {
                res = db.FirstOrDefault<T>(sql.ToString());
            }
            catch (Exception e)
            {
                throw;
            }
            return res;
        }

        public T CapReqByID<T>(int id) where T : IEntity
        {
            try
            {
                return db.SingleOrDefault<T>(id);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw;
            }
        }



        //public List<BICapRequest> getCapRequestsByLot<T>(int lotto) 
        //{
        //    try
        //    {
        //        return db.Query<BICapRequest>(string.Format("select * from GRI_BI_REQUEST_CAP where BI_LOTCAP_ID={0}", lotto)).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}

        public SgateRichieste getOriginalSGATeRequestById(int id)
        {
            try
            {
                return db.SingleOrDefault<SgateRichieste>(id);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "Richiesta non rilevata");
                throw (e);
            }
        }

        public BICapLotto getRequestLotById(int id)
        {
            try
            {
                return db.SingleOrDefault<BICapLotto>(id);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "Impossibile recuperare i dati del Lotto");
                throw (e);
            }
        }

        public BIComAnagrafica getCustomerByIntegraCode(string cliente)
        {
            var data = db.FirstOrDefault<BIComAnagrafica>(string.Format("select * from GRI_DATI_VALIDAZIONE_BI_V where COD_CLIENTE='{0}'", cliente));
            return data;
        }
        public BIComAnagrafica CodCLienteByIntegra(string integra)
        {
            BIComAnagrafica data;
            try
            {
                data = db.FirstOrDefault<BIComAnagrafica>(string.Format("select * from GRI_DATI_VALIDAZIONE_BI_V where COD_CLIENTE_INTEGRA='{0}'", integra));
                if (data == null)
                    throw new Exception();
            }
            catch (Exception e)
            {
                e.Data.Add("messaggio", "Codice Integra non valido");
                throw (e);
            }
            return data;
        }

        public List<BIContratto> GetContrattoByCliente(string sql)
        {
            try
            {
                return db.Query<BIContratto>(sql).ToList();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "Impossibile recuperare i dati contrattuali dell'utente");
                throw (e);
            }
        }

        public IUpdateOperationResult CalcoloBonusIdrico(int lotId)
        {
            try
            {
                db.OpenSharedConnection();
                try
                {
                    var Param = new List<PetaPocoParameter>();
                    Param.Add(new PetaPocoInputParameter("P_LOT_ID", System.Data.DbType.Int32, lotId));
                    var result = this.db.ExecuteProcedure("GRI_CREATE_WATER_BONUS_P", Param.ToArray());

                    return new UpdateOperationResult(result.Result);
                }
                finally
                {
                    db.CloseSharedConnection();
                }

            }
            catch (Exception e)
            {
                e.Data.Add("messaggio", "Errore nella procedura di calcolo del Bonus Idrico");
                throw (e);
            }
        }

        public List<BICapRequest> getLotDetails(QueryOptions options)
        {
            List<BICapRequest> result = null;

            try
            {
                string databaseQuery = string.Format(" select * FROM (SELECT ril.*, ROW_NUMBER() OVER(ORDER BY bi_req_cap_id DESC) AS row_number FROM gri_bi_request_cap ril where 1=1 and {2}) where row_number between {0} and {1}", options.getLowerBound(),
                    options.getUpperBound(),
                    string.Join(" AND ", options.ConditionCriterias.Select(ee => string.Format(ee.Key.matchingVerb, ee.Key.fieldName, ee.Value)))
                    );
                var t = options.ConditionCriterias.Select(ee => string.Format(ee.Key.matchingVerb, ee.Key.fieldName, ee.Value));
                result = db.Query<BICapRequest>(databaseQuery).ToList();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "Impossibile recuperare i dettagli dei Lotti");
                throw (e);
            }
            return result;
        }

        public int getLotRequestCount(QueryOptions options)
        {
            int result = 0;
            try
            {
                string databaseQuery = string.Format(" select count(*) FROM (SELECT ril.*, ROW_NUMBER() OVER(ORDER BY bi_req_cap_id DESC) AS row_number FROM gri_bi_request_cap ril where 1=1 and {0})",
                    string.Join(" AND ", options.ConditionCriterias.Select(ee => string.Format(ee.Key.matchingVerb, ee.Key.fieldName, ee.Value)))
                    );

                result = db.ExecuteScalar<int>(databaseQuery);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw (new InvalidOperationException(e.Message));
            }
            return result;

        }

        public List<LotProgressInfo> getLotProgress()
        {
            List<LotProgressInfo> result;
            try
            {
                string databaseQuery = "select * from GRI_BI_LOTPROGRESS";
                result = db.Query<LotProgressInfo>(databaseQuery).ToList();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw (new InvalidOperationException(e.Message));
            }
            return result;

        }

        public Comune getComuneByIstat(string istat)
        {
            Comune result;
            try
            {
                string databaseQuery = string.Format("select * from aque.Comuni where istat={0}", istat);
                result = db.Query<Comune>(databaseQuery).SingleOrDefault();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw (new InvalidOperationException(e.Message));
            }
            return result;
        }

        //public class BiInfoRepo : RepositoryBase<BiInfoNuoviClienti>, IBiInfoRepo
        //{
        //    public ISubCollection<BiInfoNuoviClienti> GetByLotId(Int32 LotId) 
        //    {
        //        var sql = Sql.Builder.Append(" SELECT * FROM {0} WHERE 1 = 1 ".FormatWith(EntityTableName));

        //        sql.Append(" AND ID_LOTTO = @0 ", LotId);

        //        //sql.OrderBy("ID_LOTTO ASC");

        //        IEnumerable<BiInfoNuoviClienti> t = null;
        //        t = db.Query<BiInfoNuoviClienti>(sql);

        //        return t.ToSubCollection<BiInfoNuoviClienti>();
        //    }
        //}

        //public ISubCollection<BiInfoNuoviClienti> GetByLotId(Int32 LotId)
        //{
        //    var info = _infoRepo.GetByLotId(LotId);
        //    return info;
        //}

        public BIRequestValidate GetRequestValidateById(int id)
        {
            try
            {
                //return db.Single<BIRequestValidate>(string.Format("select * from ")
                return db.Single<BIRequestValidate>(id);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                e.Data.Add("messaggio", "Richiesta non rilevata");
                throw e;
            }
        }

    }
}
