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
    public class AllegatoRimborsoRepo : RepositoryBase<AllegatoRimborso>, IAllegatoRimborsoRepo
    {
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
    }
}
