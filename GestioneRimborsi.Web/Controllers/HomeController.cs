using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GruppoCap.Core.Mvc;
using GestioneRimborsi.Core;

namespace GestioneRimborsi.Web.Controllers
{
    public class HomeController : BaseHomeController
    {
        IBonusIdricoService bis;

        public HomeController(IBonusIdricoService _bis)
        {
            this.bis = _bis;
        }

        public ActionResult Bi()
        {
            //        GestioneRimborsi.Core.Process.Xml2Struct.ReadXml();
            return View();
        }

        //public JsonResult biacquisizione(int? lotto)
        //{
        //    var model = bis.GetLotto();
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult sgateValidate(int lotto)
        {
            var model = bis.ValidaLotto(lotto);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetLotti()
        //{
        //    var model = bis.GetLotto();
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult GetCapReqS(int lotto)
        {
            var model = bis.GetCapReqS(lotto);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}