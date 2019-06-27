using GruppoCap.Authentication.Core;
using GruppoCap.Core.Data;
using System;
using System.Web.Mvc;

namespace GruppoCap.Core.Mvc
{
    public abstract class BaseCapGroupingController : RevoController
    {
        // PRIVATE MEMBERs
        private ICapGroupingService _capGroupingService = null;

        // CTOR
        public BaseCapGroupingController(ICapGroupingService capGroupingService)
        {
            _capGroupingService = capGroupingService;
        }


        //APPLICATION LIST
        public ActionResult List()
        {
            ISubCollection<CapGrouping> _groups;
            _groups = _capGroupingService.ListByApplicationId(Ambient.CurrentApplicationId);

            return View("List", _groups);
        }

        

        // APPLICATION DETAIL
        public ActionResult Detail(String capGroupingId)
        {
            ICapGrouping _grouping;
            _grouping = _capGroupingService.GetById(capGroupingId);

            return View("Detail", _grouping);
        }

        // READY TO DELETE
        public ActionResult ReadyToDelete(String capGroupingId)
        {
            ICapGrouping _grouping;
            _grouping = _capGroupingService.GetById(capGroupingId);

            return View("Delete", _grouping);
        }

        // CREATE
        [HttpPost]
        public JsonResult Create(String capGroupingCode, String description)
        {
            try
            {
                if(capGroupingCode == null)
                    return JsonError("Il codice del raggruppamento non può essere null", "Attenzione!");

                ICapGrouping _g = _capGroupingService.GetByCode(capGroupingCode);
                if (_g != null)
                    return JsonError("CapGrouping CODE già presente nel sistema", "Attenzione!");

                _g = _capGroupingService.InstanceNew();

                _g.CapGroupingCode = capGroupingCode;
                _g.ApplicationId = Ambient.CurrentApplicationId;
                _g.Description = description;
                _g.IsActive = true;
                
                _g.CreationUserId = RevoRequest.CurrentUsername;
                _g.CreationMoment = DateTime.Now;
                _g.LastUpdateUserId = RevoRequest.CurrentUsername;
                _g.LastUpdateMoment = DateTime.Now;

                IInsertOperationResult opRes = _capGroupingService.Create((CapGrouping)_g);
                if (opRes.GenericMeaning)
                {
                    RevoContext.ActivityManager.RegisterCreated<CapGrouping>((CapGrouping)_g);
                    return Json(new { status = "success", data = new { message = "Il raggruppamento è stato creato con successo. Ricarica la pagina per visualizzare le modifiche" } });
                }
                    
                else
                    return JsonError("Qualcosa è andato storto durante la creazione del raggruppamento", "Attenzione!");
            }
            catch (Exception ex)
            {
                return JsonError(ex);
            }
        }

        // UPDATE
        [HttpPost]
        public JsonResult Update(String capGroupingId, String description, Boolean isActive)
        {
            try
            {
                ICapGrouping _g = _capGroupingService.GetById(capGroupingId);

                if (_g == null)
                    return JsonError("Il raggruppamento non è stato trovato", "Attenzione!");

                _g.Description = description;
                _g.IsActive = isActive;
                
                _g.LastUpdateUserId = RevoRequest.CurrentUsername;
                _g.LastUpdateMoment = DateTime.Now;

                IUpdateOperationResult opRes = _capGroupingService.Update((CapGrouping)_g);
                if (opRes.GenericMeaning)
                {
                    RevoContext.ActivityManager.RegisterUpdate<CapGrouping>((CapGrouping)_g);
                    return Json(new { status = "success", data = new { message = "Il raggruppamento è stato modificato con successo" } });
                }
                else
                    return JsonError("Qualcosa è andato storto durante la modifica dell raggruppamento", "Attenzione!");
            }
            catch (Exception ex)
            {
                return JsonError(ex);
            }
        }

        // DELETE
        [HttpPost]
        public JsonResult Delete(String capGroupingId)
        {
            try
            {
                IUpdateOperationResult opRes = _capGroupingService.Deactivate(RevoRequest, capGroupingId);
                if (opRes.GenericMeaning)
                {
                    // RevoContext.ActivityManager.RegisterDelete<CapGrouping>((CapGrouping)_)
                    return Json(new { status = "success", data = new { deletedId = capGroupingId } });
                }
                else
                    return JsonError("Qualcosa è andato storto durante la cancellazione del raggruppamento", "Attenzione!");
            }
            catch (Exception ex)
            {
                return JsonError(ex);
            }
        }
    }
}