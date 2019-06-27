using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestioneRimborsi.Web.Support
{
    public class XmlImport
    {

    //    Private Function FromXML_ToDataTable(Pathfile As String) As DataTable

    //    Dim dt As DataTable = getEmptyDataTable()
    //    Dim ds As DataSet = Helpers.getDataSetFromXml(Pathfile)

    //    Try

    //        Dim dtRichiesta As DataTable = ds.Tables("richiesta")
    //        If Not IsNothing(dtRichiesta) AndAlso dtRichiesta.Rows.Count >= 0 Then
    //            Try
    //                For i = 0 To dtRichiesta.Rows.Count - 1
    //                    Dim RigaTab As DataRow = dt.NewRow

    //                    Dim dtRichiedente As DataTable = ds.Tables("richiedente").Select("richiesta_Id=" & dtRichiesta.Rows(i)("richiesta_Id")).CopyToDataTable
    //                    Dim dtFornitura As DataTable = ds.Tables("fornitura").Select("richiesta_Id=" & dtRichiesta.Rows(i)("richiesta_Id")).CopyToDataTable

    //                    RigaTab("protocolloDomanda") = dtRichiesta.Rows(i)("protocolloDomanda")
    //                    RigaTab("protocolloRichiesta") = dtRichiesta.Rows(i)("protocolloRichiesta")
    //                    RigaTab("tipologiaDomanda") = dtRichiesta.Rows(i)("tipologiaDomanda")
    //                    RigaTab("richiedenteNome") = dtRichiedente.Rows(0)("nome")
    //                    RigaTab("richiedenteCognome") = dtRichiedente.Rows(0)("cognome")
    //                    RigaTab("richiedenteCodiceFiscale") = dtRichiedente.Rows(0)("codiceFiscale")

    //                    If ds.Tables.Contains("documentoRiconoscimento") Then
    //                        If ds.Tables("documentoRiconoscimento").Select("richiedente_Id=" & dtRichiedente.Rows(0)("richiedente_Id")).Count > 0 Then
    //                            Dim dtDocumentoRiconoscimento As DataTable = ds.Tables("documentoRiconoscimento").Select("richiedente_Id=" & dtRichiedente.Rows(0)("richiedente_Id")).CopyToDataTable
    //                            RigaTab("richiedenteTipoDocumento") = dtDocumentoRiconoscimento.Rows(0)("tipo")
    //                            RigaTab("richiedenteNumeroDocumento") = dtDocumentoRiconoscimento.Rows(0)("numero")
    //                            RigaTab("richiedenteDataDocumento") = dtDocumentoRiconoscimento.Rows(0)("data")
    //                            RigaTab("richiedenteEnteRilascioDocumento") = dtDocumentoRiconoscimento.Rows(0)("enteRilascio")
    //                            dtDocumentoRiconoscimento.Dispose()
    //                        End If
    //                    End If

    //                    If ds.Tables.Contains("residenza") Then
    //                        If ds.Tables("residenza").Select("richiesta_Id=" & dtRichiesta.Rows(i)("richiesta_Id")).Count > 0 Then
    //                            Dim dtResidenza As DataTable = ds.Tables("residenza").Select("richiesta_Id=" & dtRichiesta.Rows(i)("richiesta_Id")).CopyToDataTable
    //                            RigaTab("richiedenteIstatComune") = dtResidenza.Rows(0)("comune")
    //                            RigaTab("richiedenteAreaCircolazione") = dtResidenza.Rows(0)("areaDiCircolazione")
    //                            RigaTab("richiedenteCivico") = dtResidenza.Rows(0)("numeroCivico")
    //                            RigaTab("richiedenteEdificio") = dtResidenza.Rows(0)("edificio") & ""
    //                            RigaTab("richiedenteScala") = dtResidenza.Rows(0)("scala") & ""
    //                            RigaTab("richiedenteInterno") = dtResidenza.Rows(0)("interno") & ""
    //                            RigaTab("richiedenteCAP") = dtResidenza.Rows(0)("cap")
    //                            dtResidenza.Dispose()
    //                        End If
    //                    End If

    //                    If ds.Tables.Contains("fornituraIndividuale") Then
    //                        If ds.Tables("fornituraIndividuale").Select("fornitura_Id=" & dtFornitura.Rows(0)("richiesta_Id")).Count > 0 Then
    //                            Dim dtFornituraIndividuale As DataTable = ds.Tables("fornituraIndividuale").Select("fornitura_Id=" & dtFornitura.Rows(0)("richiesta_Id")).CopyToDataTable
    //                            Dim dtIntestatario As DataTable = ds.Tables("intestatario").Select("fornituraIndividuale_Id=" & dtFornituraIndividuale.Rows(0)("fornituraIndividuale_Id")).CopyToDataTable
    //                            Dim dtIndirizzoInt As DataTable = ds.Tables("indirizzo").Select("fornituraIndividuale_Id=" & dtFornituraIndividuale.Rows(0)("fornituraIndividuale_Id")).CopyToDataTable

    //                            RigaTab("codiceUtenteIndividuale") = dtFornituraIndividuale.Rows(0)("codiceUtente")
    //                            RigaTab("individualeNome") = dtIntestatario.Rows(0)("nome")
    //                            RigaTab("individualeCognome") = dtIntestatario.Rows(0)("cognome")
    //                            RigaTab("individualecodiceFiscale") = dtIntestatario.Rows(0)("codiceFiscale")

    //                            If ds.Tables.Contains("documentoRiconoscimento") Then
    //                                If ds.Tables("documentoRiconoscimento").Select("intestatario_Id=" & dtIntestatario.Rows(0)("intestatario_Id")).Count > 0 Then
    //                                    Dim dtDocumentoRiconoscimentoInt As DataTable = ds.Tables("documentoRiconoscimento").Select("intestatario_Id=" & dtIntestatario.Rows(0)("intestatario_Id")).CopyToDataTable
    //                                    RigaTab("individualeTipoDocumento") = dtDocumentoRiconoscimentoInt.Rows(0)("tipo")
    //                                    RigaTab("individualeNumeroDocumento") = dtDocumentoRiconoscimentoInt.Rows(0)("numero")
    //                                    RigaTab("individualeDataDocumento") = dtDocumentoRiconoscimentoInt.Rows(0)("data")
    //                                    RigaTab("individualeEnteRilascioDocumento") = dtDocumentoRiconoscimentoInt.Rows(0)("enteRilascio")
    //                                    dtDocumentoRiconoscimentoInt.Dispose()
    //                                End If
    //                            End If
    //                            dtIntestatario.Dispose()

    //                            RigaTab("individualeistatComune") = dtIndirizzoInt.Rows(0)("comune")
    //                            RigaTab("individualeAreaCircolazione") = dtIndirizzoInt.Rows(0)("areaDiCircolazione")
    //                            RigaTab("individualeCivico") = dtIndirizzoInt.Rows(0)("numeroCivico")
    //                            RigaTab("individualeEdificio") = dtIndirizzoInt.Rows(0)("edificio") & ""
    //                            RigaTab("individualeScala") = dtIndirizzoInt.Rows(0)("scala") & ""
    //                            RigaTab("individualeInterno") = dtIndirizzoInt.Rows(0)("interno") & ""
    //                            RigaTab("individualeCAP") = dtIndirizzoInt.Rows(0)("cap")
    //                            dtIndirizzoInt.Dispose()
    //                        End If
    //                    End If

    //                    If ds.Tables.Contains("fornituraCentralizzata") Then
    //                        If ds.Tables("fornituraCentralizzata").Select("fornitura_Id=" & dtFornitura.Rows(0)("richiesta_Id")).Count > 0 Then
    //                            Dim dtFornituraCentralizzata As DataTable = ds.Tables("fornituraCentralizzata").Select("fornitura_Id=" & dtFornitura.Rows(0)("richiesta_Id")).CopyToDataTable
    //                            Dim dtIndirizzoCen As DataTable = ds.Tables("indirizziCentralizzata").Select("fornituraCentralizzata_Id=" & dtFornituraCentralizzata.Rows(0)("fornituraCentralizzata_Id")).CopyToDataTable

    //                            RigaTab("codiceUtenteCentralizzato") = dtFornituraCentralizzata.Rows(0)("codiceUtente") & ""
    //                            RigaTab("centralizzatoIBAN") = dtFornituraCentralizzata.Rows(0)("codiceIBAN") & ""
    //                            RigaTab("centralizzatoDenominazioneCondominio") = dtFornituraCentralizzata.Rows(0)("denominazioneCondominio") & ""
    //                            RigaTab("centralizzatoEdificioPlurifamiliare") = dtFornituraCentralizzata.Rows(0)("edificioPlurifamiliare") & ""

    //                            RigaTab("centralizzatoistatComune") = dtIndirizzoCen.Rows(0)("comune")
    //                            RigaTab("centralizzatoAreaCircolazione") = dtIndirizzoCen.Rows(0)("areaDiCircolazione")
    //                            RigaTab("centralizzatoCivico") = dtIndirizzoCen.Rows(0)("numeroCivico")
    //                            RigaTab("centralizzatoEdificio") = dtIndirizzoCen.Rows(0)("edificio") & ""
    //                            RigaTab("centralizzatoScala") = dtIndirizzoCen.Rows(0)("scala") & ""
    //                            RigaTab("centralizzatoInterno") = dtIndirizzoCen.Rows(0)("interno") & ""
    //                            RigaTab("centralizzatoCAP") = dtIndirizzoCen.Rows(0)("cap")

    //                            If dtIndirizzoCen.Rows.Count > 1 Then
    //                                RigaTab("centralizzatoistatComune2") = dtIndirizzoCen.Rows(1)("comune")
    //                                RigaTab("centralizzatoAreaCircolazione2") = dtIndirizzoCen.Rows(1)("areaDiCircolazione")
    //                                RigaTab("centralizzatoCivico2") = dtIndirizzoCen.Rows(1)("numeroCivico")
    //                                RigaTab("centralizzatoEdificio2") = dtIndirizzoCen.Rows(1)("edificio") & ""
    //                                RigaTab("centralizzatoScala2") = dtIndirizzoCen.Rows(1)("scala") & ""
    //                                RigaTab("centralizzatoInterno2") = dtIndirizzoCen.Rows(1)("interno") & ""
    //                                RigaTab("centralizzatoCAP2") = dtIndirizzoCen.Rows(1)("cap")
    //                            End If

    //                            If dtIndirizzoCen.Rows.Count > 2 Then
    //                                RigaTab("centralizzatoistatComune3") = dtIndirizzoCen.Rows(2)("comune")
    //                                RigaTab("centralizzatoAreaCircolazione3") = dtIndirizzoCen.Rows(2)("areaDiCircolazione")
    //                                RigaTab("centralizzatoCivico3") = dtIndirizzoCen.Rows(2)("numeroCivico")
    //                                RigaTab("centralizzatoEdificio3") = dtIndirizzoCen.Rows(2)("edificio") & ""
    //                                RigaTab("centralizzatoScala3") = dtIndirizzoCen.Rows(2)("scala") & ""
    //                                RigaTab("centralizzatoInterno3") = dtIndirizzoCen.Rows(2)("interno") & ""
    //                                RigaTab("centralizzatoCAP3") = dtIndirizzoCen.Rows(2)("cap")
    //                            End If

    //                            dtIndirizzoCen.Dispose()
    //                        End If
    //                    End If
    //                    dtFornitura.Dispose()

    //                    Dim dtBonus As DataTable = ds.Tables("bonus").Select("richiesta_Id=" & dtRichiesta.Rows(i)("richiesta_Id")).CopyToDataTable
    //                    RigaTab("componentiFamigliaAnagrafica") = dtBonus.Rows(0)("componentiFamigliaAnagrafica")
    //                    RigaTab("dataDisponibilita") = dtBonus.Rows(0)("dataDisponibilita")
    //                    RigaTab("dataAmmissione") = dtBonus.Rows(0)("dataAmmissione")
    //                    RigaTab("dataPresentazione") = dtBonus.Rows(0)("dataPresentazione")
    //                    RigaTab("dataInizioAgevolazione") = dtBonus.Rows(0)("dataInizioAgevolazione")
    //                    RigaTab("dataFineAgevolazione") = dtBonus.Rows(0)("dataFineAgevolazione")
    //                    RigaTab("allineamento") = dtBonus.Rows(0)("allineamento") & ""
    //                    RigaTab("fruizioneContinuativa") = dtBonus.Rows(0)("fruizioneContinuativa") & ""
    //                    dtBonus.Dispose()
    //                    dtRichiedente.Dispose()

    //                    dt.Rows.Add(RigaTab)
    //                Next

    //            Catch ex As Exception
    //                Helpers.WriteTrace(slogFile, "Problemi nella lettura dei dati")
    //                Return Nothing
    //            End Try
    //        End If

    //    Catch ex As Exception
    //        Helpers.WriteTrace(slogFile, " -- Errore: " & ex.ToString)
    //        dt = Nothing
    //        FATAL_ERROR = True
    //    End Try

    //    Return dt

    //End Function


    }
}