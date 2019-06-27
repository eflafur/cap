using GruppoCap.Core;
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
    public class RettificaFuoriStandardRepo : RepositoryBase<RettificaFuoriStandard>, IRettificaFuoriStandardRepo
    {
        public RettificaFuoriStandard GetRettificaApprovataByID(String FuoriStandardID)
        {
            RettificaFuoriStandard _rettifica = null;
            try
            {
                var sql = Sql.Builder.Append("select * from gri_rettifiche_fs where ID_FUORI_STANDARD = @0 and storico = 0 and esito = 1", FuoriStandardID);
                _rettifica = db.FirstOrDefault<RettificaFuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRettificaApprovataByID: " + ex.Message);
            }
            return _rettifica;
        }

        public ISubCollection<RettificaFuoriStandard> GetRettificheByID(String FuoriStandardID)
        {
            ISubCollection<RettificaFuoriStandard> _rettifiche = null;
            try
            {
                var sql = Sql.Builder.Append("select * from gri_rettifiche_fs where ID_FUORI_STANDARD = @0 order by CREATO_IL desc", FuoriStandardID);
                _rettifiche = db.Query<RettificaFuoriStandard>(sql).ToSubCollection<RettificaFuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRettificheByID: " + ex.Message);
            }
            return _rettifiche;
        }

        public RettificaFuoriStandard GetRettificaApertaByID(String FuoriStandardID)
        {
            RettificaFuoriStandard _indennizzo = null;
            try
            {
                var sql = Sql.Builder.Append("select * from gri_rettifiche_fs where ID_FUORI_STANDARD = @0 and storico = 0", FuoriStandardID);
                //var sql = Sql.Builder.Append("select ret.* from gri_rettifiche_fs ret inner join GIN_FUORI_STD_DA_VALID_FS_V fs on fs.ID_INDENNIZZO = ret.ID_FUORI_STANDARD where ret.ID_FUORI_STANDARD = @0 and ret.storico = 0", FuoriStandardID);
                _indennizzo = db.FirstOrDefault<RettificaFuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRettificaApertaByID: " + ex.Message);
            }
            return _indennizzo;
        }
        public ISubCollection<FuoriStandard> GetFuoriStandardSenzaRettifica(String CodGruppo)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_DA_VALID_V val where val.cod_gruppo LIKE @0 and val.ID_INDENNIZZO not in(select distinct id_fuori_standard from gri_rettifiche_fs)", CodGruppo + "%");
                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetFuoriStandardSenzaRettifica: " + ex.Message);
            }
            return _list;
        }
        public String ApprovaRettifica(String FuoriStandard, String CodiceCliente, String CodicePuf, String CodiceContratto, String Utente, String Note, String ErrDataInizio, String ErrDataFine, String ErrTempoLavorazione, String ErrSospensione, String ErrFlagStandard, String CodiceCausa, String CodiceSottocausa, String FlagErrore, String NonIndennizzabile, String NoteApprovatore, bool isProcessOwner)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                
                db.BeginTransaction();

                FuoriStandard _indennizzo = GetFuoriStandardRettifica(FuoriStandard.ToString());
                RettificaFuoriStandard _rettifica = GetRettificaApertaByID(FuoriStandard.ToString());
                if (_indennizzo != null && String.IsNullOrEmpty(CodiceCliente))
                {
                    CodiceCliente = _indennizzo.CodCliente;
                    CodicePuf = _indennizzo.CodPuf;
                    CodiceContratto = _indennizzo.CodContratto;
                }

                PetaPocoPrecedureResult result;
                String returnString = String.Empty;

                if (_rettifica.FlagIsRettifica == 0)
                {
                    PetaPocoParameter[] conferma = { new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, Convert.ToInt32(_indennizzo.IDFS)),
                                                     new PetaPocoInputParameter("p_COD_CLIENTE", System.Data.DbType.String, CodiceCliente),
                                                     new PetaPocoInputParameter("p_COD_PUF", System.Data.DbType.String, CodicePuf),
                                                     new PetaPocoInputParameter("p_COD_CONTRATTO", System.Data.DbType.String, CodiceContratto),
                                                     new PetaPocoInputParameter("p_UTE_VALIDAZIONE", System.Data.DbType.String, _rettifica.Autore),
                                                     new PetaPocoInputParameter("p_DT_VALIDAZIONE", System.Data.DbType.Date, _rettifica.CreatoIl),
                                                     new PetaPocoInputParameter("p_UTE_APP_PO", System.Data.DbType.String, Utente),
                                                     new PetaPocoInputParameter("p_DT_APP_PO", System.Data.DbType.Date, DateTime.Now),
                                                     new PetaPocoInputParameter("p_Note", System.Data.DbType.String, (!String.IsNullOrEmpty(Note) ? Note + (!String.IsNullOrEmpty(NoteApprovatore) ? " - " + NoteApprovatore : "") : NoteApprovatore)),
                                                     new PetaPocoInputParameter(" p_CODICE_CAUSA", System.Data.DbType.String, CodiceCausa),
                                                     new PetaPocoInputParameter("p_CODICE_SOTTOCAUSA", System.Data.DbType.String, CodiceSottocausa),
                                                     new PetaPocoInputParameter("p_TIPO_STANDARD", System.Data.DbType.String, _indennizzo.TipoStandard),
                                                     new PetaPocoInputParameter("p_FLG_UTE_NON_INDENNIZZABILE", System.Data.DbType.String, NonIndennizzabile),
                                                     new PetaPocoInputParameter("p_FLAG_ORIGINE", System.Data.DbType.String, _indennizzo.FlagOrigine),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500) };

                    result = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_VALIDAZIONE", conferma);

                    var updateRettifica = Sql.Builder.Append("UPDATE GRI_RETTIFICHE_FS SET ESITO = 1, NOTE_APPROVATORE = @1, NOTE_PROCESS_OWNER = @1, FLG_STATO = 3, PROCESS_OWNER = @2, DATA_APPROVAZIONE_PO = SYSDATE WHERE ID_FUORI_STANDARD = @0 AND STORICO = 0", Convert.ToInt32(FuoriStandard), NoteApprovatore, Utente);
                    db.Execute(updateRettifica);

                    returnString = procedureResult(result);
                    if (!String.IsNullOrEmpty(returnString))
                        throw new ApplicationException(returnString);
                }
                else if (isProcessOwner)
                {
                    var updateRettifica = Sql.Builder.Append("UPDATE GRI_RETTIFICHE_FS SET NOTE_PROCESS_OWNER = @1, FLG_STATO = 2, PROCESS_OWNER = @2, DATA_APPROVAZIONE_PO = SYSDATE WHERE ID_FUORI_STANDARD = @0 AND STORICO = 0", Convert.ToInt32(FuoriStandard), NoteApprovatore, Utente);
                    db.Execute(updateRettifica);
                }
                else
                {

                    ISubCollection<RettificaSospensione> _rettificaSospensione = GetSospensioniByIDFuoriStandard(FuoriStandard.ToString());

                    foreach (var item in _rettificaSospensione.Items)
                    {
                        var dataComunicazione = "";
                        if (item.DATA_COMUNICAZIONE != null && item.DATA_COMUNICAZIONE != DateTime.MinValue)
                            dataComunicazione = item.DATA_COMUNICAZIONE.ToShortDateString();

                        if (item.STATO_SOSPENSIONE == "N")
                        {
                            PetaPocoParameter[] inserimentoSospensioni = { new PetaPocoInputParameter("p_ID_SOSPENSIONE_DWH", System.Data.DbType.Int32, 0),
                                                     new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, item.ID_INDENNIZZO),
                                                     new PetaPocoInputParameter("p_NUMERO_PRESTAZIONE", System.Data.DbType.String, item.NUMERO_PRESTAZIONE),
                                                     new PetaPocoInputParameter("p_ID_STANDARD", System.Data.DbType.Int32, item.ID_STANDARD),
                                                     new PetaPocoInputParameter("p_STATO_SOSPENSIONE", System.Data.DbType.String, "N"),
                                                     new PetaPocoInputParameter("p_DATA_INIZIO_SOSPENSIONE", System.Data.DbType.Date, item.DATA_INIZIO_SOSPENSIONE),
                                                     new PetaPocoInputParameter("p_DATA_FINE_SOSPENSIONE",System.Data.DbType.Date, item.DATA_FINE_SOSPENSIONE),
                                                     (dataComunicazione == "" ? new PetaPocoInputParameter("p_DATA_COMUNICAZIONE",System.Data.DbType.Date,  null) : new PetaPocoInputParameter("p_DATA_COMUNICAZIONE",System.Data.DbType.Date,  Convert.ToDateTime(dataComunicazione))),
                                                     new PetaPocoInputParameter("p_CATEGORIA_SOSPENSIONE",System.Data.DbType.String, item.CATEGORIA_SOSPENSIONE),
                                                     new PetaPocoInputParameter("p_TIPO_SOSPENSIONE",System.Data.DbType.String, item.TIPO_SOSPENSIONE),
                                                     new PetaPocoInputParameter("p_DURATA_SOSPENSIONE",System.Data.DbType.Decimal, item.DURATA_SOSPENSIONE),
                                                     new PetaPocoInputParameter("p_DATA_INS",System.Data.DbType.Date, item.DATA_INS),
                                                     new PetaPocoInputParameter("p_UTE_INS", System.Data.DbType.String, item.UTE_INS),
                                                     new PetaPocoInputParameter("p_DATA_VALERR", System.Data.DbType.Date, DateTime.Now),
                                                     new PetaPocoInputParameter("p_UTE_VALERR", System.Data.DbType.String, Utente),
                                                     new PetaPocoInputParameter("p_NOTE", System.Data.DbType.String, item.NOTE),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500) };

                            result = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_INSERIMENTO_SOSPENSIONE", inserimentoSospensioni);
                            returnString = procedureResult(result);
                            if (!String.IsNullOrEmpty(returnString))
                                throw new ApplicationException(returnString);
                        }
                        else if (item.STATO_SOSPENSIONE == "R")
                        {
                            PetaPocoParameter[] inserimentoSospensioni = { new PetaPocoInputParameter("p_ID_SOSPENSIONE_DWH", System.Data.DbType.Int32, item.ID_SOSPENSIONE),
                                                     new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, item.ID_INDENNIZZO),
                                                     new PetaPocoInputParameter("p_NUMERO_PRESTAZIONE", System.Data.DbType.String, item.NUMERO_PRESTAZIONE),
                                                     new PetaPocoInputParameter("p_ID_STANDARD", System.Data.DbType.Int32, item.ID_STANDARD),
                                                     new PetaPocoInputParameter("p_DATA_INIZIO_SOSPENSIONE", System.Data.DbType.Date, item.DATA_INIZIO_SOSPENSIONE),
                                                     new PetaPocoInputParameter("p_DATA_FINE_SOSPENSIONE",System.Data.DbType.Date, item.DATA_FINE_SOSPENSIONE),
                                                     (dataComunicazione == "" ? new PetaPocoInputParameter("p_DATA_COMUNICAZIONE",System.Data.DbType.Date,  null) : new PetaPocoInputParameter("p_DATA_COMUNICAZIONE",System.Data.DbType.Date,  Convert.ToDateTime(dataComunicazione))),
                                                     new PetaPocoInputParameter("p_CATEGORIA_SOSPENSIONE",System.Data.DbType.String, item.CATEGORIA_SOSPENSIONE),
                                                     new PetaPocoInputParameter("p_TIPO_SOSPENSIONE",System.Data.DbType.String, item.TIPO_SOSPENSIONE),
                                                     new PetaPocoInputParameter("p_DURATA_SOSPENSIONE",System.Data.DbType.Decimal, item.DURATA_SOSPENSIONE),
                                                     new PetaPocoInputParameter("p_DATA_INS",System.Data.DbType.Date, item.DATA_INS),
                                                     new PetaPocoInputParameter("p_UTE_INS", System.Data.DbType.String, item.UTE_INS),
                                                     new PetaPocoInputParameter("p_DATA_VALERR", System.Data.DbType.Date, DateTime.Now),
                                                     new PetaPocoInputParameter("p_UTE_VALERR", System.Data.DbType.String, Utente),
                                                     new PetaPocoInputParameter("p_NOTE", System.Data.DbType.String, item.NOTE),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500) };

                            result = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_RETTIFICA_SOSPENSIONE", inserimentoSospensioni);
                            returnString = procedureResult(result);
                            if (!String.IsNullOrEmpty(returnString))
                                throw new ApplicationException(returnString);
                        }
                    }

                    PetaPocoParameter[] approva = { new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, Convert.ToInt32(FuoriStandard)),
                                                     new PetaPocoInputParameter("p_COD_CLIENTE", System.Data.DbType.String, CodiceCliente),
                                                     new PetaPocoInputParameter("p_COD_PUF", System.Data.DbType.String, CodicePuf),
                                                     new PetaPocoInputParameter("p_COD_CONTRATTO", System.Data.DbType.String, CodiceContratto),
                                                     new PetaPocoInputParameter("p_UTE_VALERR", System.Data.DbType.String, Utente),
                                                     new PetaPocoInputParameter("p_DT_VALERR", System.Data.DbType.Date, DateTime.Now),
                                                     new PetaPocoInputParameter("p_UTE_APP_PO", System.Data.DbType.String, _rettifica.ProcessOwner),
                                                     new PetaPocoInputParameter("p_DT_APP_PO", System.Data.DbType.Date, _rettifica.DataApprovazionePO),                                                     
                                                     new PetaPocoInputParameter("p_Note", System.Data.DbType.String, Note + (!String.IsNullOrEmpty(_rettifica.NoteProcessOwner) ? " - " + _rettifica.NoteProcessOwner : "") + (!String.IsNullOrEmpty(NoteApprovatore) ? " - " + NoteApprovatore : "")),
                                                     new PetaPocoInputParameter("p_ERR_DATA_INIZIO",System.Data.DbType.Date, Convert.ToDateTime(ErrDataInizio)),
                                                     new PetaPocoInputParameter("p_ERR_DATA_FINE",System.Data.DbType.Date,  Convert.ToDateTime(ErrDataFine)),
                                                     new PetaPocoInputParameter("p_ERR_TEMPO_LAVORAZIONE",System.Data.DbType.Decimal, Convert.ToDecimal(ErrTempoLavorazione.Replace(".",","))),
                                                     new PetaPocoInputParameter("p_ERR_SOSPENSIONE",System.Data.DbType.Decimal, Convert.ToDecimal(ErrSospensione.Replace(".",","))),
                                                     new PetaPocoInputParameter("p_ERR_FLAG_STANDARD",System.Data.DbType.String, ErrFlagStandard),
                                                     new PetaPocoInputParameter("p_TEMPO_LAVORAZIONE",System.Data.DbType.Decimal, _indennizzo.EvasoIn),
                                                     new PetaPocoInputParameter(" p_CODICE_CAUSA", System.Data.DbType.String, CodiceCausa),
                                                     new PetaPocoInputParameter("p_CODICE_SOTTOCAUSA", System.Data.DbType.String, CodiceSottocausa),
                                                     new PetaPocoInputParameter("p_ERR_FLG_ERRORE", System.Data.DbType.String, FlagErrore),
                                                     new PetaPocoInputParameter("p_FLG_UTE_NON_INDENNIZZABILE", System.Data.DbType.String, NonIndennizzabile),
                                                     new PetaPocoInputParameter("p_UTE_VALIDAZIONE", System.Data.DbType.String, _rettifica.Autore),
                                                     new PetaPocoInputParameter("p_DT_VALIDAZIONE", System.Data.DbType.Date, _rettifica.CreatoIl),
                                                     new PetaPocoInputParameter("p_TIPO_STANDARD ", System.Data.DbType.String, _indennizzo.TipoStandard),
                                                     new PetaPocoInputParameter("p_FLAG_ORIGINE ", System.Data.DbType.String, _indennizzo.FlagOrigine),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500) };

                    result = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_VALIDA_ERRORE", approva);

                    var updateRettifica = Sql.Builder.Append("UPDATE GRI_RETTIFICHE_FS SET ESITO = 1, NOTE_APPROVATORE = @1, FLG_STATO = 3, MANAGER = @2 WHERE ID_FUORI_STANDARD = @0 AND STORICO = 0", Convert.ToInt32(FuoriStandard), NoteApprovatore, Utente);
                    db.Execute(updateRettifica);

                    returnString = procedureResult(result);
                    if (!String.IsNullOrEmpty(returnString))
                        throw new ApplicationException(returnString);

                    /**********************/
                }
            }
            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in Approvazione Rettifica: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;
        }

        private String procedureResult(PetaPocoPrecedureResult result)
        {
            String returnString = String.Empty;
            if (result == null)
            {
                return returnString = "1. Errore generico durante l'esecuzione della procedura";
            }

            if (result.Result)
                if (result.OutputParams != null)
                {
                    PetaPocoOutputParameter _p = result.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Retc", StringComparison.CurrentCultureIgnoreCase));
                    if (_p != null)
                    {
                        if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                        {
                            _p = result.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Msg", StringComparison.CurrentCultureIgnoreCase));
                            if (_p != null)
                            {
                                return returnString = _p.Value.ToString();
                            }
                            else
                            {
                                return returnString = "Valore di Output non trovato";
                            }
                        }
                        else return returnString;
                    }
                    else
                    {
                        return returnString = "2. Errore generico durante l'esecuzione della procedura";
                    }
                }
                else
                {
                    return returnString = "4. Errore generico durante l'esecuzione della procedura. Nessun parametro di output";
                }
            else
            {
                return returnString = String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", result.ErrorMessage);
            }
        }


        public String RifiutaRettifica(String FuoriStandard, String Note, String NoteApprovatore, String Utente)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                db.BeginTransaction();

                FuoriStandard _indennizzo = GetFuoriStandardRettifica(FuoriStandard);

                if (_indennizzo.FlagOrigine == "I")
                {

                    PetaPocoParameter[] delStandard = { new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, Convert.ToInt32(FuoriStandard)),
                                                     new PetaPocoInputParameter("p_FLAG_ORIGINE ", System.Data.DbType.String, _indennizzo.FlagOrigine),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500)
                                                  };

                    var resultDelStandard = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_DEL_INSTANDARD", delStandard);

                    /**********************/
                    if (delStandard == null)
                    {
                        throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                    }
                    if (resultDelStandard.Result)
                        if (resultDelStandard.OutputParams != null)
                        {
                            PetaPocoOutputParameter _p = resultDelStandard.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Retc", StringComparison.CurrentCultureIgnoreCase));
                            if (_p != null)
                            {
                                if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                                {
                                    _p = resultDelStandard.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Msg", StringComparison.CurrentCultureIgnoreCase));
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
                        throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultDelStandard.ErrorMessage));
                    }
                }

                /**********************/

                var updateRettifica = Sql.Builder.Append("UPDATE GRI_RETTIFICHE_FS SET ESITO = 2, NOTE = @1, FLG_STATO = 0, NOTE_APPROVATORE = @2, MANAGER = @3, DATA_APPR_MANAGER = SYSDATE WHERE ID_FUORI_STANDARD = @0 AND STORICO = 0 AND FLG_STATO = 2", Convert.ToInt32(FuoriStandard), Note, NoteApprovatore, Utente);
                db.Execute(updateRettifica);

                updateRettifica = Sql.Builder.Append("UPDATE GRI_RETTIFICHE_FS SET ESITO = 2, NOTE = @1, FLG_STATO = 0, NOTE_PROCESS_OWNER = @2, PROCESS_OWNER = @3, DATA_APPROVAZIONE_PO = SYSDATE WHERE ID_FUORI_STANDARD = @0 AND STORICO = 0 AND FLG_STATO = 1", Convert.ToInt32(FuoriStandard), Note, NoteApprovatore, Utente);
                db.Execute(updateRettifica);

                var deleteSospensioni = Sql.Builder.Append("DELETE FROM GRI_SOSPENSIONI_FS WHERE ID_INDENNIZZO = @0 OR ID_INDENNIZZO = @1", Convert.ToInt32(FuoriStandard), _indennizzo.IndennizzoDWH);
                db.Execute(deleteSospensioni);

            }
            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in Rifiuta Rettifica: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;
        }
        public ISubCollection<FuoriStandard> RicercaAvanzataApprovatore(DateTime DataInizio, DateTime DataFine, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiAperti)
        {
            List<FuoriStandard> _list = new List<FuoriStandard>();
            try
            {
                if (TuttiAperti == "T")
                {
                    var sql = Sql.Builder.Append(@"select tot.* from (select * from GIN_FUORI_STANDARD_DA_VALID_V val UNION select * from GIN_FUORI_STANDARD_STORICO_V sto) tot right join GRI_RETTIFICHE_FS ret on tot.ID_INDENNIZZO = ret.ID_FUORI_STANDARD LEFT JOIN gin_clienti_v cli ON cli.COD_CLIENTE = tot.COD_CLIENTE
                                                    where (tot.COD_RINTRACCIABILITA = @2 OR @2 IS NULL) AND (tot.DT_DECORRENZA_INDENNIZZO >= @0 OR @0 IS NULL) AND (tot.DT_DECORRENZA_INDENNIZZO <= @1 OR @1 IS NULL) AND ((tot.COD_CLIENTE = @4 OR @4 IS NULL) OR (cli.DES_RAGIONE_SOCIALE LIKE @5 OR @4 IS NULL))
                                                    and ret.storico = 0 and tot.cod_gruppo LIKE @3", DataInizio, DataFine, CodiceRintracciabilita, CodGruppo + "%", Cliente, "%" + Cliente + "%");
                    _list = _list = db.Query<FuoriStandard>(sql).ToList<FuoriStandard>();
                }
                else if (TuttiAperti == "A")
                {
                    var sql = Sql.Builder.Append(@"select val.* from GIN_FUORI_STANDARD_DA_VALID_V val inner join gri_rettifiche_fs ret on val.ID_INDENNIZZO = ret.ID_FUORI_STANDARD LEFT JOIN gin_clienti_v cli ON cli.COD_CLIENTE = val.COD_CLIENTE 
                                                    where (val.COD_RINTRACCIABILITA = @2 OR @2 IS NULL) AND (val.DT_DECORRENZA_INDENNIZZO >= @0 OR @0 IS NULL) AND (val.DT_DECORRENZA_INDENNIZZO <= @1 OR @1 IS NULL) AND ((val.COD_CLIENTE = @4 OR @4 IS NULL) OR (cli.DES_RAGIONE_SOCIALE LIKE @5 OR @4 IS NULL)) and ret.storico = 0 and ret.esito = 0 and val.cod_gruppo LIKE @3", DataInizio, DataFine, CodiceRintracciabilita, CodGruppo + "%", Cliente, "%" + Cliente + "%");
                    _list = db.Query<FuoriStandard>(sql).ToList<FuoriStandard>();
                }

                return _list.ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in RicercaAvanzataApprovatore: " + ex.Message);
            }
        }

        public ISubCollection<RettificaFuoriStandard> CercaRettificheDaApprovare(String CodGruppo)
        {
            ISubCollection<RettificaFuoriStandard> _list = null;
            try
            {
                var sql = Sql.Builder.Append("select  ret.* from gri_rettifiche_fs ret inner join GIN_FUORI_STANDARD_DA_VALID_V val on ret.ID_FUORI_STANDARD = val.ID_INDENNIZZO where ret.storico = 0 and ret.esito = 0 and ret.cod_gruppo LIKE @0", CodGruppo + "%");
                _list = db.Query<RettificaFuoriStandard>(sql).ToSubCollection<RettificaFuoriStandard>();

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CercaRettificheDaApprovare: " + ex.Message);
            }
            return _list;
        }

        public ISubCollection<RettificaFuoriStandard> CercaRettificheRifiutate(String CodGruppo)
        {
            ISubCollection<RettificaFuoriStandard> _list = null;
            try
            {
                var sql = Sql.Builder.Append("select * from gri_rettifiche_fs where storico = 0 and esito = 2 and cod_gruppo LIKE @0", CodGruppo + "%");
                _list = db.Query<RettificaFuoriStandard>(sql).ToSubCollection<RettificaFuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CercaFuoriStandardRifiutati: " + ex.Message);
            }
            return _list;
        }

        public ISubCollection<FuoriStandard> CercaTutteLeRettifiche(String CodGruppo)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                //var sql = Sql.Builder.Append(" select tot.* from (select * from GIN_FUORI_STANDARD_DA_VALID_V val union select * from GIN_FUORI_STANDARD_STORICO_V sto) tot right join GRI_RETTIFICHE_FS ret on tot.ID_INDENNIZZO = ret.ID_FUORI_STANDARD where ret.STORICO = 0 and tot.COD_GRUPPO LIKE @0 ORDER BY (case ret.esito when 0 then 0 else 1 end)", CodGruppo + "%");
                var sql = Sql.Builder.Append(" select tot.* from (select * from GIN_FUORI_STD_DA_VALID_FS_V val union select * from GIN_FUORI_STANDARD_STORICO_V sto) tot right join GRI_RETTIFICHE_FS ret on tot.ID_INDENNIZZO = ret.ID_FUORI_STANDARD where ret.STORICO = 0 and tot.COD_GRUPPO LIKE @0 ORDER BY (case ret.esito when 0 then 0 else 1 end)", CodGruppo + "%");
                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CercaTutteLeRettifiche: " + ex.Message);
            }
            return _list;
        }

        public FuoriStandard CercaStandardRettifica(String IdFS)
        {
            FuoriStandard _rettifica = null;
            try
            {
                var sql = Sql.Builder.Append("select tot.* from (select * from GIN_FUORI_STANDARD_DA_VALID_V val union select * from GIN_FUORI_STANDARD_STORICO_V sto) tot where tot.ID_INDENNIZZO = @0", IdFS);
                _rettifica = db.FirstOrDefault<FuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CercaStandardRettifica: " + ex.Message);
            }
            return _rettifica;
        }

        public ISubCollection<FuoriStandard> CercaRettificheDaApprovareByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, bool isProcessOwner)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                int n;
                bool isNumeric = int.TryParse(codCliente, out n);
                tipologie.Add("");
                var sql = Sql.Builder.Append(@"select tot.* from GIN_FUORI_STD_DA_VALID_FS_V tot left join (select cli.cod_cliente, cli.des_ragione_sociale from gin_clienti_v cli where (cli.cod_cliente = @0 ", codCliente);
                if (!isNumeric) { sql.Append("OR cli.DES_RAGIONE_SOCIALE LIKE NVL(@0,NULL)", (!String.IsNullOrEmpty(codCliente) ? ("%" + (codCliente.ToUpper()) + "%") : "")); }
                sql.Append(")) cli2 on cli2.cod_cliente = tot.cod_cliente")
                .Append(" right join gri_rettifiche_fs ret on ret.ID_FUORI_STANDARD = tot.ID_INDENNIZZO")
                .Append(string.Format(" WHERE TRIM(tot.cod_gruppo) IN ({0}) ", string.Join(",", tipologie.Select(ee => string.Format("'{0}'", ee)).ToArray())))
                .Append(@"AND ret.storico = 0 and ret.esito = 0
                                     AND (tot.ID_STANDARD = @0 OR @0 IS NULL)
                                     AND (tot.COD_RINTRACCIABILITA = @1 OR @1 IS NULL)
                                     AND ((tot.COD_CLIENTE = @2 OR @2 IS NULL) OR (tot.cod_cliente in (cli2.cod_cliente) OR @2 IS NULL))
                                     AND (tot.DT_DECORRENZA_INDENNIZZO >= @4 OR @4 IS NULL OR TO_CHAR(@4, 'YYYY') = '0001')
                                     AND (tot.DT_DECORRENZA_INDENNIZZO <= @5 OR @5 IS NULL OR TO_CHAR(@5, 'YYYY') = '9999') AND rownum < 500", indicatore, codRintracciabilita, codCliente, "%" + ((codCliente != null) ? codCliente.ToUpper() : "") + "%", dataInizio, dataFine);
                if (inStandard == "S")
                    sql.Append(" AND tot.FLAG_ORIGINE = 'I'");
                else if (inStandard == "FS")
                    sql.Append(" AND tot.FLAG_ORIGINE = 'F'");
                if (isProcessOwner)
                    sql.Append(" AND (ret.FLG_STATO = 1 OR ret.FLG_STATO = 3)");
                else
                    sql.Append(" AND (ret.FLG_STATO = 2 OR ret.FLG_STATO = 3)");

                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CercaRettificheDaApprovareByFilter: " + ex.Message);
            }
            return _list;
        }

        public ISubCollection<FuoriStandard> CercaRettificheTutteByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, bool isProcessOwner)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                int n;
                bool isNumeric = int.TryParse(codCliente, out n);
                tipologie.Add("");
                var sql = Sql.Builder.Append(@"SELECT tot.* FROM (select * from GIN_FUORI_STANDARD_STORICO_V union select * from GIN_FUORI_STD_DA_VALID_FS_V) tot left join (select cli.cod_cliente, cli.des_ragione_sociale from gin_clienti_v cli where (cli.cod_cliente = @0 ", codCliente);
                if (!isNumeric) { sql.Append(" OR cli.DES_RAGIONE_SOCIALE LIKE NVL(@0,NULL)", (!String.IsNullOrEmpty(codCliente) ? (" % " + (codCliente.ToUpper()) + " % ") : "")); }
                sql.Append(")) cli2 on cli2.cod_cliente = tot.cod_cliente")
                .Append(" right join GRI_RETTIFICHE_FS ret on tot.ID_INDENNIZZO = ret.ID_FUORI_STANDARD")
                .Append(string.Format(" WHERE TRIM(tot.cod_gruppo) IN ({0}) ", string.Join(",", tipologie.Select(ee => string.Format("'{0}'", ee)).ToArray())))
                .Append(@"AND ret.STORICO = 0
                                     AND (tot.ID_STANDARD = @0 OR @0 IS NULL)
                                     AND (tot.COD_RINTRACCIABILITA = @1 OR @1 IS NULL)
                                     AND ((tot.COD_CLIENTE = @2 OR @2 IS NULL) OR (tot.cod_cliente in (cli2.cod_cliente) OR @2 IS NULL)) 
                                     AND (tot.DT_DECORRENZA_INDENNIZZO >= @4 OR @4 IS NULL OR TO_CHAR(@4, 'YYYY') = '0001')
                                     AND (tot.DT_DECORRENZA_INDENNIZZO <= @5 OR @5 IS NULL OR TO_CHAR(@5, 'YYYY') = '9999') AND rownum <= 500", indicatore, codRintracciabilita, codCliente, "%" + ((codCliente != null) ? codCliente.ToUpper() : "") + "%", dataInizio, dataFine);
                if (inStandard == "S")
                    sql.Append(" AND tot.FLAG_ORIGINE = 'I'");
                else if (inStandard == "FS")
                    sql.Append(" AND tot.FLAG_ORIGINE = 'F'");
                if (isProcessOwner)
                    sql.Append(" AND (ret.FLG_STATO = 1 OR ret.FLG_STATO = 3)");
                else
                    sql.Append(" AND (ret.FLG_STATO = 2 OR ret.FLG_STATO = 3)");
                sql.Append("ORDER BY ret.ESITO ASC");

                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CercaRettificheTutteByFilter: " + ex.Message);
            }
            return _list;
        }

        public Dictionary<string, int> GetCountRettifiche(List<String> CodiciGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (CodiciGruppo == null || CodiciGruppo.Count() == 0)
                return result;
            try
            {
                CodiciGruppo.Add("");
                var sql = Sql.Builder.Append(string.Format("select TRIM(tot.cod_gruppo) Key, count(*) Value from (select * from GIN_FUORI_STANDARD_DA_VALID_V val union select * from GIN_FUORI_STANDARD_STORICO_V sto) tot right join GRI_RETTIFICHE_FS ret on tot.ID_INDENNIZZO = ret.ID_FUORI_STANDARD where ret.STORICO = 0 and TRIM(tot.cod_gruppo) IN ({0}) group by TRIM(tot.cod_gruppo) order by TRIM(tot.cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                if (groupedResult != null && groupedResult.Count() != 0)
                {
                    groupedResult.ToList().ForEach(ee =>
                    {
                        result.Add(ee.Key, ee.Value);
                    });
                }
            }
            catch (Exception ex)
            {
                return result;
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCountRettifiche: " + ex.Message);
            }
            return result;
        }

        public Dictionary<string, int> GetCountRettificheDaApprovare(List<String> CodiciGruppo, bool isProcessOwner)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (CodiciGruppo == null || CodiciGruppo.Count() == 0)
                return result;
            try
            {
                //CodiciGruppo.ForEach(x => result.Add(x, 0));
                CodiciGruppo.Add("");
                //var sql = Sql.Builder.Append(string.Format(@"select TRIM(tot.cod_gruppo) Key, count(*) Value from (select * from GIN_FUORI_STD_DA_VALID_FS_V val where id_indennizzo in (select id_fuori_standard from gri_rettifiche_fs where storico = 0 and ESITO = 0) 
                //union select * from GIN_FUORI_STANDARD_STORICO_V sto where id_indennizzo in (select id_fuori_standard from gri_rettifiche_fs where storico = 0 and ESITO = 0)) tot where TRIM(tot.cod_gruppo) IN ({0}) group by TRIM(tot.cod_gruppo) order by TRIM(tot.cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));

                var sql = Sql.Builder.Append(string.Format(@"select TRIM(tot.cod_gruppo) Key, count(*) Value from (select * from GIN_FUORI_STD_DA_VALID_FS_V val where id_indennizzo in (select id_fuori_standard from gri_rettifiche_fs where storico = 0 and ESITO = 0 and flg_stato != {1})) tot
                                                             where TRIM(tot.cod_gruppo) IN ({0}) group by TRIM(tot.cod_gruppo) order by TRIM(tot.cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray()), (isProcessOwner ? "2" : "1")));
                // union select * from GIN_FUORI_STANDARD_STORICO_V sto where id_indennizzo in (select id_fuori_standard from gri_rettifiche_fs where storico = 0 and ESITO = 0)
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                if (groupedResult != null)
                {
                    groupedResult.ToList().ForEach(ee =>
                    {
                        result.Add(ee.Key, ee.Value);
                    });
                }
            }
            catch (Exception ex)
            {
                return result;
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCountRettificheDaApprovare: " + ex.Message);
            }
            return result;
        }

        public Dictionary<string, int> GetCountNotCanceled(List<String> CodiciGruppo, bool isProcessOwner)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (CodiciGruppo == null || CodiciGruppo.Count() == 0)
                return result;
            try
            {                
                CodiciGruppo.Add("");               

                var sql = Sql.Builder.Append(string.Format(@"select TRIM(tot.cod_gruppo) Key, count(*) Value from (select * from GIN_FUORI_STD_DA_VALID_FS_V val where id_indennizzo in (select id_fuori_standard from gri_rettifiche_fs where storico = 0 and ESITO = 0 and flg_stato != {1} and causale != 'ANN')) tot
                                                             where TRIM(tot.cod_gruppo) IN ({0}) group by TRIM(tot.cod_gruppo) order by TRIM(tot.cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray()), (isProcessOwner ? "2" : "1")));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                if (groupedResult != null)
                {
                    groupedResult.ToList().ForEach(ee =>
                    {
                        result.Add(ee.Key, ee.Value);
                    });
                }
            }
            catch (Exception ex)
            {
                return result;
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCountRettificheDaAnnullare: " + ex.Message);
            }
            return result;
        }

        public Dictionary<string, int> GetCountRettificheDaAnnullare(List<String> CodiciGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (CodiciGruppo == null || CodiciGruppo.Count() == 0)
                return result;
            try
            {
                CodiciGruppo.Add("");

                var sql = Sql.Builder.Append(string.Format(@"select TRIM(tot.cod_gruppo) Key, count(*) Value from (select * from GIN_FUORI_STD_DA_VALID_FS_V val where id_indennizzo in (select id_fuori_standard from gri_rettifiche_fs where storico = 0 and ESITO = 0 and flg_stato = 2 and causale = 'ANN')) tot
                                                             where TRIM(tot.cod_gruppo) IN ({0}) group by TRIM(tot.cod_gruppo) order by TRIM(tot.cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                if (groupedResult != null)
                {
                    groupedResult.ToList().ForEach(ee =>
                    {
                        result.Add(ee.Key, ee.Value);
                    });
                }
            }
            catch (Exception ex)
            {
                return result;
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCountRettificheDaAnnullare: " + ex.Message);
            }
            return result;
        }        

        public FuoriStandard GetFuoriStandardRettifica(String FuoriStandardID)
        {
            FuoriStandard _indennizzo = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_DA_VALID_V where ID_INDENNIZZO = @0 order by DT_DECORRENZA_INDENNIZZO", FuoriStandardID);
                _indennizzo = db.FirstOrDefault<FuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetFuoriStandardRettifica: " + ex.Message);
            }
            return _indennizzo;
        }

        //public String AnnullaPrestazione(String idFuoriStandard, String codiceCliente, String codiceContratto, String codicePuf, String note, String utente)
        public String AnnullaPrestazione(String idFuoriStandard, DateTime dataInizioAttivita, DateTime dataFineAttivita, String quantita, String quantitaSosp, String fuoriStandard,
               String causale, String sottoCausale, String Utente, String NonIndennizzabile, String CodiceCliente, String CodicePuf, String CodiceContratto, String Note)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                db.BeginTransaction();

                FuoriStandard _indennizzo = GetFuoriStandardRettifica(idFuoriStandard);

                if (_indennizzo.FlagOrigine == "I")
                {

                    PetaPocoParameter[] inStandard = { new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, Convert.ToInt32(idFuoriStandard)),
                                                     new PetaPocoInputParameter("p_COD_CLIENTE", System.Data.DbType.String, _indennizzo.CodCliente),
                                                     new PetaPocoInputParameter("p_COD_PUF", System.Data.DbType.String, _indennizzo.CodPuf),
                                                     new PetaPocoInputParameter("p_COD_CONTRATTO", System.Data.DbType.String, _indennizzo.CodContratto),
                                                     new PetaPocoInputParameter("p_FLAG_ORIGINE", System.Data.DbType.String, _indennizzo.FlagOrigine),
                                                     new PetaPocoInputParameter("p_ID_STANDARD", System.Data.DbType.String, _indennizzo.IdStandard),
                                                     new PetaPocoOutputParameter("p_ID_INDENNIZZO_FS", System.Data.DbType.String, 2500),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500)
                                                  };

                    var resultInStandard = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_INS_INSTANDARD", inStandard);

                    /**********************/
                    if (resultInStandard == null)
                    {
                        throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                    }
                    if (resultInStandard.Result)
                        if (resultInStandard.OutputParams != null)
                        {
                            PetaPocoOutputParameter _p = resultInStandard.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Retc", StringComparison.CurrentCultureIgnoreCase));
                            if (_p != null)
                            {
                                if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                                {
                                    _p = resultInStandard.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Msg", StringComparison.CurrentCultureIgnoreCase));
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
                        throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultInStandard.ErrorMessage));
                    }
                    PetaPocoOutputParameter _idStandard = resultInStandard.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_ID_INDENNIZZO_FS", StringComparison.CurrentCultureIgnoreCase));
                    if (_idStandard != null)
                    {
                        idFuoriStandard = _idStandard.Value.ToString();
                    }
                }
                /**********************/

                if (String.IsNullOrEmpty(idFuoriStandard))
                    idFuoriStandard = _indennizzo.IDFS.ToString();

                var updateRettifica = Sql.Builder.Append("UPDATE GRI_RETTIFICHE_FS SET STORICO = 1 WHERE ID_FUORI_STANDARD = @0", idFuoriStandard);
                db.Execute(updateRettifica);

                var inserisciRettifica = Sql.Builder.Append("INSERT INTO GRI_RETTIFICHE_FS(ID_FUORI_STANDARD,DATA_INIZIO_ATTIVITA,DATA_FINE_ATTIVITA,")
                    .Append("QUANTITA,FLAG_STANDARD,CAUSALE,SOTTOCAUSALE,ESITO,CREATO_IL,AUTORE,STORICO,COD_GRUPPO,DATA_INIZIO_ORIGINALE,DATA_FINE_ORIGINALE,QUANTITA_ORIGINALE,INDENNIZZABILE,CODICE_CLIENTE,CODICE_PUF,CODICE_CONTRATTO,NOTE,QUANTITA_SOSPENSIONE,FLG_IS_RETTIFICA,FLG_STATO)")
                    .Append(" VALUES (@0,@1,@2,@3,@4,@5,@6,0,@7,@8,0,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,1,1)", idFuoriStandard, dataInizioAttivita, dataFineAttivita,
                    quantita.Replace(".", ","), fuoriStandard, causale, sottoCausale, DateTime.Now, Utente, _indennizzo.CodiceGruppo, _indennizzo.DataInizio, _indennizzo.DataFine, _indennizzo.EvasoIn, NonIndennizzabile, CodiceCliente, CodicePuf, CodiceContratto, Note, quantitaSosp.Replace(".", ","));
                db.Execute(inserisciRettifica);

                //db.CompleteTransaction();
            }

            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in Annulla Prestazione: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;

            //catch (Exception ex)
            //{
            //    db.AbortTransaction();
            //    throw new ApplicationException("Impossibile eseguire l'istruzione in AnnullaPrestazione: " + ex.Message);
            //}
            //return String.Empty;
        }

        public ISubCollection<FuoriStandard> GetFuoriStandardFromIDs(List<String> IDFuoriStandard)
        {
            ISubCollection<FuoriStandard> _result = null;
            var _ids = String.Join(",", IDFuoriStandard.ToArray());
            if (String.IsNullOrEmpty(_ids))
                _ids = "''";

            try
            {
                //string.Join(",", IDFuoriStandard.Select(ee => string.Format("{0}", ee)))
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_DA_VALID_V where ID_INDENNIZZO in(" + _ids + ")");

                _result = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetFuoriStandardByID: " + ex.Message);
            }
            return _result;
        }

        public String RettificaPrimoStep(String idFuoriStandard, DateTime dataInizioAttivita, DateTime dataFineAttivita, String quantita, String quantitaSosp, String fuoriStandard,
               String causale, String sottoCausale, String Utente, String NonIndennizzabile, String CodiceCliente, String CodicePuf, String CodiceContratto, String Note, Int32 flgRettifica)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                db.BeginTransaction();

                FuoriStandard _indennizzo = GetFuoriStandardRettifica(idFuoriStandard);

                if (_indennizzo.FlagOrigine == "I")
                {
                    PetaPocoParameter[] inStandard = { new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, Convert.ToInt32(idFuoriStandard)),
                                                     new PetaPocoInputParameter("p_COD_CLIENTE", System.Data.DbType.String, _indennizzo.CodCliente),
                                                     new PetaPocoInputParameter("p_COD_PUF", System.Data.DbType.String, _indennizzo.CodPuf),
                                                     new PetaPocoInputParameter("p_COD_CONTRATTO", System.Data.DbType.String, _indennizzo.CodContratto),
                                                     new PetaPocoInputParameter("p_FLAG_ORIGINE", System.Data.DbType.String, _indennizzo.FlagOrigine),
                                                     new PetaPocoInputParameter("p_ID_STANDARD", System.Data.DbType.String, _indennizzo.IdStandard),
                                                     new PetaPocoOutputParameter("p_ID_INDENNIZZO_FS", System.Data.DbType.String, 2500),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500)
                                                  };

                    var resultInStandard = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_INS_INSTANDARD", inStandard);

                    /**********************/
                    if (resultInStandard == null)
                    {
                        throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                    }
                    if (resultInStandard.Result)
                        if (resultInStandard.OutputParams != null)
                        {
                            PetaPocoOutputParameter _p = resultInStandard.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Retc", StringComparison.CurrentCultureIgnoreCase));
                            if (_p != null)
                            {
                                if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                                {
                                    _p = resultInStandard.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Msg", StringComparison.CurrentCultureIgnoreCase));
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
                        throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultInStandard.ErrorMessage));
                    }

                    PetaPocoOutputParameter _idStandard = resultInStandard.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_ID_INDENNIZZO_FS", StringComparison.CurrentCultureIgnoreCase));
                    if (_idStandard != null)
                    {
                        idFuoriStandard = _idStandard.Value.ToString();
                    }
                }
                /**********************/
                if (String.IsNullOrEmpty(idFuoriStandard))
                    idFuoriStandard = _indennizzo.IDFS.ToString();

                var updateRettifica = Sql.Builder.Append("UPDATE GRI_RETTIFICHE_FS SET STORICO = 1 WHERE ID_FUORI_STANDARD = @0", idFuoriStandard);
                db.Execute(updateRettifica);

                var inserisciRettifica = Sql.Builder.Append("INSERT INTO GRI_RETTIFICHE_FS(ID_FUORI_STANDARD,DATA_INIZIO_ATTIVITA,DATA_FINE_ATTIVITA,")
                    .Append("QUANTITA,FLAG_STANDARD,CAUSALE,SOTTOCAUSALE,ESITO,CREATO_IL,AUTORE,STORICO,COD_GRUPPO,DATA_INIZIO_ORIGINALE,DATA_FINE_ORIGINALE,QUANTITA_ORIGINALE,INDENNIZZABILE,CODICE_CLIENTE,CODICE_PUF,CODICE_CONTRATTO,NOTE,QUANTITA_SOSPENSIONE,FLG_IS_RETTIFICA,FLG_STATO)")
                    .Append(" VALUES (@0,@1,@2,@3,@4,@5,@6,0,@7,@8,0,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,1)", idFuoriStandard, dataInizioAttivita, dataFineAttivita,
                    quantita.Replace(".", ","), fuoriStandard, causale, sottoCausale, DateTime.Now, Utente, _indennizzo.CodiceGruppo, _indennizzo.DataInizio, _indennizzo.DataFine, _indennizzo.EvasoIn, NonIndennizzabile, CodiceCliente, CodicePuf, CodiceContratto, Note, quantitaSosp, flgRettifica);
                db.Execute(inserisciRettifica);

                //db.CompleteTransaction();
            }
            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in Rettifica Primo Step: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;
            //catch (Exception ex)
            //{
            //    db.AbortTransaction();
            //    throw new ApplicationException("Impossibile eseguire l'istruzione in RettificaPrimoStep: " + ex.Message);
            //}
            //return String.Empty;
        }
        public String GetManagerInfo(String CodGruppo)
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT MANAGER_EMAIL FROM GRI_CAPGROUPING_ON_STANDARD WHERE COD_PRESTAZIONE = @0", CodGruppo);
                return db.Query<String>(sql).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile completare l'operazione in GetManagerInfo: " + ex.Message);
            }
        }

        public ISubCollection<RettificaSospensione> GetSospensioniByIDFuoriStandard(String FuoriStandardID)
        {
            ISubCollection<RettificaSospensione> _sospensioni = null;
            IList<RettificaSospensione> sospensioni = new List<RettificaSospensione>();
            try
            {
                var sql = Sql.Builder.Append("select * from GRI_GIN_SOSPENSIONI_V where ID_INDENNIZZO = @0 order by ID_SOSPENSIONE desc, STATO_SOSPENSIONE desc, FLAG_ELIMINATO desc", FuoriStandardID);
                _sospensioni = db.Query<RettificaSospensione>(sql).ToSubCollection<RettificaSospensione>();
                long idSosp = 0;
                foreach (var item in _sospensioni.Items)
                {
                    if (item.ID_SOSPENSIONE == idSosp && item.ID_SOSPENSIONE != 0)
                    {
                        idSosp = item.ID_SOSPENSIONE;
                    }
                    else
                    {
                        sospensioni.Add(item);
                        idSosp = item.ID_SOSPENSIONE;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetSospensioniByIDFuoriStandard: " + ex.Message);
            }
            return sospensioni.Where(x => x.FLAG_ELIMINATO == 0).ToSubCollection();
        }

    }
}
