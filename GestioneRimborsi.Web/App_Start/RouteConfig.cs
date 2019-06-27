using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GruppoCap.Core.Mvc;

namespace GestioneRimborsi.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            CommonRoutes.RegisterRevoRoutes(routes);


            #region PLANNING SLOTs

            routes.MapRoute(
                name: "planningslots",
                url: "planning-slots",
                defaults: new { controller = "PlanningSlot", action = "List" }
            );

            routes.MapRoute(
                name: "planningslots-inner-list",
                url: "planning-slots-inner-list",
                defaults: new { controller = "PlanningSlot", action = "InnerList" }
            );

            routes.MapRoute(
                name: "planningslots-search",
                url: "planning-slots/search/",
                defaults: new { controller = "PlanningSlot", action = "Search" }
            );

            routes.MapRoute(
                name: "planningslots-create",
                url: "planning-slots/create",
                defaults: new { controller = "PlanningSlot", action = "Create" }
            );

            routes.MapRoute(
                name: "planningslot-detail",
                url: "planning-slot/{planningSlotId}",
                defaults: new { controller = "PlanningSlot", action = "Detail" }
            );

            routes.MapRoute(
                name: "planningslot-preview",
                url: "planning-slot/{planningSlotId}/preview",
                defaults: new { controller = "PlanningSlot", action = "Preview" }
            );

            routes.MapRoute(
                name: "planningslot-update",
                url: "planning-slot/{planningSlotId}/update",
                defaults: new { controller = "PlanningSlot", action = "Update" }
            );

            routes.MapRoute(
                name: "planningslot-ready-to-delete",
                url: "planning-slot/{planningSlotId}/delete-confirm",
                defaults: new { controller = "PlanningSlot", action = "ReadyToDelete" }
            );

            routes.MapRoute(
                name: "planningslot-delete",
                url: "planning-slot/{planningSlotId}/delete",
                defaults: new { controller = "PlanningSlot", action = "Delete" }
            );

            routes.MapRoute(
                name: "planningslot-extract",
                url: "planning-slot/{planningSlotId}/extract",
                defaults: new { controller = "UtenzaLettura", action = "Extract" }
            );

            routes.MapRoute(
                name: "planningslot-download",
                url: "planning-slot/{planningSlotId}/download/{filename}",
                defaults: new { controller = "UtenzaLettura", action = "Download" }
            );

            #endregion

            #region SLOT SCHEDULEs

            routes.MapRoute(
                name: "slotschedules",
                url: "slot-schedules",
                defaults: new { controller = "SlotSchedule", action = "SlotSelection" }
            );

            routes.MapRoute(
                name: "slotschedules-search",
                url: "slot-schedules/search/",
                defaults: new { controller = "SlotSchedule", action = "SearchForSchedule" }
            );

            routes.MapRoute(
                name: "slotschedules-schedule",
                url: "planning-slot/{planningSlotId}/schedule",
                defaults: new { controller = "SlotSchedule", action = "Schedule" }
            );

            routes.MapRoute(
                name: "slotschedules-create",
                url: "planning-slot/{planningSlotId}/schedule/create",
                defaults: new { controller = "SlotSchedule", action = "Create" }
            );

            routes.MapRoute(
                name: "slotschedules-delete",
                url: "planning-slot/{planningSlotId}/schedule/{slotScheduleId}/delete",
                defaults: new { controller = "SlotSchedule", action = "Delete" }
            );

            #endregion

            #region COMUNI CAP

            routes.MapRoute(
                name: "comuni-cap",
                url: "comuni-cap",
                defaults: new { controller = "ComuneCAP", action = "Index" }
            );

            routes.MapRoute(
                name: "comuni-cap-json",
                url: "comuni-cap/json",
                defaults: new { controller = "ComuneCAP", action = "ComuniJsonList" }
            );

            #endregion

            #region RISULTATI TOUR DI LETTURA

            routes.MapRoute(
                name: "nuova-importazione-tour-lettura",
                url: "nuova-importazione",
                defaults: new { controller = "TourLettura", action = "New" }
            );

            #endregion

            #region LOTTO RIMBORSI
            routes.MapRoute(
                name: "lottorimborsi-list",
                url: "lottorimborsi-list",
                defaults: new { controller = "LottoRimborsi", action = "List" }
            );

            routes.MapRoute(
                name: "lottorimborsi-inner-list",
                url: "lottorimborsi-inner-list/{UserName}",
                defaults: new { controller = "LottoRimborsi", action = "InnerList", UserName = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "lottorimborsi-genera-file",
              url: "lottorimborsi-genera-file/{UserName}",
              defaults: new { controller = "LottoRimborsi", action = "GeneraFileRimborsi", UserName = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "lottorimborsi-download-file",
              url: "lottorimborsi-download-file",
              defaults: new { controller = "LottoRimborsi", action = "DownloadFileRimborsi" }
            );

            routes.MapRoute(
              name: "lottorimborsi-setDataValuta",
              url: "LottoRimborsi/SetDataValuta",
              defaults: new { controller = "LottoRimborsi", action = "SetDataValuta" }
            );
            #endregion

            #region BONUS IDRICO
            routes.MapRoute(
           name: "Carica pagina per BI",
           url: "BonusIdrico",
           defaults: new { controller = "BonusIdrico", action = "Bi" }
         );
            routes.MapRoute(
                name: "getRequestValidationDetails",
                url: "getRequestValidationDetails",
                defaults: new { controller = "BonusIdrico", action = "getRequestValidationDetails" })
                ;
            routes.MapRoute(
                name: "getLotDetails",
                url: "getLotDetails",
                defaults: new { controller = "BonusIdrico", action = "getLotDetails" })
                ;
            routes.MapRoute(
                name: "getLotProgress",
                url: "getLotProgress",
                defaults: new { controller = "BonusIdrico", action = "getLotProgress" })
                ;
            routes.MapRoute(
             name: "Carica - Lotti",
             url: "getlotti",
             defaults: new { controller = "BonusIdrico", action = "GetLotti" }
           );

            routes.MapRoute(
              name: "valida-bonus-idrico",
              url: "biacquisizione",
              defaults: new { controller = "BonusIdrico", action = "Acquisizione" }
            );

            routes.MapRoute(
              name: "Validazione-Sgate",
              url: "sgatevalidate",
              defaults: new { controller = "BonusIdrico", action = "SgateValidate" }
            );

            routes.MapRoute(
              name: "Conferma-Lotto",
              url: "lotConfirm",
              defaults: new { controller = "BonusIdrico", action = "LotConfirm" }
            );

            routes.MapRoute(
              name: "bonus-idrico",
              url: "getcapreqs",
              defaults: new { controller = "BonusIdrico", action = "GetCapReqS" }
            );

            routes.MapRoute(
             name: "richiesta-dati-utente",
             url: "getclient",
             defaults: new { controller = "BonusIdrico", action = "GetClient" }
           );

            routes.MapRoute(
          name: "partial-view-fornitura",
          url: "getfornitura",
          defaults: new { controller = "BonusIdrico", action = "GetFornitura" }
        );

            routes.MapRoute(
            name: "forza -fornitura",
            url: "forceintegra",
            defaults: new { controller = "BonusIdrico", action = "ForceIntegra" }
           );


            routes.MapRoute(
            name: "estrae dati contratto utente",
            url: "getcontrattodetail",
            defaults: new { controller = "BonusIdrico", action = "GetcontrattoDetail" }
           );

            routes.MapRoute(
           name: "estrae singola utenza",
           url: "getutenza",
           defaults: new { controller = "BonusIdrico", action = "GetUtenza" }
          );

            routes.MapRoute(
             name: "Uploadfile",
             url: "upload",
             defaults: new { controller = "BonusIdrico", action = "UploadFile" }
            );

            routes.MapRoute(
             name: "Downloadfile",
             url: "download/{id}",
             defaults: new { controller = "BonusIdrico", action = "DownloadOutcome", id = UrlParameter.Optional }
            );

            routes.MapRoute(
             name: "loadExcelNuoviClienti",
             url: "loadExcelNuoviClienti",
             defaults: new { controller = "BonusIdrico", action = "loadExcelNuoviClienti" }
            );

            #endregion

            #region GESTIONE DISPOSIZIONI

            routes.MapRoute(
                name: "gestioneDisposizioni-list",
                url: "gestioneDisposizioni-list",
                defaults: new { controller = "GestioneDisposizioni", action = "List" }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-searchSepaHeader",
                url: "gestioneDisposizioni-searchSepaHeader",
                defaults: new { controller = "GestioneDisposizioni", action = "SearchSepaHeader" }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-creditTransaction",
                url: "gestioneDisposizioni/SearchSepaCreditTransaction",
                defaults: new { controller = "GestioneDisposizioni", action = "SearchSepaCreditTransaction" }
            );

            routes.MapRoute(
              name: "gestioneDisposizioni-download-file",
              url: "gestioneDisposizioni-download-file",
              defaults: new { controller = "GestioneDisposizioni", action = "DownloadFileDisposizioni" }
            );

            routes.MapRoute(
              name: "gestioneDisposizioni-download-csv",
              url: "gestioneDisposizioni-download-csv",
              defaults: new { controller = "GestioneDisposizioni", action = "DownloadCsvDisposizioni" }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-inner-list",
                url: "gestioneDisposizioni-inner-list/{UserName}",
                defaults: new { controller = "GestioneDisposizioni", action = "InnerList", UserName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-deleteTransaction",
                url: "gestioneDisposizioni/DeleteSepaCreditTransaction",
                defaults: new { controller = "GestioneDisposizioni", action = "DeleteSepaCreditTransaction" }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-recuperaSepaCreditTransaction",
                url: "gestioneDisposizioni/RecuperaSepaCreditTransaction",
                defaults: new { controller = "GestioneDisposizioni", action = "RecuperaSepaCreditTransaction" }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-schedaModifiche",
                url: "gestioneDisposizioni/SchedaModifiche",
                defaults: new { controller = "GestioneDisposizioni", action = "SchedaModifiche" }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-schedaElencoModifiche",
                url: "gestioneDisposizioni/SchedaElencoModifiche",
                defaults: new { controller = "GestioneDisposizioni", action = "SchedaElencoModifiche" }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-modificaDisposizioni",
                url: "gestioneDisposizioni/ModificaDisposizioni",
                defaults: new { controller = "GestioneDisposizioni", action = "ModificaDisposizioni" }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-modificaMotivazione",
                url: "gestioneDisposizioni/ModificaMotivazione",
                defaults: new { controller = "GestioneDisposizioni", action = "ModificaMotivazione" }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-bloccaDisposizione",
                url: "gestioneDisposizioni/BloccaDisposizione",
                defaults: new { controller = "GestioneDisposizioni", action = "BloccaDisposizione" }
            );

            routes.MapRoute(
                name: "gestioneDisposizioni-storicoModifica",
                url: "gestioneDisposizioni/GetStoricoModifica",
                defaults: new { controller = "GestioneDisposizioni", action = "GetStoricoModifica" }
            );
            #endregion

            #region GESTIONE RIMBORSI

            routes.MapRoute(
              name: "rimborsi",
              url: "rimborsi",
              defaults: new { controller = "Rimborsi", action = "Index" }
          );

            routes.MapRoute(
                name: "guide",
                url: "guide",
                defaults: new { controller = "Rimborsi", action = "Guide" }
            );

            routes.MapRoute(
                name: "guideGfs",
                url: "guideGfs",
                defaults: new { controller = "FuoriStandard", action = "GuidaGFS" }
            );

            routes.MapRoute(
              name: "rimborsi-cercaRimborsi",
              url: "rimborsi-cercaRimborsi/{codCliente}/{utente}",
              defaults: new { controller = "Rimborsi", action = "CercaRimborsi" }
          );

            routes.MapRoute(
              name: "rimborsi-cercaRimborsiConfermati",
              url: "rimborsi-cercaRimborsiConfermati/{codCliente}/{Utente}",
              defaults: new { controller = "Rimborsi", action = "CercaRimborsiConfermati" }
          );

            routes.MapRoute(
              name: "rimborsi-cercaCliente",
              url: "rimborsi-cercaCliente/{term}",
              defaults: new { controller = "Rimborsi", action = "CercaCliente" }
          );

            routes.MapRoute(
              name: "rimborsi-cercaClienti",
              url: "rimborsi-cercaClienti/{term}",
              defaults: new { controller = "Rimborsi", action = "CercaClienti" }
          );

            routes.MapRoute(
              name: "rimborsi-dettaglioRimborso",
              url: "Rimborsi/DettaglioRimborso/{AnnoDocumento}/{NumeroDocumento}/{CodCliente}",
              defaults: new { controller = "Rimborsi", action = "DettaglioRimborso" }
          );

            routes.MapRoute(
              name: "rimborsi-impostaBeneficiario",
              url: "Rimborsi/ImpostaBeneficiario",
              defaults: new { controller = "Rimborsi", action = "ImpostaBeneficiario" }
          );

            routes.MapRoute(
              name: "rimborsi-intestazioneLettera",
              url: "Rimborsi/IntestazioneLettera",
              defaults: new { controller = "Rimborsi", action = "IntestazioneLettera" }
          );

            routes.MapRoute(
              name: "rimborsi-recapitoAssegno",
              url: "Rimborsi/RecapitoAssegno",
              defaults: new { controller = "Rimborsi", action = "RecapitoAssegno" }
          );

            routes.MapRoute(
              name: "rimborsi-confermaRimborsi",
              url: "Rimborsi/ConfermaRimborsi",
              defaults: new { controller = "Rimborsi", action = "ConfermaRimborsi" }
          );

            routes.MapRoute(
              name: "rimborsi-annullaRimborsi",
              url: "Rimborsi/AnnullaRimborsi",
              defaults: new { controller = "Rimborsi", action = "AnnullaRimborsi" }
          );

            routes.MapRoute(
              name: "rimborsi-ristampaRimborsi",
              url: "Rimborsi/RistampaRimborsi",
              defaults: new { controller = "Rimborsi", action = "RistampaRimborsi" }
          );

            routes.MapRoute(
              name: "rimborsi-utenteRimborso",
              url: "Rimborsi/UtenteRimborso",
              defaults: new { controller = "Rimborsi", action = "UtenteRimborso" }
          );

            routes.MapRoute(
              name: "rimborsi-utenteRimborsoAnn",
              url: "Rimborsi/UtenteRimborsoAnn",
              defaults: new { controller = "Rimborsi", action = "UtenteRimborsoAnn" }
          );

            routes.MapRoute(
              name: "rimborsi-cancellaRimborsi",
              url: "Rimborsi/CancellaRimborsi",
              defaults: new { controller = "Rimborsi", action = "CancellaRimborsi" }
          );

            routes.MapRoute(
              name: "rimborsi-confermareRimborsi",
              url: "Rimborsi/ConfermareRimborsi",
              defaults: new { controller = "Rimborsi", action = "ConfermareRimborsi" }
          );

            routes.MapRoute(
              name: "rimborsi-rimborsiConfermati",
              url: "Rimborsi/RimborsiConfermati",
              defaults: new { controller = "Rimborsi", action = "RimborsiConfermati" }
           );

            routes.MapRoute(
              name: "rimborsi-ristampaRimborsiSelezionati",
              url: "Rimborsi/RistampaRimborsiSelezionati",
              defaults: new { controller = "Rimborsi", action = "RistampaRimborsiSelezionati" }
          );

            routes.MapRoute(
              name: "rimborsi-salvaRimborsi",
              url: "Rimborsi/SalvaRimborsi",
              defaults: new { controller = "Rimborsi", action = "SalvaRimborsi" }
          );

            routes.MapRoute(
              name: "rimborsi-RistampaMassiva",
              url: "Rimborsi/RistampaMassiva",
              defaults: new { controller = "Rimborsi", action = "RistampaMassiva" }
          );

            routes.MapRoute(
              name: "rimborsi-aggiungiFile",
              url: "Rimborsi/AggiungiFile",
              defaults: new { controller = "Rimborsi", action = "AggiungiFile" }
          );

            routes.MapRoute(
              name: "rimborsi-elencoFileRimborso",
              url: "Rimborsi/ElencoFileRimborso",
              defaults: new { controller = "Rimborsi", action = "ElencoFileRimborso" }
          );

            routes.MapRoute(
              name: "rimborsi-deleteFile",
              url: "Rimborsi/DeleteFile",
              defaults: new { controller = "Rimborsi", action = "DeleteFile" }
          );

            routes.MapRoute(
              name: "rimborsi-registraIBAN",
              url: "Rimborsi/CambiamentoIBAN",
              defaults: new { controller = "Rimborsi", action = "CambiamentoIBAN" }
          );
            #endregion

            #region GESTIONE FUORI STANDARD
            routes.MapRoute(
              name: "fuoriStandard",
              url: "FuoriStandard",
              defaults: new { controller = "FuoriStandard", action = "Index" }
          );

            routes.MapRoute(
              name: "rettifiche",
              url: "Rettifiche",
              defaults: new { controller = "FuoriStandard", action = "Rettifiche" }
          );

            routes.MapRoute(
              name: "reportPrestazioni",
              url: "ReportPrestazioni",
              defaults: new { controller = "FuoriStandard", action = "ReportPrestazioni" }
          );

            routes.MapRoute(
              name: "fuoriStandard-cercaIndennizziAperti",
              url: "FuoriStandard/CercaFuoriStandardAperti",
              defaults: new { controller = "FuoriStandard", action = "CercaFuoriStandardAperti" }
          );

            routes.MapRoute(
              name: "fuoriStandard-cercaIndennizziByTipologia",
              url: "FuoriStandard/CercaFuoriStandardByTipologia",
              defaults: new { controller = "FuoriStandard", action = "CercaFuoriStandardByTipologia" }
          );

            routes.MapRoute(
              name: "fuoriStandard-cercaIndennizziPagabili",
              url: "FuoriStandard/CercaIndennizziPagabili",
              defaults: new { controller = "FuoriStandard", action = "CercaIndennizziPagabili" }
          );

            routes.MapRoute(
              name: "fuoriStandard-registraIndennizzo",
              url: "FuoriStandard/RegistraFuoriStandard",
              defaults: new { controller = "FuoriStandard", action = "RegistraFuoriStandard" }
          );

            routes.MapRoute(
              name: "fuoriStandard-approvaUno",
              url: "FuoriStandard/ValidaFuoriStandardFirstStep",
              defaults: new { controller = "FuoriStandard", action = "ValidaFuoriStandardFirstStep" }
          );

            routes.MapRoute(
              name: "fuoriStandard-validaIndennizzoNuovoCliente",
              url: "FuoriStandard/ValidaFuoriStandardNuovoCliente",
              defaults: new { controller = "FuoriStandard", action = "ValidaFuoriStandardNuovoCliente" }
          );

            routes.MapRoute(
              name: "fuoriStandard-rettificaPrimoStep",
              url: "FuoriStandard/RettificaPrimoStep",
              defaults: new { controller = "FuoriStandard", action = "RettificaPrimoStep" }
          );

            routes.MapRoute(
              name: "fuoriStandard-rifiutaIndennizzi",
              url: "FuoriStandard/RifiutaFuoriStandard",
              defaults: new { controller = "FuoriStandard", action = "RifiutaFuoriStandard" }
          );

            routes.MapRoute(
             name: "fuoriStandard-indennizziPagabili",
             url: "FuoriStandard/IndennizziPagabili",
             defaults: new { controller = "FuoriStandard", action = "IndennizziPagabili" }
         );

            routes.MapRoute(
              name: "fuoriStandard-indePagabili",
              url: "FuoriStandard/IndePagabili",
              defaults: new { controller = "FuoriStandard", action = "IndePagabili" }
          );

            routes.MapRoute(
              name: "fuoriStandard-registrazioneIndennizzo",
              url: "FuoriStandard/RegistrazioneFuoriStandard",
              defaults: new { controller = "FuoriStandard", action = "RegistrazioneFuoriStandard" }
          );

            routes.MapRoute(
              name: "fuoriStandard-schedaIndennizzo",
              url: "FuoriStandard/SchedaFuoriStandard",
              defaults: new { controller = "FuoriStandard", action = "SchedaFuoriStandard" }
          );

            routes.MapRoute(
              name: "fuoriStandard-annullaPrestazione",
              url: "FuoriStandard/AnnullaPrestazione",
              defaults: new { controller = "FuoriStandard", action = "AnnullaPrestazione" }
          );

            routes.MapRoute(
              name: "fuoriStandard-schedaErroreUmano",
              url: "FuoriStandard/SchedaErroreUmano",
              defaults: new { controller = "FuoriStandard", action = "SchedaErroreUmano" }
          );

            routes.MapRoute(
              name: "fuoriStandard-schedaCausaCAP",
              url: "FuoriStandard/SchedaCausaCAP",
              defaults: new { controller = "FuoriStandard", action = "SchedaCausaCAP" }
          );

            routes.MapRoute(
              name: "fuoriStandard-schedaIndennizzoStorico",
              url: "FuoriStandard/SchedaFuoriStandardStorico",
              defaults: new { controller = "FuoriStandard", action = "SchedaFuoriStandardStorico" }
          );

            routes.MapRoute(
                name: "fuoriStandard-schedaClienteStorico",
                url: "FuoriStandard/SchedaClienteStorico",
                defaults: new { controller = "FuoriStandard", action = "SchedaClienteStorico" }
            );

            routes.MapRoute(
                name: "filtra-datiCategorieIndennizzi",
                url: "FuoriStandard/FiltraDatiSottoCategoria/{OptionCategoria}/{firstTime}",
                defaults: new { controller = "FuoriStandard", action = "FiltraDatiSottoCategoria" }
            );

            routes.MapRoute(
                name: "filtra-filtraDatiContratti",
                url: "FuoriStandard/FiltraDatiContratti/{CodiceCliente}",
                defaults: new { controller = "FuoriStandard", action = "FiltraDatiContratti" }
            );

            routes.MapRoute(
                name: "filtra-filtraDatiPUF",
                url: "FuoriStandard/FiltraDatiPUF/{CodiceContratto}",
                defaults: new { controller = "FuoriStandard", action = "FiltraDatiPUF" }
            );

            routes.MapRoute(
                name: "fuoriStandard-approvazioneRettifiche",
                url: "FuoriStandard/ApprovazioneRettifiche",
                defaults: new { controller = "FuoriStandard", action = "ApprovazioneRettifiche" }
            );

            routes.MapRoute(
                name: "fuoriStandard-cercaTutteLeRettifiche",
                url: "FuoriStandard/CercaTutteLeRettifiche",
                defaults: new { controller = "FuoriStandard", action = "CercaTutteLeRettifiche" }
            );

            routes.MapRoute(
                name: "fuoriStandard-visualizzaStorico",
                url: "FuoriStandard/VisualizzaStorico",
                defaults: new { controller = "FuoriStandard", action = "VisualizzaStorico" }
            );

            routes.MapRoute(
                name: "fuoriStandard-gestisciRettifiche",
                url: "FuoriStandard/GestisciRettifiche",
                defaults: new { controller = "FuoriStandard", action = "GestisciRettifiche" }
            );

            routes.MapRoute(
               name: "fuoriStandard-recupero",
               url: "FuoriStandard/RecuperoFuoriStandard",
               defaults: new { controller = "FuoriStandard", action = "RecuperoFuoriStandard" }
           );

            routes.MapRoute(
                name: "fuoriStandard-cercaIndennizziStoricoByTipologia",
                url: "FuoriStandard/CercaFuoriStandardStoricoByTipologia",
                defaults: new { controller = "FuoriStandard", action = "CercaFuoriStandardStoricoByTipologia" }
            );

            routes.MapRoute(
                name: "fuoriStandard-cercaStoricoChiusi",
                url: "FuoriStandard/CercaStoricoChiusi",
                defaults: new { controller = "FuoriStandard", action = "CercaStoricoChiusi" }
            );

            routes.MapRoute(
                name: "fuoriStandard-cercaStoricoPendenti",
                url: "FuoriStandard/CercaStoricoPendenti",
                defaults: new { controller = "FuoriStandard", action = "CercaStoricoPendenti" }
            );

            routes.MapRoute(
                name: "fuoriStandard-cercaStoricoTutti",
                url: "FuoriStandard/CercaStoricoTutti",
                defaults: new { controller = "FuoriStandard", action = "CercaStoricoTutti" }
            );

            routes.MapRoute(
                name: "fuoriStandard-IsApproved",
                url: "FuoriStandard/IsApproved/{CodiceCategoria}",
                defaults: new { controller = "FuoriStandard", action = "IsApproved" }
            );

            routes.MapRoute(
                name: "fuoriStandard-ricercaAvanzata",
                url: "FuoriStandard/RicercaAvanzata",
                defaults: new { controller = "FuoriStandard", action = "RicercaAvanzata" }
            );

            routes.MapRoute(
                name: "fuoriStandard-ricercaAvanzataStorico",
                url: "FuoriStandard/RicercaAvanzataStorico",
                defaults: new { controller = "FuoriStandard", action = "RicercaAvanzataStorico" }
            );

            routes.MapRoute(
                name: "fuoriStandard-ricercaAvanzataApprovatore",
                url: "FuoriStandard/RicercaAvanzataApprovatore",
                defaults: new { controller = "FuoriStandard", action = "RicercaAvanzataApprovatore" }
            );

            routes.MapRoute(
                name: "fuoriStandard-downloadFile",
                url: "FuoriStandard/DownloadFile",
                defaults: new { controller = "FuoriStandard", action = "DownloadFile" }
            );

            routes.MapRoute(
                name: "fuoriStandard-downloadFileStorico",
                url: "FuoriStandard/DownloadFileStorico",
                defaults: new { controller = "FuoriStandard", action = "DownloadFileStorico" }
            );

            routes.MapRoute(
                name: "fuoriStandard-cercaFuoriStandardDaApprovare",
                url: "FuoriStandard/CercaFuoriStandardDaApprovare",
                defaults: new { controller = "FuoriStandard", action = "CercaFuoriStandardDaApprovare" }
            );

            routes.MapRoute(
                name: "fuoriStandard-cercaFuoriStandardRifiutati",
                url: "FuoriStandard/CercaFuoriStandardRifiutati",
                defaults: new { controller = "FuoriStandard", action = "CercaFuoriStandardRifiutati" }
            );

            routes.MapRoute(
                name: "fuoriStandard-approvaRettifica",
                url: "FuoriStandard/ApprovaRettifica",
                defaults: new { controller = "FuoriStandard", action = "ApprovaRettifica" }
            );

            routes.MapRoute(
                name: "fuoriStandard-rifiutaRettifica",
                url: "FuoriStandard/RifiutaRettifica",
                defaults: new { controller = "FuoriStandard", action = "RifiutaRettifica" }
            );

            routes.MapRoute(
                name: "fuoriStandard-aggiungiAllegato",
                url: "FuoriStandard/AggiungiAllegato",
                defaults: new { controller = "FuoriStandard", action = "AggiungiAllegato" }
            );

            routes.MapRoute(
                name: "fuoriStandard-elencoAllegatiFS",
                url: "FuoriStandard/ElencoAllegatiFS",
                defaults: new { controller = "FuoriStandard", action = "ElencoAllegatiFS" }
            );

            routes.MapRoute(
                name: "fuoriStandard-eliminaAllegato",
                url: "FuoriStandard/EliminaAllegato",
                defaults: new { controller = "FuoriStandard", action = "EliminaAllegato" }
            );

            routes.MapRoute(
            name: "fuoriStandard-cercaFsStorico",
            url: "FuoriStandard/CercaFsStorico",
            defaults: new { controller = "FuoriStandard", action = "CercaFsStorico" }
        );

            routes.MapRoute(
                name: "fuoriStandard-cercaFSNonIndennizzabili",
                url: "FuoriStandard/CercaFSNonIndennizzabili",
                defaults: new { controller = "FuoriStandard", action = "CercaFSNonIndennizzabili" }
            );

            routes.MapRoute(
                name: "fuoriStandard-exportPrestazioni",
                url: "FuoriStandard/ExportPrestazioni",
                defaults: new { controller = "FuoriStandard", action = "ExportPrestazioni" }
            );

            routes.MapRoute(
                name: "fuoriStandard-exportReportPrestazioni",
                url: "FuoriStandard/ExportReportPrestazioni",
                defaults: new { controller = "FuoriStandard", action = "ExportReportPrestazioni" }
            );

            routes.MapRoute(
                name: "fuoriStandard-schedaAssociaCliente",
                url: "FuoriStandard/SchedaAssociaCliente",
                defaults: new { controller = "FuoriStandard", action = "SchedaAssociaCliente" }
            );

            routes.MapRoute(
                name: "fuoriStandard-associaNuovoCliente",
                url: "FuoriStandard/AssociaNuovoCliente",
                defaults: new { controller = "FuoriStandard", action = "AssociaNuovoCliente" }
            );

            routes.MapRoute(
                name: "fuoriStandard-cercaFsRettifiche",
                url: "FuoriStandard/CercaFsRettifiche",
                defaults: new { controller = "FuoriStandard", action = "CercaFsRettifiche" }
            );

            routes.MapRoute(
                name: "fuoriStandard-filtraDatiIndicatori",
                url: "FuoriStandard/FiltraDatiIndicatori/{view}/{OptionGruppo}/{firstTime}",
                defaults: new { controller = "FuoriStandard", action = "FiltraDatiIndicatori" }
            );

            routes.MapRoute(
                name: "fuoriStandard-anteprimaPrestazioni",
                url: "FuoriStandard/AnteprimaPrestazioni",
                defaults: new { controller = "FuoriStandard", action = "AnteprimaPrestazioni" }
            );

            routes.MapRoute(
                name: "fuoriStandard-dropDownFilters",
                url: "FuoriStandard/DropDownFilters",
                defaults: new { controller = "FuoriStandard", action = "DropDownFilters" }
            );

            routes.MapRoute(
                name: "fuoriStandard-filtraDatiDropdown",
                url: "FuoriStandard/FiltraDatiDropdown",
                defaults: new { controller = "FuoriStandard", action = "FiltraDatiDropdown" }
            );
            routes.MapRoute(
                name: "fuoriStandard-validaSospensioni",
                url: "FuoriStandard/ValidaSospensioni",
                defaults: new { controller = "FuoriStandard", action = "ValidaSospensioni" }
            );
            routes.MapRoute(
                name: "fuoriStandard-elencoSospensioni",
                url: "FuoriStandard/ElencoSospensioni",
                defaults: new { controller = "FuoriStandard", action = "ElencoSospensioni" }
            );
            routes.MapRoute(
                name: "fuoriStandard-aggiornaSospensione",
                url: "FuoriStandard/AggiornaSospensione",
                defaults: new { controller = "FuoriStandard", action = "AggiornaSospensione" }
            );
            routes.MapRoute(
                name: "fuoriStandard-categorieSospensioni",
                url: "FuoriStandard/CategorieSospensioni",
                defaults: new { controller = "FuoriStandard", action = "CategorieSospensioni" }
            );
            routes.MapRoute(
                name: "fuoriStandard-tipiSospensione",
                url: "FuoriStandard/TipiSospensione",
                defaults: new { controller = "FuoriStandard", action = "TipiSospensione" }
            );
            routes.MapRoute(
                name: "fuoriStandard-ripristinaSospensioniCancellate",
                url: "FuoriStandard/RipristinaSospensioniCancellate",
                defaults: new { controller = "FuoriStandard", action = "RipristinaSospensioniCancellate" }
            );
            routes.MapRoute(
                name: "fuoriStandard-annullaSospensione",
                url: "FuoriStandard/AnnullaSospensione",
                defaults: new { controller = "FuoriStandard", action = "AnnullaSospensione" }
            );
            routes.MapRoute(
                name: "fuoriStandard-getSospensione",
                url: "FuoriStandard/GetSospensione",
                defaults: new { controller = "FuoriStandard", action = "GetSospensione" }
            );
            routes.MapRoute(
                name: "fuoriStandard-downloadFileSospensione",
                url: "FuoriStandard/DownloadFileSospensione",
                defaults: new { controller = "FuoriStandard", action = "DownloadFileSospensione" }
            );
            routes.MapRoute(
                name: "fuoriStandard-eliminaAllegatoSospensione",
                url: "FuoriStandard/EliminaAllegatoSospensione",
                defaults: new { controller = "FuoriStandard", action = "EliminaAllegatoSospensione" }
            );
            routes.MapRoute(
               name: "fuoriStandard-aggiungiAllegatoSospensione",
               url: "FuoriStandard/AggiungiAllegatoSospensione",
               defaults: new { controller = "FuoriStandard", action = "AggiungiAllegatoSospensione" }
           );
            routes.MapRoute(
               name: "fuoriStandard-elencoAllegatiSospensione",
               url: "FuoriStandard/ElencoAllegatiSospensione",
               defaults: new { controller = "FuoriStandard", action = "ElencoAllegatiSospensione" }
           );
            routes.MapRoute(
               name: "fuoriStandard-sospendiModifiche",
               url: "FuoriStandard/SospendiModifiche",
               defaults: new { controller = "FuoriStandard", action = "SospendiModifiche" }
           );
            routes.MapRoute(
               name: "fuoriStandard-getNuovaSospensione",
               url: "FuoriStandard/GetNuovaSospensione",
               defaults: new { controller = "FuoriStandard", action = "GetNuovaSospensione" }
           );
            routes.MapRoute(
           name: "fuoriStandard-getAnniBloccati",
           url: "FuoriStandard/GetAnniBloccati",
           defaults: new { controller = "FuoriStandard", action = "GetAnniBloccati" }
       );
            routes.MapRoute(
               name: "fuoriStandard-modificaDataBlocco",
               url: "FuoriStandard/ModificaDataBlocco",
               defaults: new { controller = "FuoriStandard", action = "ModificaDataBlocco" }
           );
            routes.MapRoute(
               name: "fuoriStandard-aggiornaDataBlocco",
               url: "FuoriStandard/AggiornaDataBlocco",
               defaults: new { controller = "FuoriStandard", action = "AggiornaDataBlocco" }
           );
            routes.MapRoute(
               name: "fuoriStandard-nuovaDataBlocco",
               url: "FuoriStandard/NuovaDataBlocco",
               defaults: new { controller = "FuoriStandard", action = "NuovaDataBlocco" }
           );

            #endregion
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
