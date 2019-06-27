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
    public class RettificaSospensioneRepo : RepositoryBase<RettificaSospensione>, IRettificaSospensioneRepo
    {
        public ISubCollection<RettificaSospensione> GetSospensioniByIDFuoriStandard(String FuoriStandardID, bool edit)
        {
            ISubCollection<RettificaSospensione> _sospensioni = null;
            IList<RettificaSospensione> sospensioni = new List<RettificaSospensione>();
            IList<RettificaSospensione> sospLavorate = new List<RettificaSospensione>();
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
                if (edit)
                {
                    var sospensioniAperte = sospensioni.Where(x => x.FLAG_ELIMINATO == 0).ToSubCollection();
                    var sospensioniResult = sospensioni.Where(x => x.FLAG_ELIMINATO == 0).ToSubCollection();

                    var sqlLavorate = Sql.Builder.Append(@"select ID_SOSPENSIONE,ID_INDENNIZZO,NUMERO_PRESTAZIONE,ID_STANDARD,STATO_SOSPENSIONE,DATA_INIZIO_SOSPENSIONE,DATA_FINE_SOSPENSIONE,DATA_COMUNICAZIONE,cat.DESC_CATEGORIA_SOSPENSIONE CATEGORIA_SOSPENSIONE,tipi.DESC_TIPO_SOSPENSIONE TIPO_SOSPENSIONE,DURATA_SOSPENSIONE,DATA_INS,UTE_INS,DATA_VALERR,UTE_VALERR,NOTE,ROW_ID,FLAG_ELIMINATO,STATUS
                                                            FROM GRI_SOSPENSIONI_FS gri INNER JOIN GRI_TIPI_SOSPENSIONE tipi ON gri.TIPO_SOSPENSIONE = tipi.ID_TIPO_SOSPENSIONE INNER JOIN GRI_CATEGORIE_SOSPENSIONE cat ON gri.CATEGORIA_SOSPENSIONE = cat.ID_CATEGORIA_SOSPENSIONE where ID_INDENNIZZO = @0 and STATUS = 0", FuoriStandardID);
                    sospLavorate = db.Query<RettificaSospensione>(sqlLavorate).ToList<RettificaSospensione>();

                    foreach (var itm in sospLavorate)
                    {
                        foreach (var item in sospensioniAperte.Items)
                        {                            
                            if (item.ROW_ID == itm.ROW_ID || (item.ID_SOSPENSIONE != 0 && item.ID_SOSPENSIONE == itm.ID_SOSPENSIONE))
                            {
                                sospensioniResult.Items.Remove(item);
                                if (!sospensioniResult.Items.Contains(itm))
                                    sospensioniResult.Items.Add(itm);
                            }
                            else if (!sospensioniResult.Items.Contains(itm))
                            {
                                sospensioniResult.Items.Add(itm);
                            }
                        }
                        if (sospensioniAperte.Items.Count == 0 && !sospensioniResult.Items.Contains(itm))
                            sospensioniResult.Items.Add(itm);
                    }
                    return sospensioniResult.Items.Where(x => x.FLAG_ELIMINATO == 0).ToSubCollection();
                }
                else
                {
                    SospendiModifiche(Convert.ToInt32(FuoriStandardID));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetSospensioniByIDFuoriStandard: " + ex.Message);
            }
            return sospensioni.Where(x => x.FLAG_ELIMINATO == 0).ToSubCollection();
        }

        public RettificaSospensione GetSospensioneByID(String RowID)
        {
            RettificaSospensione _sospensione = null;
            try
            {
                var sql = Sql.Builder.Append("select * from gin_sospensioni_cds where ID_SOSPENSIONE = @0", RowID);
                _sospensione = db.FirstOrDefault<RettificaSospensione>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetSospensioneByID: " + ex.Message);
            }
            return _sospensione;
        }

        public IInsertOperationResult AggiornaSospensione(RettificaSospensione sospensione)
        {
            try
            {
                db.BeginTransaction();
                if (sospensione.STATO_SOSPENSIONE == "N")
                {
                    var sql = Sql.Builder.Append("INSERT INTO GRI_SOSPENSIONI_FS VALUES('',@0,@1,@2,'N',@3,@4,@5,@6,@7,'',@9,@10,'','',@8,default,0,0)",
                        sospensione.ID_INDENNIZZO, sospensione.NUMERO_PRESTAZIONE, sospensione.ID_STANDARD, sospensione.DATA_INIZIO_SOSPENSIONE,
                        sospensione.DATA_FINE_SOSPENSIONE, sospensione.DATA_COMUNICAZIONE, sospensione.CATEGORIA_SOSPENSIONE, sospensione.TIPO_SOSPENSIONE,
                        sospensione.NOTE, sospensione.DATA_INS, sospensione.UTE_INS);
                    db.Execute(sql);
                }
                else if (sospensione.STATO_SOSPENSIONE == "R")
                {
                    RettificaSospensione _sospensione = GetSospensioneCategoria(sospensione.ROW_ID.ToString());
                    if (_sospensione != null)
                    {
                        var sql = Sql.Builder.Append("INSERT INTO GRI_SOSPENSIONI_FS VALUES(@11,@0,@1,@2,'R',@3,@4,@5,@6,@7,'','','',@9,@10,@8,default,0,0)",
                        sospensione.ID_INDENNIZZO, sospensione.NUMERO_PRESTAZIONE, sospensione.ID_STANDARD, sospensione.DATA_INIZIO_SOSPENSIONE,
                        sospensione.DATA_FINE_SOSPENSIONE, sospensione.DATA_COMUNICAZIONE, sospensione.CATEGORIA_SOSPENSIONE, sospensione.TIPO_SOSPENSIONE,
                        sospensione.NOTE, sospensione.DATA_INS, sospensione.UTE_INS, sospensione.ID_SOSPENSIONE);
                        db.Execute(sql);                        
                    }
                    else
                    {
                        RettificaSospensione _sospensioneAperta = GetSospensioneModifica(sospensione.ROW_ID.ToString());
                        if (_sospensioneAperta.STATUS == 0)
                        {
                            var sqlUpdate = Sql.Builder.Append(@"UPDATE GRI_SOSPENSIONI_FS SET DATA_INIZIO_SOSPENSIONE = @0, DATA_FINE_SOSPENSIONE = @1, DATA_COMUNICAZIONE = @2,
                                                        CATEGORIA_SOSPENSIONE = @3, TIPO_SOSPENSIONE = @4, NOTE = @5, DATA_VALERR = @6, UTE_VALERR = @7, STATO_SOSPENSIONE = @10 
                                                       WHERE ROW_ID = @8 AND ID_INDENNIZZO = @9",
                               sospensione.DATA_INIZIO_SOSPENSIONE, sospensione.DATA_FINE_SOSPENSIONE, sospensione.DATA_COMUNICAZIONE, sospensione.CATEGORIA_SOSPENSIONE,
                               sospensione.TIPO_SOSPENSIONE, sospensione.NOTE, sospensione.DATA_INS, sospensione.UTE_INS, sospensione.ROW_ID, sospensione.ID_INDENNIZZO, (_sospensioneAperta.ID_SOSPENSIONE != 0 ? "R" : "N"));
                            db.Execute(sqlUpdate);                            
                        }
                        else if (_sospensioneAperta.ID_SOSPENSIONE != 0)
                        {
                            var sql = Sql.Builder.Append("INSERT INTO GRI_SOSPENSIONI_FS VALUES(@11,@0,@1,@2,'R',@3,@4,@5,@6,@7,'','','',@9,@10,@8,default,0,0)",
                        sospensione.ID_INDENNIZZO, sospensione.NUMERO_PRESTAZIONE, sospensione.ID_STANDARD, sospensione.DATA_INIZIO_SOSPENSIONE,
                        sospensione.DATA_FINE_SOSPENSIONE, sospensione.DATA_COMUNICAZIONE, sospensione.CATEGORIA_SOSPENSIONE, sospensione.TIPO_SOSPENSIONE,
                        sospensione.NOTE, sospensione.DATA_INS, sospensione.UTE_INS, _sospensioneAperta.ID_SOSPENSIONE);
                            db.Execute(sql);
                            var sqlStatus = Sql.Builder.Append(@"UPDATE GRI_SOSPENSIONI_FS SET STATUS = 2 WHERE ROW_ID = @0 AND ID_INDENNIZZO = @1",
                           sospensione.ROW_ID, sospensione.ID_INDENNIZZO);
                            db.Execute(sqlStatus);
                        }
                        else
                        {
                            var sql = Sql.Builder.Append("INSERT INTO GRI_SOSPENSIONI_FS VALUES('',@0,@1,@2,'N',@3,@4,@5,@6,@7,'',@9,@10,'','',@8,default,0,0)",
                        sospensione.ID_INDENNIZZO, sospensione.NUMERO_PRESTAZIONE, sospensione.ID_STANDARD, sospensione.DATA_INIZIO_SOSPENSIONE,
                        sospensione.DATA_FINE_SOSPENSIONE, sospensione.DATA_COMUNICAZIONE, sospensione.CATEGORIA_SOSPENSIONE, sospensione.TIPO_SOSPENSIONE,
                        sospensione.NOTE, sospensione.DATA_INS, sospensione.UTE_INS);
                            db.Execute(sql);
                            var sqlStatus = Sql.Builder.Append(@"UPDATE GRI_SOSPENSIONI_FS SET STATUS = 2 WHERE ROW_ID = @0 AND ID_INDENNIZZO = @1",
                           sospensione.ROW_ID, sospensione.ID_INDENNIZZO);
                            db.Execute(sqlStatus);
                        }
                    }
                    //var sqlAllegati = Sql.Builder.Append(@"UPDATE GRI_SOSPENSIONI_ALLEGATI SET ID_SOSPENSIONE = @0 WHERE ID_SOSPENSIONE = @0", sospensione.ROW_ID);
                    //db.Execute(sqlAllegati);
                }
                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                return new InsertOperationResult(sospensione.ID_SOSPENSIONE, false, ex.Message);
            }
            return new InsertOperationResult(sospensione.ID_SOSPENSIONE, true);
        }

        public List<CategoriaSospensione> GetCategorieSospensione(String NumeroPrestazione)
        {
            List<CategoriaSospensione> _categorie = null;
            try
            {
                var sql = Sql.Builder.Append("select sosp.* from gri_categorie_sospensione sosp inner join gri_categorie_prestazione pre on pre.id_categoria_sospensione = sosp.id_categoria_sospensione where pre.cod_prestazione = @0 order by desc_categoria_sospensione asc", NumeroPrestazione);
                _categorie = db.Query<CategoriaSospensione>(sql).ToList<CategoriaSospensione>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCategorieSospensione: " + ex.Message);
            }
            return _categorie;
        }

        public List<TipoSospensione> GetTipiSospensione(String CodCategoria)
        {
            List<TipoSospensione> _tipi = null;
            try
            {
                var sql = Sql.Builder.Append("select tipi.* from gri_tipi_sospensione tipi inner join gri_tipi_categoria cate on cate.id_tipo_sospensione = tipi.id_tipo_sospensione where cate.id_categoria_sospensione = @0 order by desc_tipo_sospensione asc", CodCategoria);
                _tipi = db.Query<TipoSospensione>(sql).ToList<TipoSospensione>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetTipiSospensione: " + ex.Message);
            }
            return _tipi;
        }

        public IUpdateOperationResult AggiornaDurataSospensioni(List<RettificaSospensione> sospensioni)
        {
            try
            {
                db.BeginTransaction();
                foreach (var item in sospensioni)
                {
                    var sql = Sql.Builder.Append("UPDATE GRI_SOSPENSIONI_FS SET DURATA_SOSPENSIONE = @0 WHERE ID_INDENNIZZO = @1 AND ROW_ID= @2", item.DURATA_SOSPENSIONE, item.ID_INDENNIZZO, item.ROW_ID);
                    db.Execute(sql);
                }
                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                return new UpdateOperationResult(false, ex.Message);
            }
            return new UpdateOperationResult(true);
        }

        public void RipristinaSospensioniCancellate(String idFuoriStandard)
        {
            try
            {
                //var sql = Sql.Builder.Append("UPDATE GRI_SOSPENSIONI_FS SET FLAG_ELIMINATO = 0 WHERE ID_INDENNIZZO = @0", idFuoriStandard);
                var sql = Sql.Builder.Append("DELETE FROM GRI_SOSPENSIONI_FS WHERE ID_INDENNIZZO = @0", idFuoriStandard);
                db.Execute(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in RipristinaSospensioniCancellate: " + ex.Message);
            }
        }

        public void ConfermaSospensioniInLavorazione(int idIndennizzo)
        {
            try
            {
                db.BeginTransaction();

                var sql = Sql.Builder.Append("DELETE FROM GRI_SOSPENSIONI_FS WHERE STATUS = 2 AND ID_INDENNIZZO = @0", idIndennizzo);
                db.Execute(sql);

                var sqlUpdate = Sql.Builder.Append("UPDATE GRI_SOSPENSIONI_FS SET STATUS = 1 WHERE STATUS = 0 AND ID_INDENNIZZO = @0", idIndennizzo);
                db.Execute(sqlUpdate);

                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in ConfermaSospensioniInLavorazione: " + ex.Message);
            }
        }

        public void SospendiModifiche(int idIndennizzo)
        {
            try
            {
                db.BeginTransaction();

                var sql = Sql.Builder.Append("DELETE FROM GRI_SOSPENSIONI_FS WHERE STATUS = 0 AND ID_INDENNIZZO = @0", idIndennizzo);
                db.Execute(sql);

                var sqlUpdate = Sql.Builder.Append("UPDATE GRI_SOSPENSIONI_FS SET STATUS = 1 WHERE STATUS = 2 AND ID_INDENNIZZO = @0", idIndennizzo);
                db.Execute(sqlUpdate);

                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in SospendiModifiche: " + ex.Message);
            }
        }

        public void AnnullaSospensione(int idIndennizzo, long idSospensione, long rowID)
        {
            try
            {
                db.BeginTransaction();
                //RettificaSospensione sospensione = GetSospensioneCategoria(rowID.ToString());
                RettificaSospensione sospensione = GetSospensioneCategoria(idSospensione.ToString());
                if (sospensione != null)
                {
                    var sqlInsert = Sql.Builder.Append("INSERT INTO GRI_SOSPENSIONI_FS VALUES(@0,@1,@2,@3,'R',@4,@5,'',@6,@7,@8,@9,@10,'','','',default,1,0)",
                    sospensione.ID_SOSPENSIONE, sospensione.ID_INDENNIZZO, sospensione.NUMERO_PRESTAZIONE, sospensione.ID_STANDARD, sospensione.DATA_INIZIO_SOSPENSIONE,
                    sospensione.DATA_FINE_SOSPENSIONE, sospensione.CATEGORIA_SOSPENSIONE, sospensione.TIPO_SOSPENSIONE, sospensione.DURATA_SOSPENSIONE,
                    sospensione.DATA_INS, sospensione.UTE_INS);
                    db.Execute(sqlInsert);
                }
                else
                {
                    sospensione = GetSospensioneModifica(rowID.ToString());
                    if (sospensione.STATUS == 1)
                    {
                        var sql = Sql.Builder.Append("INSERT INTO GRI_SOSPENSIONI_FS VALUES(@11,@0,@1,@2,'R',@3,@4,@5,@6,@7,'',@9,@10,'','',@8,default,1,0)",
                        sospensione.ID_INDENNIZZO, sospensione.NUMERO_PRESTAZIONE, sospensione.ID_STANDARD, sospensione.DATA_INIZIO_SOSPENSIONE,
                        sospensione.DATA_FINE_SOSPENSIONE, sospensione.DATA_COMUNICAZIONE, sospensione.CATEGORIA_SOSPENSIONE, sospensione.TIPO_SOSPENSIONE,
                        sospensione.NOTE, sospensione.DATA_INS, sospensione.UTE_INS, sospensione.ID_SOSPENSIONE);
                        db.Execute(sql);
                        var sqlStatus = Sql.Builder.Append(@"UPDATE GRI_SOSPENSIONI_FS SET STATUS = 2 WHERE ROW_ID = @0 AND ID_INDENNIZZO = @1",
                           sospensione.ROW_ID, sospensione.ID_INDENNIZZO);
                        db.Execute(sqlStatus);
                    }
                    else
                    {
                        var sqlUpdate = Sql.Builder.Append("UPDATE GRI_SOSPENSIONI_FS SET FLAG_ELIMINATO = 1, STATUS = 0 WHERE ID_INDENNIZZO = @0 AND ROW_ID = @1", idIndennizzo, rowID);
                        db.Execute(sqlUpdate);
                    }
                }
                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in AnnullaSospensione: " + ex.Message);
            }
        }

        public RettificaSospensione GetSospensioneCategoria(String RowID)
        {
            RettificaSospensione _sospensione = null;
            try
            {
                var sql = Sql.Builder.Append(@"select ID_SOSPENSIONE, ID_INDENNIZZO,NUMERO_PRESTAZIONE,ID_STANDARD,STATO_SOSPENSIONE,DATA_INIZIO_SOSPENSIONE,DATA_FINE_SOSPENSIONE,DATA_COMUNICAZIONE,cat.ID_CATEGORIA_SOSPENSIONE CATEGORIA_SOSPENSIONE,tipi.ID_TIPO_SOSPENSIONE TIPO_SOSPENSIONE,DURATA_SOSPENSIONE,
                                               ID_SOSPENSIONE_DWH,DATA_INS,UTE_INS,ERR_DATA_INIZIO_SOSPENSIONE,ERR_DATA_FINE_SOSPENSIONE,ERR_DATA_COMUNICAZIONE,ERR_CATEGORIA_SOSPENSIONE,ERR_TIPO_SOSPENSIONE,ERR_DURATA_SOSPENSIONE,FLG_ERRORE,DATA_VALERR,UTE_VALERR,NOTE
                                             FROM gin_sospensioni_cds cds INNER JOIN GRI_TIPI_SOSPENSIONE tipi ON tipi.DESC_TIPO_SOSPENSIONE = cds.TIPO_SOSPENSIONE
                                             INNER JOIN GRI_CATEGORIE_SOSPENSIONE cat ON cds.CATEGORIA_SOSPENSIONE = cat.DESC_CATEGORIA_SOSPENSIONE where ID_SOSPENSIONE = @0", RowID);
                _sospensione = db.FirstOrDefault<RettificaSospensione>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetSospensioneCategoria: " + ex.Message);
            }
            return _sospensione;
        }

        public RettificaSospensione GetSospensioneModifica(String RowID)
        {
            RettificaSospensione _sospensione = null;
            try
            {
                var sqlAperta = Sql.Builder.Append(@"select ID_INDENNIZZO,CATEGORIA_SOSPENSIONE,DATA_COMUNICAZIONE,DATA_FINE_SOSPENSIONE,DATA_INIZIO_SOSPENSIONE,NOTE,ROW_ID,STATO_SOSPENSIONE,TIPO_SOSPENSIONE, ID_SOSPENSIONE
                                               FROM GRI_SOSPENSIONI_FS where ROW_ID = @0 AND STATUS = 0", RowID);
                _sospensione = db.FirstOrDefault<RettificaSospensione>(sqlAperta);
                if (_sospensione == null)
                {
                    var sql = Sql.Builder.Append(@"select ID_INDENNIZZO, cat.ID_CATEGORIA_SOSPENSIONE CATEGORIA_SOSPENSIONE,DATA_COMUNICAZIONE,DATA_FINE_SOSPENSIONE,DATA_INIZIO_SOSPENSIONE,NOTE,ROW_ID,STATO_SOSPENSIONE,tipi.ID_TIPO_SOSPENSIONE TIPO_SOSPENSIONE, STATUS, NUMERO_PRESTAZIONE, ID_STANDARD, DATA_INS, UTE_INS, ID_SOSPENSIONE
                                               FROM GRI_GIN_SOSPENSIONI_V cds INNER JOIN GRI_TIPI_SOSPENSIONE tipi ON tipi.DESC_TIPO_SOSPENSIONE = cds.TIPO_SOSPENSIONE
                                               INNER JOIN GRI_CATEGORIE_SOSPENSIONE cat ON cds.CATEGORIA_SOSPENSIONE = cat.DESC_CATEGORIA_SOSPENSIONE where ROW_ID = @0", RowID);
                    _sospensione = db.FirstOrDefault<RettificaSospensione>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetSospensioneModifica: " + ex.Message);
            }
            return _sospensione;
        }

        public ISubCollection<AllegatoSospensione> GetElencoAllegatiSosp(long IdSospensione)
        {
            ISubCollection<AllegatoSospensione> _list = null;
            try
            {
                var sql = Sql.Builder.Append("select * from gri_sospensioni_allegati where ID_SOSPENSIONE = @0", IdSospensione);
                _list = db.Query<AllegatoSospensione>(sql).ToSubCollection<AllegatoSospensione>();
                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetElencoAllegatiSosp: " + ex.Message);
            }
        }
        public bool DeleteAllegatoSosp(String NomeFile, String ServerPath, String TipoFile)
        {
            try
            {
                db.BeginTransaction();

                var sql = Sql.Builder.Append("DELETE FROM gri_sospensioni_allegati WHERE NOME_FILE = @0", NomeFile);
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

        public String AggiungiFileSosp(long IdSospensione, String IdFs, System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String Utente)
        {
            try
            {
                db.BeginTransaction();

                //select PROGRESSIVO con SELECT MAX
                var sqlProgressivo = Sql.Builder.Append("SELECT nvl(max(Progressivo),0)+1 AS PROGRESSIVO FROM GRI_SOSPENSIONI_ALLEGATI WHERE NOME_ORIGINALE = @0", NomefileOriginale);
                var Progressivo = db.SingleOrDefault<Int16>(sqlProgressivo);

                String NomeFile = String.Format("{0}_{1}", NomefileOriginale, Progressivo).Replace("'", " ");

                var sqlAllegati = Sql.Builder.Append("INSERT INTO GRI_SOSPENSIONI_ALLEGATI(PROGRESSIVO,NOME_FILE,ESTENSIONE,DIMENSIONE,DATA_INSERIMENTO,UTENTE_INSERIMENTO,IDFS,NOME_ORIGINALE,ID_SOSPENSIONE)")
                .Append(" VALUES (@0,@1,@2,@3,@4,@5,@6,@7,@8)", Progressivo, NomeFile, Extension, file.Length, DateTime.Now, Utente, IdFs, NomefileOriginale, IdSospensione);
                db.Execute(sqlAllegati);

                string percorso = ServerPath + NomeFile + Extension;
                byte[] bytesInStream = new byte[file.Length];
                file.Read(bytesInStream, 0, bytesInStream.Length);

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
                throw new ApplicationException("Impossibile eseguire l'istruzione in AggiungiFileSosp: " + ex.Message);
            }
        }

    }
}
