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
    public class AnniBloccatiRepo : RepositoryBase<AnnoBloccato>, IAnniBloccatiRepo
    {
        public ISubCollection<AnnoBloccato> GetAnniBloccati()
        {
            ISubCollection<AnnoBloccato> _anniBloccati = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GRI_ANNI_BLOCCATI_FS ORDER BY ANNO_COMPETENZA DESC");
                _anniBloccati = db.Query<AnnoBloccato>(sql).ToSubCollection<AnnoBloccato>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetAnniBloccati: " + ex.Message);
            }
            return _anniBloccati;
        }

        public AnnoBloccato GetAnnoBloccato()
        {
            AnnoBloccato _annoBloccato = null;
            try
            {
                var sql = Sql.Builder.Append("select * from (select * from GRI_ANNI_BLOCCATI_FS where DATA_BLOCCO + 365 >= SYSDATE ORDER BY ANNO_COMPETENZA DESC) where rownum = 1");
                _annoBloccato = db.FirstOrDefault<AnnoBloccato>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetAnnoBloccato: " + ex.Message);
            }
            return _annoBloccato;
        }

        public AnnoBloccato GetLastAnnoBloccato()
        {
            AnnoBloccato _annoBloccato = null;
            try
            {
                var sql = Sql.Builder.Append("select * from (select * from GRI_ANNI_BLOCCATI_FS ORDER BY ANNO_COMPETENZA DESC) where rownum = 1");
                _annoBloccato = db.FirstOrDefault<AnnoBloccato>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetLastAnnoBloccato: " + ex.Message);
            }
            return _annoBloccato;
        }

        public void InsertDataBlocco(string annoCompetenza, DateTime dataBlocco, String utente)
        {
            AnnoBloccato annoBloccato = new AnnoBloccato();
            annoBloccato.ANNO_COMPETENZA = Convert.ToInt32(annoCompetenza);
            annoBloccato.DATA_BLOCCO = dataBlocco;
            annoBloccato.DATA_INSERIMENTO = DateTime.Now;
            annoBloccato.UTENTE_INSERIMENTO = utente;
            try
            {
                db.BeginTransaction();
                db.Insert(annoBloccato);
                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in InsertDataBlocco: " + ex.Message);
            }
        }

        public void UpdateDataBlocco(string annoCompetenza, DateTime dataBlocco, String utente)
        {
            AnnoBloccato annoBloccato = new AnnoBloccato();
            try
            {
                var sql = Sql.Builder.Append("select * from (select * from GRI_ANNI_BLOCCATI_FS where ANNO_COMPETENZA = @0 ORDER BY ANNO_COMPETENZA DESC) where rownum = 1", annoCompetenza);
                annoBloccato = db.FirstOrDefault<AnnoBloccato>(sql);

                annoBloccato.DATA_BLOCCO = dataBlocco;
                annoBloccato.UTENTE_MODIFICA = utente;
                annoBloccato.DATA_MODIFICA = DateTime.Now;

                db.BeginTransaction();
                db.Update("GRI_ANNI_BLOCCATI_FS", "ANNO_COMPETENZA", annoBloccato, annoBloccato.ANNO_COMPETENZA);
                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in UpdateDataBlocco: " + ex.Message);
            }
        }

        public bool CheckIsBlocked(String idFuoriStandard)
        {
            FuoriStandard fuoriStandard = null;
            AnnoBloccato annoBloccato = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_DA_VALID_V WHERE ID_INDENNIZZO = @0", idFuoriStandard);
                fuoriStandard = db.FirstOrDefault<FuoriStandard>(sql);
                if (fuoriStandard == null)
                {
                    var sqlInStandard = Sql.Builder.Append("select * from GIN_FUORI_STD_DA_VALID_FS_V WHERE ID_INDENNIZZO = @0", idFuoriStandard);
                    fuoriStandard = db.FirstOrDefault<FuoriStandard>(sqlInStandard);
                }

                var sqlAnno = Sql.Builder.Append("select * from (select * from GRI_ANNI_BLOCCATI_FS WHERE to_char(DATA_BLOCCO, 'YYYY') <= to_char(sysdate, 'YYYY') ORDER BY ANNO_COMPETENZA DESC) WHERE ROWNUM = 1");
                annoBloccato = db.FirstOrDefault<AnnoBloccato>(sqlAnno);

                if (fuoriStandard != null && annoBloccato != null)
                {
                    if (DateTime.Now >= annoBloccato.DATA_BLOCCO)
                    {
                        if (fuoriStandard.DataFine != null && fuoriStandard.DataFine.Value.Year <= annoBloccato.ANNO_COMPETENZA)
                            return true;
                        else if (fuoriStandard.DataFine != null && fuoriStandard.DataFine.Value.Year == (annoBloccato.ANNO_COMPETENZA + 1))
                        {
                            if (fuoriStandard.Provenienza != "SAFO")
                                return true;
                        }
                    }
                    else if (fuoriStandard.DataFine != null && fuoriStandard.DataFine.Value.Year < annoBloccato.ANNO_COMPETENZA)
                        return true;
                }
                
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CheckIsBlocked: " + ex.Message);
            }
            return false;
        }

        public bool CheckIsBlockedApprovazione(String idFuoriStandard, String ErrDataInizio, String ErrDataFine)
        {
            FuoriStandard fuoriStandard = null;
            AnnoBloccato annoBloccato = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_DA_VALID_V WHERE ID_INDENNIZZO = @0", idFuoriStandard);
                fuoriStandard = db.FirstOrDefault<FuoriStandard>(sql);
                if (fuoriStandard == null)
                {
                    var sqlInStandard = Sql.Builder.Append("select * from GIN_FUORI_STD_DA_VALID_FS_V WHERE ID_INDENNIZZO = @0", idFuoriStandard);
                    fuoriStandard = db.FirstOrDefault<FuoriStandard>(sqlInStandard);
                }

                var sqlAnno = Sql.Builder.Append("select * from (select * from GRI_ANNI_BLOCCATI_FS WHERE to_char(DATA_BLOCCO, 'YYYY') <= to_char(sysdate, 'YYYY') ORDER BY ANNO_COMPETENZA DESC) WHERE ROWNUM = 1");
                annoBloccato = db.FirstOrDefault<AnnoBloccato>(sqlAnno);

                if (fuoriStandard != null && annoBloccato != null)
                {
                    var dataInizio = fuoriStandard.DataInizio;
                    var dataFine = fuoriStandard.DataFine;
                    var errDataInizio = Convert.ToDateTime(ErrDataInizio);
                    var errDataFine = Convert.ToDateTime(ErrDataFine);
                    bool isBetween = false;
                    if (errDataInizio != null)
                        dataInizio = errDataInizio;
                    if (errDataFine != null)
                        dataFine = errDataFine;

                    if (DateTime.Now >= annoBloccato.DATA_BLOCCO)
                    {
                        if (dataFine != null)
                        { 
                            if (dataFine.Value.Year <= annoBloccato.ANNO_COMPETENZA)
                                return true;
                            else if (dataFine.Value.Year == (annoBloccato.ANNO_COMPETENZA + 1))
                            {
                                if (fuoriStandard.Provenienza != "SAFO")
                                    return true;
                            }
                        }                      

                        if (fuoriStandard.Provenienza == "SAFO")
                        {
                            if (dataFine != null && (dataFine.Value.Year - annoBloccato.ANNO_COMPETENZA) == 1)
                            {
                                if (dataInizio != null && dataInizio.Value.Year == (dataFine.Value.Year - 1))
                                    isBetween = true;
                            }

                            if (isBetween)
                            {
                                var dataInizioOriginal = fuoriStandard.DataInizio;
                                var dataFineOriginal = fuoriStandard.DataFine;                               
                                if (dataInizioOriginal != null && dataInizio.Value.Year != dataInizioOriginal.Value.Year)
                                    return true;
                                if (dataFineOriginal != null && dataFine.Value.Year != dataFineOriginal.Value.Year)
                                    return true;
                            }
                        }
                    }
                    else if (dataFine != null && dataFine.Value.Year < annoBloccato.ANNO_COMPETENZA)
                        return true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CheckIsBlockedApprovazione: " + ex.Message);
            }
            return false;
        }
    }
}

