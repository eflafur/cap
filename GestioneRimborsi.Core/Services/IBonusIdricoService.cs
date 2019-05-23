
using GestioneRimborsi.Core.Models;
using GruppoCap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core
{
    public interface  IBonusIdricoService: IRevoService
    {
        IEnumerable<BICapLotto> GetLotto(DateTime DataAcquisizione,DateTime DataCarico,DateTime DataScadenza,string Desc ,string file);
        void InsertLotto(BICapLotto lotto);
        IEnumerable<SgateRichieste> GetRichieste(int lottoId);
        List< BICapRequest> GetCapReqS(int id);
        IEnumerable< BICapLotto > ValidaLotto(int id, List<int> reqId = null);
        IEnumerable<BICapLotto> ConfermaLotto(int id);
        BICapLotto getLotById(int lotId);
        SgateRichieste  GetCliente(int id);
        BICapRequest getValidatedRequest(int requestId);
        int  SetIntegra(int id, string integra, List<string> lscode,string codcliente);
        List<BIContratto> GetUtenze(string cliente);
        BIContratto GetUtenza(string utenza);
        string GenerateOutcomeFile(int lotId);
        BICapRequest GetCapReq(int id);
        List<BICapRequest> getLotDetails(QueryOptions options);
        IEnumerable<BICapLotto> GetLotti();
        List<LotProgressInfo> getLotProgress();
        int getLotRequestCount(QueryOptions options);
        Comune getComuneByIstat(string istat);
        List<BiInfoNuoviClienti> GetInfoNuoviClienti(int id);
    }
}
