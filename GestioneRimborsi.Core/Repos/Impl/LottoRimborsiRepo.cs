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
    public class LottoRimborsiRepo : RepositoryBase<Rimborso>, ILottoRimborsiRepo
    {

        public GruppoCap.Core.Data.ISubCollection<Rimborso> LottoRimborsiByUserName(string UserName)
        {
            IEnumerable<Rimborso> _rimborsi = null;

            var sql = Sql.Builder.Append("SELECT PKOPERAZIONE,IBANDEST,ABIORD,CABORD,IMPORTO,DESCRDEST,UTENTE_CONFERMA,CODICE_CLIENTE,BANCA_ORDINANTE,IDRIMBORSO FROM V_TEST_SEPA WHERE UTENTE_CONFERMA = @0", UserName);

            _rimborsi = db.Query<Rimborso>(sql).OrderByDescending(o => o.TotaleEuro);

            return _rimborsi.ToSubCollection<Rimborso>();
        }

        public Rimborso GetById(object id)
        {
            throw new NotImplementedException();
        }

        public GruppoCap.Core.Data.ISubCollection<Rimborso> GetByIds(object[] ids)
        {
            throw new NotImplementedException();
        }

        public GruppoCap.Core.Data.ISubCollection<Rimborso> List()
        {
            throw new NotImplementedException();
        }

        public GruppoCap.Core.IInsertOperationResult Insert(Rimborso entity)
        {
            throw new NotImplementedException();
        }

        public GruppoCap.Core.IUpdateOperationResult Update(Rimborso entity)
        {
            throw new NotImplementedException();
        }

        public GruppoCap.Core.IUpdateOperationResult Update(string UserName, string FileName, string UpdateUser, DateTime DataValuta)
        {
            AnnoBloccato annoBloccato = null;
            db.BeginTransaction();
            try
            {
                var sqlAnno = Sql.Builder.Append("select * from (select * from GRI_ANNI_BLOCCATI_FS ORDER BY ANNO_COMPETENZA DESC) where rownum = 1");
                annoBloccato = db.FirstOrDefault<AnnoBloccato>(sqlAnno);

                var sql = Sql.Builder.Append("Update GESTORE.GIN_INDENNIZZI_CDS c set DT_RIMBORSO = @DataValuta where exists (" +
                    "select b.* from GESTORE.UT_RIMB_TEST a inner join GESTORE.GIN_INDENNIZZI_CDS b on a.numero_Documento = to_char(b.ID_INDENNIZZO) " +
                    "and a.anno_documento = to_char(b.DT_DECORRENZA_INDENNIZZO, 'yyyy') where a.TIPO_DOCUMENTO = 'INDE' " +
                    "and UTENTE_CONFERMA = @UtenteConferma AND FLAG_GENERATO = 'N' and b.ID_INDENNIZZO = c.ID_INDENNIZZO and b.DT_DECORRENZA_INDENNIZZO = c.DT_DECORRENZA_INDENNIZZO AND TIPO_RIMBORSO in ('BON','ASS','BOD') )" +
                    (annoBloccato != null && DateTime.Now >= annoBloccato.DATA_BLOCCO ? " AND (DT_RIMBORSO IS NULL OR to_char(c.DT_DECORRENZA_INDENNIZZO, 'yyyy') > '@AnnoBlocco')" : "AND DT_RIMBORSO IS NULL") + "",
                    new { UtenteConferma = UserName, DataValuta = DataValuta, AnnoBlocco = (annoBloccato != null ? annoBloccato.ANNO_COMPETENZA.ToString() : "") });

                Int32 _result = db.Execute(sql);

                sql = Sql.Builder.Append("UPDATE UT_RIMB_TEST SET FLAG_GENERATO = @FlagGenerato, NOME_FILE_GENERATO=@NomeFile, DATA_GENERAZIONE=@DataGenerazione, UTENTE_GENERAZIONE=@UtenteGeneazione, DATA_VALUTA=@DataValuta WHERE UTENTE_CONFERMA = @UtenteConferma AND FLAG_GENERATO = 'N' and tipo_rimborso in ('BON','ASS','BOD')" +
                    (annoBloccato != null && DateTime.Now >= annoBloccato.DATA_BLOCCO ? " AND (DATA_VALUTA IS NULL OR ANNO_DOCUMENTO > '@AnnoBlocco')" : "AND DATA_VALUTA IS NULL") + "",
                    new { FlagGenerato = "Y", NomeFile = FileName, DataGenerazione = DateTime.Now, UtenteGeneazione = UpdateUser, UtenteConferma = UserName, DataValuta = DataValuta, AnnoBlocco = (annoBloccato != null ? annoBloccato.ANNO_COMPETENZA.ToString() : "") });

                _result = db.Execute(sql);

                db.CompleteTransaction();

                return new UpdateOperationResult(true);
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                return new UpdateOperationResult(false, ex.Message);
            }


        }

        public GruppoCap.Core.IDeleteOperationResult DeleteById(object id)
        {
            throw new NotImplementedException();
        }

        public Rimborso CreateEntityInstance()
        {
            throw new NotImplementedException();
        }


        List<string> ILottoRimborsiRepo.UsersOfRimborsi()
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT DISTINCT UTENTE_CONFERMA FROM V_TEST_SEPA");
                return db.Query<string>(sql).ToList();
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        public string SetDataValuta(DateTime DataValuta, ISubCollection<Rimborso> rimborsi)
        {
            try
            {
                db.BeginTransaction();
                foreach (var item in rimborsi.Items)
                {
                    var items = item.IdRimborso.Split(';');
                    var sql = Sql.Builder.Append("UPDATE UT_RIMB_TEST SET DATA_VALUTA = @0 WHERE (NUMERO_DOCUMENTO = @1 OR ID_RIMBORSO_INTEGRA = @1) AND (ANNO_DOCUMENTO = @2 OR ANNO_RIMBORSO_INTEGRA = @2) ", DataValuta, items[1], items[0]);
                    db.Execute(sql);
                }
                db.CompleteTransaction();
                return String.Empty;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                return ex.Message;
            }
        }

        List<string> ILottoRimborsiRepo.SepaUsers()
        {
            try
            {
                var sql = Sql.Builder.Append("SELECT DISTINCT AUTORE FROM SEPAHEADER WHERE AUTORE IS NOT NULL AND AUTORE != 'Sconosciuto'");
                return db.Query<string>(sql).ToList();
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        public ISubCollection<SepaHeader> GetSepaHeader()
        {
            IEnumerable<SepaHeader> _sepaHeader = null;

            var sql = Sql.Builder.Append("select distinct sh.* from sepaheader sh inner join (select sp.idheader from sepapaymentelements sp inner join SEPACREDITTRANSFERTRANSACTION sc on sp.id=sc.idpayment) pay on sh.id=pay.idheader ORDER BY ID");

            _sepaHeader = db.Query<SepaHeader>(sql);

            return _sepaHeader.ToSubCollection<SepaHeader>();
        }

        public ISubCollection<SepaHeader> GetSepaHeaderByUser(String User)
        {
            IEnumerable<SepaHeader> _sepaHeader = null;

            var sql = Sql.Builder.Append("SELECT * FROM SEPAHEADER WHERE AUTORE = @0 ORDER BY ID", User);

            _sepaHeader = db.Query<SepaHeader>(sql);

            return _sepaHeader.ToSubCollection<SepaHeader>();
        }

        public SepaHeader GetSepaHeaderByTransaction(long idPayment)
        {
            SepaHeader _sepaHeader = null;

            var sql = Sql.Builder.Append("SELECT sh.* FROM SEPAHEADER sh INNER JOIN SEPAPAYMENTELEMENTS sp ON sp.IDHEADER = sh.ID WHERE sp.ID = @0", idPayment);

            _sepaHeader = db.FirstOrDefault<SepaHeader>(sql);

            return _sepaHeader;
        }

        public ISubCollection<SepaCreditTransaction> GetSepaCreditTransaction(long id)
        {
            IEnumerable<SepaCreditTransaction> _sepaTransaction = null;

            var sql = Sql.Builder.Append("SELECT sc.* FROM SEPACREDITTRANSFERTRANSACTION sc INNER JOIN SEPAPAYMENTELEMENTS sp ON sc.IDPAYMENT = sp.ID AND sp.IDHEADER = @0", id);

            _sepaTransaction = db.Query<SepaCreditTransaction>(sql);

            return _sepaTransaction.ToSubCollection<SepaCreditTransaction>();
        }

        public SepaCreditTransaction GetTransactionByID(long id)
        {
            SepaCreditTransaction _sepaTransaction = null;

            var sql = Sql.Builder.Append("SELECT * FROM SEPACREDITTRANSFERTRANSACTION WHERE ID = @0", id);

            _sepaTransaction = db.FirstOrDefault<SepaCreditTransaction>(sql);

            return _sepaTransaction;
        }

        public ISubCollection<SepaCreditTransaction> DeleteSepaCreditTransaction(long id, String autore)
        {
            IEnumerable<SepaCreditTransaction> _sepaTransaction = null;
            try
            {
                db.BeginTransaction();

                var sqlUpdateDeleted = Sql.Builder.Append("UPDATE SEPACREDITTRANSFERTRANSACTION SET ELIMINATO_IL = SYSDATE, ELIMINATO_DA = @0 WHERE ID = @1", autore, id);
                db.Execute(sqlUpdateDeleted);


                var sql = Sql.Builder.Append("SELECT * FROM SEPACREDITTRANSFERTRANSACTION WHERE IDPAYMENT IN (SELECT IDPAYMENT FROM SEPACREDITTRANSFERTRANSACTION WHERE ID= @0)", id);
                _sepaTransaction = db.Query<SepaCreditTransaction>(sql);

                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in DeleteSepaCreditTransaction(): " + ex.Message);
            }

            return _sepaTransaction.ToSubCollection<SepaCreditTransaction>();
        }

        public ISubCollection<SepaCreditTransaction> RecuperaSepaCreditTransaction(long id, String autore)
        {
            IEnumerable<SepaCreditTransaction> _sepaTransaction = null;
            try
            {
                db.BeginTransaction();

                var sqlUpdateChange = Sql.Builder.Append("UPDATE SEPACREDITTRANSFERTRANSACTION SET ELIMINATO_IL = NULL, ELIMINATO_DA = NULL, RECUPERATO_IL = SYSDATE, RECUPERATO_DA = @0 WHERE ID = @1", autore, id);
                db.Execute(sqlUpdateChange);


                var sql = Sql.Builder.Append("SELECT * FROM SEPACREDITTRANSFERTRANSACTION WHERE IDPAYMENT IN (SELECT IDPAYMENT FROM SEPACREDITTRANSFERTRANSACTION WHERE ID= @0)", id);
                _sepaTransaction = db.Query<SepaCreditTransaction>(sql);

                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in RecuperaSepaCreditTransaction(): " + ex.Message);
            }

            return _sepaTransaction.ToSubCollection<SepaCreditTransaction>();
        }

        public ISubCollection<SepaCreditTransaction> ModificaTransazione(long id, String nuovoIban, String nuovoBeneficiario, String motivazione, String autore)
        {
            IEnumerable<SepaCreditTransaction> _sepaTransaction = null;
            try
            {
                db.BeginTransaction();

                SepaCreditTransaction _tr = GetTransactionByID(id);

                var sqlProgressivo = Sql.Builder.Append("SELECT nvl(max(internal_id),0)+1 AS PROGRESSIVO FROM GRI_STORICO_MOD_DISP_BANCARIE");
                var Progressivo = db.SingleOrDefault<Int32>(sqlProgressivo);

                var sqlModifiche = Sql.Builder.Append(@"INSERT INTO GRI_STORICO_MOD_DISP_BANCARIE (INTERNAL_ID, AUTORE, VECCHIO_IBAN, NUOVO_IBAN, VECCHIO_BENEFICIARIO, NUOVO_BENEFICIARIO, MODIFICATO_IL, MOTIVAZIONE, ID_TRANSAZIONE) 
                                                        VALUES (@0,@1,@2,@3,@4,@5,@6,@7,@8)", Progressivo, autore, _tr.CreditorIban, nuovoIban, _tr.CreditorName, nuovoBeneficiario, DateTime.Now, motivazione, id);
                db.Execute(sqlModifiche);

                var sqlUpdateTransaction = Sql.Builder.Append("UPDATE SEPACREDITTRANSFERTRANSACTION SET MODIFICATO_IL = SYSDATE, MODIFICATO_DA = @0, CREDITORIBAN = @1, CREDITORNAME = @2 WHERE ID = @3", autore, nuovoIban, nuovoBeneficiario, id);
                db.Execute(sqlUpdateTransaction);

                var sql = Sql.Builder.Append("SELECT * FROM SEPACREDITTRANSFERTRANSACTION WHERE ID= @0", id);
                _sepaTransaction = db.Query<SepaCreditTransaction>(sql);

                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in ModificaTransazione(): " + ex.Message);
            }

            return _sepaTransaction.ToSubCollection<SepaCreditTransaction>();
        }

        public String ModificaMotivazione(long id, String motivazione)
        {
            String result = String.Empty;
            try
            {
                db.BeginTransaction();

                var sqlProgressivo = Sql.Builder.Append("SELECT nvl(max(internal_id),0) AS PROGRESSIVO FROM GRI_STORICO_MOD_DISP_BANCARIE WHERE ID_TRANSAZIONE = @0", id);
                var Progressivo = db.SingleOrDefault<Int32>(sqlProgressivo);

                var sqlModificaMotivo = Sql.Builder.Append("UPDATE GRI_STORICO_MOD_DISP_BANCARIE SET MOTIVAZIONE = @0 WHERE ID_TRANSAZIONE = @1 AND INTERNAL_ID = @2", motivazione, id, Progressivo);
                db.Execute(sqlModificaMotivo);

                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in ModificaTransazione(): " + ex.Message);
            }

            return result;
        }

        public List<String> GetElencoMotivazioni()
        {
            List<String> _motivazioni = new List<string>();
            try
            {
                var sql = Sql.Builder.Append("SELECT DESCRIZIONE FROM GRI_DISPOSIZIONI_MOTIVAZIONI ORDER BY DESCRIZIONE");
                _motivazioni = db.Query<String>(sql).ToList<String>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetElencoMotivazioni(): " + ex.Message);
            }
            return _motivazioni;
        }

        public String GetMotivazione(long id)
        {
            String _motivazione = String.Empty;
            try
            {
                var sql = Sql.Builder.Append("SELECT MOTIVAZIONE FROM GRI_STORICO_MOD_DISP_BANCARIE WHERE ID_TRANSAZIONE = @0 ORDER BY MODIFICATO_IL DESC", id);
                _motivazione = db.FirstOrDefault<String>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetMotivazione(): " + ex.Message);
            }
            return _motivazione;
        }

        public ISubCollection<DisposizioneModificata> GetDisposizioniModificate(long id)
        {
            ISubCollection<DisposizioneModificata> _disposizioni = null;
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GRI_STORICO_MOD_DISP_BANCARIE WHERE ID_TRANSAZIONE = @0 ORDER BY MODIFICATO_IL DESC", id);
                _disposizioni = db.Query<DisposizioneModificata>(sql).ToSubCollection<DisposizioneModificata>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetDisposizioniModificate(): " + ex.Message);
            }
            return _disposizioni;
        }

        public DisposizioneModificata GetStoricoModifica(Int32 internalId)
        {
            DisposizioneModificata _modifica = null;
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GRI_STORICO_MOD_DISP_BANCARIE WHERE INTERNAL_ID = @0", internalId);
                _modifica = db.FirstOrDefault<DisposizioneModificata>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetStoricoModifica(): " + ex.Message);
            }
            return _modifica;
        }

        public ISubCollection<SepaHeader> BloccaDisposizione(long id, bool sblocca, String autore)
        {
            IEnumerable<SepaHeader> _sepaHeader = null;
            try
            {
                db.BeginTransaction();

                if (sblocca == false)
                {
                    var sqlUpdateBlocco = Sql.Builder.Append("UPDATE SEPAHEADER SET BLOCCATO_IL = SYSDATE, BLOCCATO_DA = @0 WHERE ID = @1", autore, id);
                    db.Execute(sqlUpdateBlocco);
                }
                else
                {
                    var sqlUpdateBlocco = Sql.Builder.Append("UPDATE SEPAHEADER SET BLOCCATO_IL = NULL, BLOCCATO_DA = NULL, SBLOCCATO_IL = SYSDATE, SBLOCCATO_DA = @0 WHERE ID = @1", autore, id);
                    db.Execute(sqlUpdateBlocco);
                }
                var sql = Sql.Builder.Append("SELECT * FROM SEPAHEADER ORDER BY ID");
                _sepaHeader = db.Query<SepaHeader>(sql);

                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in BloccaDisposizione(): " + ex.Message);
            }

            return _sepaHeader.ToSubCollection<SepaHeader>();
        }
        public String GetManagerMail_Iban(String CodGruppo)
        {
            string managermail = string.Empty;

            try
            {

                var sql = Sql.Builder.Append("SELECT MANAGER_EMAIL_IBAN FROM GRI_CAPGROUPING_ON_STANDARD WHERE COD_PRESTAZIONE = @0", CodGruppo);

                managermail = db.Query<String>(sql).FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile completare l'operazione in GetManagerInfo: " + ex.Message);
            }

            return managermail;
        }

    }
}
