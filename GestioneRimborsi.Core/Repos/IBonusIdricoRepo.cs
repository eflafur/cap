using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core
{
    public interface IBonusIdricoRepo : IRepository<Lotto>
    {
        /*Actions*/
        void ValidateLot(int lotId);
        void ConfirmLot(int lotId);
        void InsertSgateReq(SgateRichieste req);
        void InsertCapReq(BICapRequest Lotto);
        int InsertCapLotto(BICapLotto lotto);
        void UpdateSgateReq(string table, string pk, SgateRichieste nodo);
        void UpdateCapLotto(string table, string pk, BICapLotto nodo);
        void UpdateCapReq(string table, string pk, BICapRequest nodo);
        void DeleteCapReq(int Lotto);

        IEnumerable<BICapLotto> getRequestLots(int? id);        
        BICapRequest getCapRequestById(int id);
        IEnumerable<SgateRichieste> getOriginalSGATeRequests(int? id, string sql = null);

        List<BICapRequest> getCAPRequestById(int id);
        
        IEnumerable<BICapRequest> getCapRequestsByLot(int lotId);

        /*single Entities*/
        SgateRichieste getOriginalSGATeRequestById(int id);
        BICapLotto getRequestLotById(int id);
        BIComAnagrafica getCustomerByIntegraCode(string cliente);
        string GetContratto(string sql);


        List<BICapRequest> getLotDetails(Models.QueryOptions options);
        int getLotRequestCount(Models.QueryOptions options);


        BICLienti GetCliente(string sql);

        BIPuf GetPuf(string sql);
        T GetPufGeneric<T>(T sql) where T : IEntity;
        List<BIContratto> GetContrattoByCliente(string sql);
        IUpdateOperationResult CalcoloBonusIdrico(int lotId);
        T CapReqByID<T>(int id) where T : IEntity;
        BIComAnagrafica CodCLienteByIntegra(string integra);

        /*Utilities */
        List<LotProgressInfo> getLotProgress();
        Comune getComuneByIstat(string istat);
        void UpdateIntegraTable(SgateRichieste sgreq, BICapRequest bicapreq, BICapLotto lotto);

       
        List<BiInfoNuoviClienti> GetInfoNuoviClienti(int id);

        BIRequestValidate GetRequestValidateById(int id);
        // List<BICapRequest> getCapRequestsByLot<T>(int lotto);
    }

    //public interface IBiInfoRepo : IRepository<BiInfoNuoviClienti>
    //{
    //    ISubCollection<BiInfoNuoviClienti> GetByLotId(Int32 LotId);
    //}

}
