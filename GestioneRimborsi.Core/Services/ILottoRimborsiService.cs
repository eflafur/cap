using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface ILottoRimborsiService : IRevoService
    {
        ISubCollection<Rimborso> LottoRimborsiByUserName(string UserName);
        List<string> UsersOfRimborsi();
        IUpdateOperationResult Update(string UserName, string FileName, string UpdateUser, DateTime DataValuta);
        string SetDataValuta(DateTime DataValuta, ISubCollection<Rimborso> rimborsi);
        List<string> SepaUsers();
        ISubCollection<SepaHeader> GetSepaHeader();
        ISubCollection<SepaHeader> GetSepaHeaderByUser(String User);
        SepaHeader GetSepaHeaderByTransaction(long idPayment);
        ISubCollection<SepaCreditTransaction> GetSepaCreditTransaction(long id);
        SepaCreditTransaction GetTransactionByID(long id);
        ISubCollection<SepaCreditTransaction> DeleteSepaCreditTransaction(long id, String autore);
        ISubCollection<SepaCreditTransaction> RecuperaSepaCreditTransaction(long id, String autore);
        ISubCollection<SepaCreditTransaction> ModificaTransazione(long id, String nuovoIban, String nuovoBeneficiario, String motivazione, String autore);
        String ModificaMotivazione(long id, String motivazione);
        DisposizioneModificata GetStoricoModifica(Int32 internalId);
        List<String> GetElencoMotivazioni();
        String GetMotivazione(long id);
        ISubCollection<DisposizioneModificata> GetDisposizioniModificate(long id);
        ISubCollection<SepaHeader> BloccaDisposizione(long id, bool sblocca, String autore);
        String GetManagerMail_Iban(String CodGruppo);

    }
}
