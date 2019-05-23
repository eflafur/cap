using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core
{
    public class LottoRimborsiService : ILottoRimborsiService
    {

        ILottoRimborsiRepo _LottoRimborsiRepo = null;

        public LottoRimborsiService(ILottoRimborsiRepo LottoRimborsiRepo)
        {
            _LottoRimborsiRepo = LottoRimborsiRepo;
        }
        GruppoCap.Core.Data.ISubCollection<Rimborso> ILottoRimborsiService.LottoRimborsiByUserName(string UserName)
        {
            return _LottoRimborsiRepo.LottoRimborsiByUserName(UserName);
        }

        List<string> ILottoRimborsiService.UsersOfRimborsi()
        {
            return _LottoRimborsiRepo.UsersOfRimborsi();
        }

        public GruppoCap.Core.IUpdateOperationResult Update(string UserName, string FileName, string UpdateUser, DateTime DataValuta)
        {
            return _LottoRimborsiRepo.Update(UserName, FileName, UpdateUser, DataValuta);
        }

        public string SetDataValuta(DateTime DataValuta, ISubCollection<Rimborso> rimborsi)
        {
            return _LottoRimborsiRepo.SetDataValuta(DataValuta, rimborsi);
        }

        List<string> ILottoRimborsiService.SepaUsers()
        {
            return _LottoRimborsiRepo.SepaUsers();
        }

        public ISubCollection<SepaHeader> GetSepaHeader()
        {
            return _LottoRimborsiRepo.GetSepaHeader();
        }

        public ISubCollection<SepaHeader> GetSepaHeaderByUser(String User)
        {
            return _LottoRimborsiRepo.GetSepaHeaderByUser(User);
        }

        public SepaHeader GetSepaHeaderByTransaction(long idPayment)
        {
            return _LottoRimborsiRepo.GetSepaHeaderByTransaction(idPayment);
        }

        public ISubCollection<SepaCreditTransaction> GetSepaCreditTransaction(long id)
        {
            return _LottoRimborsiRepo.GetSepaCreditTransaction(id);
        }

        public SepaCreditTransaction GetTransactionByID(long id)
        {
            return _LottoRimborsiRepo.GetTransactionByID(id);
        }

        public ISubCollection<SepaCreditTransaction> DeleteSepaCreditTransaction(long id, String autore)
        {
            return _LottoRimborsiRepo.DeleteSepaCreditTransaction(id, autore);
        }

        public ISubCollection<SepaCreditTransaction> RecuperaSepaCreditTransaction(long id, String autore)
        {
            return _LottoRimborsiRepo.RecuperaSepaCreditTransaction(id, autore);
        }

        public ISubCollection<SepaCreditTransaction> ModificaTransazione(long id, String nuovoIban, String nuovoBeneficiario, String motivazione, String autore)
        {
            return _LottoRimborsiRepo.ModificaTransazione(id, nuovoIban, nuovoBeneficiario, motivazione, autore);
        }

        public String ModificaMotivazione(long id, String motivazione)
        {
            return _LottoRimborsiRepo.ModificaMotivazione(id, motivazione);
        }

        public DisposizioneModificata GetStoricoModifica(Int32 internalId)
        {
            return _LottoRimborsiRepo.GetStoricoModifica(internalId);
        }

        public List<String> GetElencoMotivazioni()
        {
            return _LottoRimborsiRepo.GetElencoMotivazioni();
        }

        public String GetMotivazione(long id)
        {
            return _LottoRimborsiRepo.GetMotivazione(id);
        }

        public ISubCollection<DisposizioneModificata> GetDisposizioniModificate(long id)
        {
            return _LottoRimborsiRepo.GetDisposizioniModificate(id);
        }

        public ISubCollection<SepaHeader> BloccaDisposizione(long id, bool sblocca, String autore)
        {
            return _LottoRimborsiRepo.BloccaDisposizione(id, sblocca, autore);
        }
        public String GetManagerMail_Iban(String CodGruppo)
        {
            return _LottoRimborsiRepo.GetManagerMail_Iban(CodGruppo);
        }
    }
}
