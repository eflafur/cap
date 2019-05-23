using GruppoCap.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GruppoCap;

namespace GestioneRimborsi.Web
{
    public static class UrlFor
    {

        #region Lotto Rimborsi
        public static String LottoRimborsiList
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("lottorimborsi-list").ToAbsoluteUrl(); }
        }
        public static String LottoRimborsi_InnerList(string UserName)
        {
            return CommonUrls.BaseUrl.AppendUrlTokens("lottorimborsi-inner-list", UserName).ToAbsoluteUrl().EnsureEndsWith("/");
        }
        public static String LottoRimborsi_InnerList()
        {
            return CommonUrls.BaseUrl.AppendUrlTokens("lottorimborsi-inner-list").ToAbsoluteUrl().EnsureEndsWith("/");
        }
        public static String GeneraFileRimborsi
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("lottorimborsi-genera-file").ToAbsoluteUrl().EnsureEndsWith("/"); }
        }
        public static String DownloadFileRimborsi
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("lottorimborsi-download-file").ToAbsoluteUrl().EnsureEndsWith("?") + "id=_id_"; }
        }        
        
        #endregion

        #region Gestione Disposizioni
        public static String GestioneDisposizioniList
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("gestioneDisposizioni-list").ToAbsoluteUrl(); }
        }
        public static String DownloadDisposizioni
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("gestioneDisposizioni-download-file").ToAbsoluteUrl().EnsureEndsWith("?") + "id=_id_"; }
        }

        public static String DownloadCsvDisposizioni
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("gestioneDisposizioni-download-csv").ToAbsoluteUrl().EnsureEndsWith("?") + "id=_id_"; }
        }

        public static String GestioneDisposizioni_SearchSepaHeader()
        {
            return CommonUrls.BaseUrl.AppendUrlTokens("gestioneDisposizioni-searchSepaHeader").ToAbsoluteUrl().EnsureEndsWith("/");
        }

        public static String GestioneDisposizioni_InnerSepaList()
        {
            return CommonUrls.BaseUrl.AppendUrlTokens("gestioneDisposizioni-inner-list").ToAbsoluteUrl().EnsureEndsWith("/");
        }

        public static String GestioneDisposizioni_InnerSepaList(string UserName)
        {
            return CommonUrls.BaseUrl.AppendUrlTokens("gestioneDisposizioni-inner-list", UserName).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        #endregion

        #region GESTIONE RIMBORSI
        public static String HomeRimborsi
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("rimborsi").ToAbsoluteUrl(); }
        }
        public static String Guide
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("guide").ToAbsoluteUrl(); }
        }
        public static String GuideGFS
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("guideGfs").ToAbsoluteUrl(); }
        }
        public static String RimborsiSearch(String codCliente, String utente)
        {
            var urlTokens = new string[] { "rimborsi-cercaRimborsi", codCliente, utente };
            return CommonUrls.BaseUrl.AppendUrlTokens(urlTokens).ToAbsoluteUrl().EnsureEndsWith("/");
            //return CommonUrls.BaseUrl.AppendUrlTokens("rimborsi-cercaRimborsi", codCliente).ToAbsoluteUrl().EnsureEndsWith("/");
        }
        public static String CercaRimborsiConfermati(String codCliente, String Utente)
        {
            var urlTokens = new string[] { "rimborsi-cercaRimborsiConfermati", codCliente, Utente };
            return CommonUrls.BaseUrl.AppendUrlTokens(urlTokens).ToAbsoluteUrl().EnsureEndsWith("/");
        }
        public static String ClienteSearch(String term)
        {
            return CommonUrls.BaseUrl.AppendUrlTokens("rimborsi-cercaCliente", term).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        public static String ClientiSearch(String term)
        {
            return CommonUrls.BaseUrl.AppendUrlTokens("rimborsi-cercaClienti", term).ToAbsoluteUrl().EnsureEndsWith("/");
        }
        #endregion

        #region GESTIONE FUORI STANDARD
        public static String HomeFuoriStandard
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("FuoriStandard").ToAbsoluteUrl(); }
        }
        #endregion

        #region BONUS IDRICO
        public static String BonusIdrico
        {
            get { return CommonUrls.BaseUrl.AppendUrlTokens("BonusIdrico").ToAbsoluteUrl(); }
        }
        #endregion
    }
}