
var status = 0;
var step = 0;
var lt = null;
var row = null;
var detail = [];
var lotselected = null;

var ana = {
    customerId: "",
    esito: "",
    integra: "",
    id: "",
    utenze: "",
    datapresentazione: "",
    ncf: "",
    esitosgate: "",
    sel: 0
};
var flag = {
    Ok: "OK"
}

function LotDetailsSearchCriterias(RequestStatus, CustomerType, Outcome, RequestId) {
    this.RequestStatus = RequestStatus;
    this.CustomerType = CustomerType;
    this.Outcome = Outcome;
    this.RequestId = RequestId;
};

var pageUtils = {
    selectedLotRow: null,
    formState: "start",
    cleanLotTable: function () {
        $("#tblLotList tbody").empty();
    },

    getLotStatusIcon: function (status) {
        var statuses = {
            "1": "fa-check-circle",
            "2": "fa-check-chevron-right",
            "3": "fa-thumbs-down text-danger",
            "5": "fa-thumbs-up text-success",
            "6": "fa-lock text-success"
        };
        return statuses[status];
    },

    getRequestStatusIcon: function (status) {
        var statuses = {
            "OK": "fa-smile-o text-success",
            "OK*": "fa-smile-o text-success",
            "OK**": "fa-smile-o text-success",
            "KO1": "fa-frown-o text-danger",
            "KO2": "fa-frown-o text-danger",
            "KO3": "fa-meh-o text-warning",
            "KO4": "fa-meh-o text-warning",
            "KO5": "fa-meh-o text-warning",
            "KO6": "fa-meh-o text-warning",
            "KO7": "fa-meh-o text-warning"
        };
        if (status != null) {
            if (status.search("OK") >= 0)
                return statuses["OK"];
            var statusSplitted = status.split("-");
            return statuses[statusSplitted[0]];
        }
        return "";
    },

    setInfoLotto: function () {
        $("#infoLottoUpload").empty();
        var today = moment(new Date($.now()).toString()).format('L');
        $("#tmpl-infoLottoUpload").tmpl(today).appendTo("#infoLottoUpload");

        $("#mdUploadXml").modal();
    },

    setEndSgateReq: function (loadDate) {
        var endDate = new Date(moment(loadDate, "DD/MM/YYYY"));
        var sgateEndDate = new Date(endDate.getFullYear(), endDate.getMonth() + 2, 1);
        var finalDate = moment(sgateEndDate).format('L');
        $("#lottodateend").text(finalDate);
    },

    getStatusdescription: function (status) {

    },

    getOutcomeGeneralStatus: function (EsitoAutoVal, EsitoManVal) {
        return "checked";
    },

    getOutcomeStatus: function (sourceOutcome, EsitoAutoVal, EsitoManVal) {
        var realOutcome = EsitoAutoVal;
        if (realOutcome == null) { realOutcome = ""; }
        if (EsitoManVal != undefined)
            realOutcome = EsitoManVal;

        var splittedResult = realOutcome.split("-");
        var checked = ""
        if (sourceOutcome == "Positivo") {
            if (realOutcome.includes("OK"))
                return "checked";
        }
        if (sourceOutcome == "Negativo") {
            if (!realOutcome.includes("OK"))
                return "checked";
        }
        if (sourceOutcome == "Multi") {
            if (!realOutcome.includes("KO1") && !realOutcome.includes("KO2") && !realOutcome.includes("OK"))
                return "checked";
        }
        $.each(splittedResult, function (index, item) {
            if (splittedResult.includes(sourceOutcome))
                checked = "checked='checked'";
            return false;
        });
        return checked;
    },

    getProcessedIcon: function (Processato) {
        if (Processato == "1") {
            return "fa-check-square-o text-success"
        }
        return "fa-square-o text-muted"
    },

    getFileFromServer: function () {
        if (pageUtils.selectedLotRow == undefined) {
            alert('Per scaricare un esito o una rettifica è necessario selezionare un lotto dalla lista.');
            return false;
        }
        var lotId = $(pageUtils.selectedLotRow).attr("bi-lotid");
        controller.downloadFileFromServer(lotId, function () {

        });
    },

    selectLotTableRow: function (row) {
        //     row.addClass("selected");
        $("#tblLotList tr").removeClass("selected");
        $("#btnValidateLot").attr("disabled", "disabled");
        $("#btnConfirmLot").attr("disabled", "disabled");
        $("#btnDownloadExcel").attr("disabled", "disabled");
        $("#btnDownloadEsito").attr("disabled", "disabled");
        pageUtils.selectedLotRow = row

        if (row != null) {
            row.addClass("selected");
            step = row.attr("bi-statusvalue");
            if (step != 5 && step != 6) {
                $("#btnValidateLot").removeAttr("disabled", "disabled");
                $("#btnDownloadEsito").attr("disabled", "disabled");
                $("#btnConfirmLot").attr("disabled", "disabled");
                $("#btnDownloadExcel").attr("disabled", "disabled");
            }
            if (step == 5) {
                $("#btnDownloadEsito").removeAttr("disabled", "disabled");
                $("#btnValidateLot").attr("disabled", "disabled");
                $("#btnConfirmLot").removeAttr("disabled", "disabled");
                $("#btnDownloadExcel").attr("disabled", "disabled");
            }
            if (step == 6) {
                $("#btnDownloadEsito").attr("disabled", "disabled");
                $("#btnValidateLot").attr("disabled", "disabled");
                $("#btnConfirmLot").attr("disabled", "disabled");
                $("#btnDownloadExcel").removeAttr("disabled", "disabled");
            }
        }
    },

    loadLotTable: function () {
        pageUtils.showActivityIndicator("Caricamento lotti in corso...");
        controller.getLotti(function (data) {
            $("#tblLotBody").empty();
            //$("#tmpl-lotTableRow").tmpl(data).appendTo("#tblLotBody");
            $('#tblLotBody').append($("#tmpl-lotTableRow").tmpl(data));
            pageUtils.hideActivityIndicator();
        }, function (code, message, error) {
            pageUtils.hideActivityIndicator();
            pageUtils.showDialog("OPERAZIONE FALLITA : Codice:" + code.status + " - Descrizione:" + code.responseJSON.StatusDescription);
        }
        );

        //function (jqXHR, textStatus, errorThrown) {
        //    pageUtils.hideActivityIndicator();
        //    pageUtils.showDialog(jqXHR.statusText, function () { pageUtils.hideActivityIndicator(); });
        //});
        //   });
    },

    browseLotPage: function (lotId, pageIndex) {
        var lotid = lotselected.trim();
        var RequestStatus = $("#statoselect option:selected").val();
        var CustomerType = $("#tiposelect option:selected").val();
        if ($("#opzioneselect option:selected").val() == 0)
            var Outcome = $("#idselect").val();
        if ($("#opzioneselect option:selected").val() == 1)
            var RequestId = $("#idselect").val();
        var data = new LotDetailsSearchCriterias(RequestStatus, CustomerType, Outcome, RequestId);
        pageUtils.navigateLotDetail(lotId, pageIndex, data);
    },

    navigateLotDetail: function (lotId, pageIndex, data) {
        pageUtils.showActivityIndicator("Caricamento lotto in corso...");
        var searchParams = {
            lotId: lotId,
            pageSize: 10,
            pageIndex: parseInt(pageIndex), // qui bisogna gestire la paginazione, aggiungendo il parametro in ingresso (ricordarsi che il valore da passare è pagina -1!. es: pagina 1 => 0)
            criterias: {
                // Per dettagli sui filtri vedi oggetto "LotDetailsSearchCriterias", tra i model del frontend
                RequestStatus: data.RequestStatus,   // qui gestisci lo stato richiesta. 
                CustomerType: data.CustomerType,    // qui la tipologia utenza
                Outcome: data.Outcome,        // qui l'esito
                RequestId: data.RequestId         // qui l'id richiesta
            }
        }
        pageUtils.doLotNavigation(lotId, searchParams, function () { });
    },

    getSearchCriteria: function () {
        var lotid = lotselected.trim();
        var pageIndex = $("#pageactive.active >a").text() - 1;
        var RequestStatus = $("#statoselect option:selected").val();
        var CustomerType = $("#tiposelect option:selected").val();
        if ($("#opzioneselect option:selected").val() == 0)
            var Outcome = $("#idselect").val();
        if ($("#opzioneselect option:selected").val() == 1)
            var RequestId = $("#idselect").val();
        var data = new LotDetailsSearchCriterias(RequestStatus, CustomerType, Outcome, RequestId);
        pageUtils.navigateLotDetail(lotid, pageIndex, data);
    },

    resetSearchCriterias: function (dontSearch) {
        $("#statoselect").val(2);
        $("#tiposelect").val(0);
        $("#opzioneselect").val(-1);
        $("#idselect").val("");
        if (!dontSearch) {
            pageUtils.getSearchCriteria();
        }
    },

    doLotNavigation(lotId, searchParams, complete) {

        controller.getLotDetailsExtended(searchParams, function (data) {
            /*data è fatto così:
               RecordCount,
               PageSize,
               PageCount,
               PageIndex,
               Results
        */
            $("#tblLotDetailsBody").empty();
            $("#lotDetailPagination").empty();
            $("#tmpl-lotDetail").tmpl(data.Results).appendTo("#tblLotDetailsBody");
            var thePages = [];
            var pagerSize = 5;
            var lBound = data.PageIndex - pagerSize < 0 ? 1 : data.PageIndex - pagerSize;
            var hBound = data.PageIndex + pagerSize > data.PageCount + 1 ? data.PageCount + 1 : (data.PageIndex + pagerSize < pagerSize * 2 ? pagerSize * 2 : data.PageIndex + pagerSize);
            for (var i = lBound; i < hBound; i++) {
                thePages.push({
                    pageIndex: i,
                    isActive: i == data.PageIndex + 1,
                    isDisabled: i == data.PageIndex + 1
                });
            }
            var paginationUtility = {
                isFirstPage: data.PageIndex == 0,
                isLastPage: data.PageIndex == data.PageCount,
                currentPage: data.PageIndex,
                lastPage: data.PageCount,
                lotId: lotId,
                pages: thePages
            };
            $("#tmpl-pagination").tmpl(paginationUtility).appendTo("#lotDetailPagination");
            pageUtils.hideActivityIndicator();
            complete();
        },
         function (code, message, error) {
             pageUtils.hideActivityIndicator();
             pageUtils.showDialog("OPERAZIONE FALLITA : Codice:" + code.status + " - Descrizione:" + code.responseJSON.StatusDescription);
         });
    },

    openLotDetail: function (lotId) {
        pageUtils.showActivityIndicator("Caricamento lotto in corso...");
        lotselected = lotId;
        lt = lotId;
        var searchParams = {
            lotId: lotId,
            pageSize: 10,
            pageIndex: 0, // qui bisogna gestire la paginazione, aggiungendo il parametro in ingresso (ricordarsi che il valore da passare è pagina -1!. es: pagina 1 => 0)
            criterias: {
                // Per dettagli sui filtri vedi oggetto "LotDetailsSearchCriterias", tra i model del frontend
                RequestStatus: 2,   // qui gestisci lo stato richiesta. 
                CustomerType: 0,    // qui la tipologia utenza
                Outcome: '',        // qui l'esito
                RequestId: 0        // qui l'id richiesta
            }
        }
        pageUtils.doLotNavigation(lotId, searchParams, function () {
            $("#mdLotDetails").modal();
            pageUtils.resetSearchCriterias(true);
        });
    },

    loadCustomerCandidates: function () {
        controller.getCustomerCandidates(function (data) {
            $("#tblCustomerUtenzeBody").empty();
            $("#tmpl-CustomerUtenze").tmpl(data).appendTo("#tblCustomerUtenzeBody");
            $("#mdLotDetails").modal();
            ana.utenze = data.slice();
        },
        function (code, message, error) {
            pageUtils.showDialog("OPERAZIONE FALLITA : Codice:" + code.status + " - Descrizione:" + code.responseJSON.StatusDescription);
        })
    },

    openCustomerUtenza: function (utenza) {

        controller.getCustomerUtenza(utenza, function (data) {
            if (data == null) {
                console.log("Utenza inesistente");
                return;
            }
            $("#tblCustomerUtenzeBody").empty();

            $("#tmpl-CustomerUtenze").tmpl(data).appendTo("#tblCustomerUtenzeBody");

            data["esito"] = ana.esito;
            data["datapresentazione"] = moment((new Date(parseInt((ana.datapresentazione).substr(6)))).toString()).format('L');
            $("#CustomerDetail").empty();
            $("#tmpl-CustomerDetail").tmpl(data).appendTo("#CustomerDetail");
            ana.integra = utenza;
            ana.customerId = data.codCliente;
        },
        function (code, message, error) {
            pageUtils.showDialog("OPERAZIONE FALLITA : Codice:" + code.status + " - Descrizione:" + code.responseJSON.StatusDescription);
        })
    },

    openGetCustomerDetail: function (utenza) {
        $("#CustomerDetail").empty();
        $.each(ana.utenze, function (index, data) {
            data["esito"] = ana.esito;
            data["ncf"] = ana.ncf;
            data["esitosgate"] = ana.esitosgate;
            data["datapresentazione"] = moment((new Date(parseInt((ana.datapresentazione).substr(6)))).toString()).format('L');
            if (data.codClienteIntegra == utenza) {
                $("#CustomerDetail").empty();
                $("#tmpl-CustomerDetail").tmpl(data).appendTo("#CustomerDetail");
                ana.integra = utenza;
                return false;
            }
        });
    },

    openRequestDetail: function (customerId, esito, id) {
        $("#codIntegra").val("");

        pageUtils.showActivityIndicator("Caricamento dati richiesta in corso...");
        controller.getRequestDetail(customerId, esito, id, function (data) {
            $("#requestorDetails").empty();
            $("#requestorProtocol").empty();
            $("#tblCustomerUtenzeBody").empty();
            var _status = data.status;

            //if (data.sgate.EsitoD.indexOf("OK")!==-1) {
            //    $("#forcevalidation").addClass("notdisplayed");
            //    $("#codIntegra").addClass("notdisplayed");
            //    $("#esitoRichiesta").addClass("notdisplayed");

            //}
            //else {
            //    $("#forcevalidation").removeClass("notdisplayed");
            //    $("#codIntegra").removeClass("notdisplayed");
            //    $("#esitoRichiesta").removeClass("notdisplayed");
            //}            

            if (data.sgate.CodUtenteInd == null) {
                $("#iconRequestDetail").removeClass("fa fa-user");
                $("#iconRequestDetail").addClass("fa fa-users");
            }
            else {
                $("#iconRequestDetail").removeClass("fa fa-users");
                $("#iconRequestDetail").addClass("fa fa-user");
            }

            if (data.status == 6) {
                $("#forcevalidation").addClass("notdisplayed");
                $("#codIntegra").addClass("notdisplayed");
            }
            else {
                $("#forcevalidation").removeClass("notdisplayed");
                $("#codIntegra").removeClass("notdisplayed");
            }

            $("#tmpl-requestorDetails").tmpl(data.sgate).appendTo("#requestorDetails");
            $("#tmpl-requestorProtocol").tmpl(data).appendTo("#requestorProtocol");

            ana.customerId = customerId;
            ana.esito = esito;
            ana.id = id;
            ana.datapresentazione = data.sgate.DataAcquisizione;
            ana.ncf = data.sgate.CompFamigliaAnag;
            ana.esitosgate = data.sgate.EsitoD;
            ana.integra = data.integra;
            controller.getRequestvalidationDetails(id, function (data) {
                $("#esitoRichiesta").empty();
                $("#CustomerDetail").empty();
                $("#tmpl-esitoRichiesta").tmpl(data).appendTo("#esitoRichiesta");

                $("input[type=radio][name='ok']").attr("disabled", !$("#positiveOutcome").is(':checked'));
                $("input[type=radio][name='ko']").attr("disabled", !$("#negativeOutcome").is(':checked'));

                $("input[type=radio][name='ko']").change(function () {
                    if (!$("#multipleOutcomes").is(':checked'))
                        $("input[type=checkbox][name='ko']").prop("checked", false);
                    $("input[type=checkbox][name='ko']").attr("disabled", !$("#multipleOutcomes").is(':checked'));
                });

                $("input[type='radio'][name='outcome']").change(function () {
                    if (!$("input[type='radio'][value='ok']").is(':checked')) {
                        $("input[type=radio][name='ok']").prop("checked", false);
                    }

                    if (!$("input[type='radio'][value='ko']").is(':checked')) {
                        $("input[type=radio][name='ko']").prop("checked", false);
                        $("input[type=checkbox][name='ko']").prop("checked", false);
                    }

                    $("input[type=radio][name='ok']").attr("disabled", !$("input[type='radio'][value='ok']").is(':checked'));

                    $("input[type=radio][name='ko']").attr("disabled", !$("input[type='radio'][value='ko']").is(':checked'));
                    $("input[type=checkbox][name='ko']").attr("disabled", !$("#multipleOutcomes").is(':checked'));
                });

                if (_status == 6) {
                    $(".pnlEsitiReq input").attr("disabled", true);
                }
                else {
                    $(".pnlEsitiReq input").attr("disabled", false);
                }

                $("#mdRequestDetails").modal();
            },
                 function (code, message, error) {
                     pageUtils.hideActivityIndicator();
                     pageUtils.showDialog("OPERAZIONE FALLITA : Codice:" + code.status + " - Descrizione:" + code.responseJSON.StatusDescription);

                 });
            pageUtils.loadCustomerCandidates();
            pageUtils.hideActivityIndicator();
        },
        function (code, message, error) {
            pageUtils.hideActivityIndicator();
            pageUtils.showDialog("OPERAZIONE FALLITA : Codice:" + code.status + " - Descrizione:" + code.responseJSON.StatusDescription);
        });
    },

    /*********************************************************************/

    doLotValidation: function (lotId) {
        pageUtils.showActivityIndicator("Validazione lotto in corso... attendere prego");
        controller.validateLotto(lotId, function (data) {
            pageUtils.cleanLotTable();
            pageUtils.loadLotTable();
            pageUtils.hideActivityIndicator();
        },
        function (code, message, error) {
            pageUtils.hideActivityIndicator();
            pageUtils.showDialog("OPERAZIONE FALLITA : Codice:" + code.status + " - Descrizione:" + code.responseJSON.StatusDescription);
        })
    },

    doLotConfirm: function (lotId) {
        pageUtils.showActivityIndicator("Conferma lotto in corso... attendere prego");
        controller.confirmLot(lotId, function (data) {
            pageUtils.cleanLotTable();
            pageUtils.loadLotTable();
            pageUtils.hideActivityIndicator();
            window.location.href = 'loadExcelNuoviClienti?lotId=' + lotId;
        });
    },

    doDownloadExcel: function (lotId) {
        window.location.href = 'loadExcelNuoviClienti?lotId=' + lotId;
    },

    doForceValidation: function () {
        var newOutcome = $($("input[type='checkbox']:checked").map(function () { return $(this).val(); })).get().join('-');
        newOutcome = newOutcome + $($("input[name='ok']:checked").map(function () { return $(this).val(); })).get().join('-');

        if ($("#chkKo1").is(":checked"))
            newOutcome = "KO1";
        if ($("#chkKo2").is(":checked"))
            newOutcome = "KO2";

        if ((newOutcome.indexOf("OK") >= 0) && ($("#codIntegra").val() == "" && ana.sel == 0)) {
            ana.sel = 1;
            pageUtils.showDialog("inserire codice integra o selezionare una utenza ");
            return;
        }

        ana.sel = 0;

        if ($("#codIntegra").val() != "")
            ana.integra = $("#codIntegra").val();
        controller.doForceValidation(ana.id, ana.integra, newOutcome, ana.customerId, function (data) {
            ana.sel = data == 0 ? 1 : 0;
            pageUtils.showDialog("Operazione eseguita con successo");
        },
        function (code, message, error) {
            pageUtils.showDialog("OPERAZIONE FALLITA : Codice:" + code.status + " - Descrizione:" + code.responseJSON.StatusDescription);
        })
        pageUtils.cleanLotTable();
        pageUtils.loadLotTable();
        pageUtils.openLotDetail(lt);
    },

    showDialog: function (messageText, callback) {
        $("#dialog #messageText").html(messageText);
        $("#dialog").modal({ backdrop: 'static', keyboard: false, show: true });
        $("#dialog btn").click(callback);
        if (ana.sel != 1) {
            $("#btnRequestDetails").click();
        }
        ana.sel = 0;


    },

    showActivityIndicator: function (messageText) {
        $("#activityIndicator #messageText").html(messageText);
        $("#activityIndicator").modal({ backdrop: 'static', keyboard: false, show: true });

    },

    hideActivityIndicator: function () {
        $("#activityIndicator").modal('hide');
    },

    loadLotProgress: function () {
        $("#lotProgressMenu").empty();
        $("#lotProgressMenu").append('<li><a href="#"><span class="">recupero informazioni...</span></a></li>');
        controller.getLotProgress(function (data) {
            $("#lotProgressMenu").empty();
            if (data.length > 0) {
                $("#tmpl-lotProgress").tmpl(data).appendTo("#lotProgressMenu");
            }
            else {
                $("#lotProgressMenu").append('<li><a href="#"><span class="">Nessun caricamento in corso.</span></a></li>');
            }
        });
    },

    triggerRadioOutcome: function () {
        if (!$("input[type='radio'][value='ok']").is(':checked')) {
            $("input[type=radio][name='ok']").prop("checked", false);
        }

        if (!$("input[type='radio'][value='ko']").is(':checked')) {
            $("input[type=radio][name='ko']").prop("checked", false);
            $("input[type=checkbox][name='ko']").prop("checked", false);
        }

        $("input[type=radio][name='ok']").attr("disabled", !$("input[type='radio'][value='ok']").is(':checked'));

        $("input[type=radio][name='ko']").attr("disabled", !$("input[type='radio'][value='ko']").is(':checked'));
        $("input[type=checkbox][name='ko']").attr("disabled", !$("#multipleOutcomes").is(':checked'));
    },

}

var controller = {

    getLotti: function (callback, failure) {
        //callback => funzione di callback in caso di successo
        // failure => funzione di callback in caso di errore
        // always => funzione eseguita la termine di ogni chiamata, positiva o no
        //$.post(
        //"biacquisizione",
        //function (data) {
        //    callback(data);
        //});
        $.post("biacquisizione")
            .done(function (data) {
                callback(data)
            })
            .fail(function (jqXHR, httpStatusMessage, customErrorMessage) {
                failure(jqXHR, httpStatusMessage, customErrorMessage)
            })
            .always(function (jqXHR, textStatus, errorThrown) {
            });
    },

    getLotDetailsExtended: function (searchParams, callback, failure) {
        $.post("getLotDetails",
            {
                lotId: searchParams.lotId,
                pageSize: searchParams.pageSize,
                pageIndex: searchParams.pageIndex,
                criterias: {
                    RequestStatus: searchParams.criterias.RequestStatus,
                    CustomerType: searchParams.criterias.CustomerType,
                    Outcome: searchParams.criterias.Outcome,
                    RequestId: searchParams.criterias.RequestId
                }
            })
              .done(function (data) {
                  $.each(data.Results, function (index, row) {
                      if (row.EsitoAutoVal != null) {
                          if (row.EsitoAutoVal.includes("OK") || row.EsitoAutoVal.includes("OK*") || row.EsitoAutoVal.includes("OK**"))
                              row["flag"] = flag.OK;
                          else if (row.EsitoAutoVal.includes("KO1"))
                              row["flag"] = flag.KO1;
                          else
                              row["flag"] = flag.ALL;
                      }
                      else
                          row["flag"] = flag.ALL;
                  });
                  callback(data)
              })
            .fail(function (jqXHR, httpStatusMessage, customErrorMessage) {
                failure(jqXHR, httpStatusMessage, customErrorMessage)
            })
            .always(function (jqXHR, textStatus, customErrorMessage) {
            });
    },

    getLotDetails: function (lotId, callback, failure) {
        $.post(
            "getcapreqs",
            { lotto: lotId })
            .done(function (data) {
                $.each(data, function (index, row) {
                    if (row.EsitoAutoVal.includes("OK") || row.EsitoAutoVal.includes("OK*") || row.EsitoAutoVal.includes("OK**"))
                        row["flag"] = flag.OK;
                    else if (row.EsitoAutoVal.includes("KO1"))
                        row["flag"] = flag.KO1;
                    else
                        row["flag"] = flag.ALL;
                });
                callback(data);
            })
            .fail(function (jqXHR, httpStatusMessage, customErrorMessage) {
                failure(jqXHR, httpStatusMessage, customErrorMessage);
            })
            .always(function (jqXHR, textStatus, errorThrown) {
            })
    },

    getCustomerCandidates: function (callback, failure) {
        $.post(
            "getcontrattodetail",
            { codCliente: ana.customerId })
            .done(function (data) {
                getEsito();
                callback(data);
            })
           .fail(function (jqXHR, httpStatusMessage, customErrorMessage) {
               failure(jqXHR, httpStatusMessage, customErrorMessage);
           })
            .always(function (jqXHR, textStatus, errorThrown) {
            })
    },

    getCustomerUtenza: function (utenza, callback, failure) {
        $.post(
            "getutenza",
            { utenza: utenza })
        .done(function (data) {
            if (data == "KO") {
                alert("Utenza non rilevata")
                return;
            }
            callback(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            failure(jqXHR, textStatus, errorThrow);
        })
    },

    //setCustomerUtenza: function (integra, callback,failure) {
    //    $.post(
    //       "forceintegra",
    //       { id: $("#ana1").val(), integra: integra, lscode: lscode })
    //    .done(function (data) {
    //        callback(data);
    //    })
    //      .fail(function (jqXHR, httpStatusMessage, customErrorMessage) {
    //          failure(jqXHR, httpStatusMessage, customErrorMessage);
    //      })
    //        .always(function (jqXHR, textStatus, errorThrown) {
    //        })
    //},

    getRequestDetail: function (customerId, esito, requestId, callback, failure) {
        $.post(
            "getclient",
            { id: requestId })
        .done(function (data) {
            callback(data);
        })
            .fail(function (jqXHR, httpStatusMessage, customErrorMessage) {
                failure(jqXHR, httpStatusMessage, customErrorMessage);
            })
            .always(function (jqXHR, textStatus, errorThrown) {
            })
    },

    getRequestvalidationDetails: function (requestId, callback, failure) {
        $.post("getRequestValidationDetails",
            { requestId: requestId })
        .done(function (data) {
            callback(data);
            pageUtils.triggerRadioOutcome();
        })
           .fail(function (jqXHR, httpStatusMessage, customErrorMessage) {
               failure(jqXHR, httpStatusMessage, customErrorMessage);
           })
            .always(function (jqXHR, textStatus, errorThrown) { })
    },


    validateLotto: function (lotId, callback, failure) {

        $.post("sgatevalidate", { lotto: lotId, reqId: null })
              .done(function (data) {
                  callback(data);
              })
           .fail(function (jqXHR, httpStatusMessage, customErrorMessage) {
               failure(jqXHR, httpStatusMessage, customErrorMessage);
           })
            .always(function (jqXHR, textStatus, errorThrown) { })
    },

    doForceValidation: function (Id, customerId, newOutcome, codcliente, callback, failure) {
        $.post(
         "forceintegra",
         { id: Id, integra: customerId, lscode: newOutcome, codcliente: codcliente })
         .done(function (data) { callback(data) })
           .fail(function (jqXHR, httpStatusMessage, customErrorMessage) {
               failure(jqXHR, httpStatusMessage, customErrorMessage);
           })
            .always(function (jqXHR, httpStatusMessage, customErrorMessage) { })
    },

    confirmLot: function (lotId, callback) {
        $.post("lotConfirm", { lotto: lotId })
              .done(function (data) {
                  callback(data);
              })
            .fail(function () {
                alert("error");
            });
    },

    downloadFileFromServer: function (lotId, callback) {
        var a = 1;

        window.open("download/" + lotId);

    },

    getLotProgress: function (callback) {
        $.post(
        "getLotProgress", {}, function (data) {
            callback(data);
        });
    },


}

function GetDate(jsonDate) {
    var value = new Date(parseInt(jsonDate.substr(6)));
    return value.getMonth() + 1 + "/" + value.getDate() + "/" + value.getFullYear();
}


//START
$(document).ready(function () {
    //    $.ajaxSetup({cache:false});
    $("#brand").text("Clienti CV");
    displayOff();
    pageUtils.hideActivityIndicator();
    $('#userTypeInfo').collapse('hide');
    $("#lotProgess").on("show.bs.dropdown", pageUtils.loadLotProgress());

    $("#myInput").keyup(function () {
        Filter();
    });

    $("#btnValidateLot").click(function () {
        var row = $("#tblLotBody tr.selected");

        pageUtils.doLotValidation($(row).attr("bi-lotId"));
    });

    $("#btnConfirmLot").click(function () {
        var row = $("#tblLotBody tr.selected");
        pageUtils.doLotConfirm($(row).attr("bi-lotId"));
    });

    $("#btnDownloadExcel").click(function () {
        var row = $("#tblLotBody tr.selected");
        pageUtils.doDownloadExcel($(row).attr("bi-lotId"));
    });

    $("#btnRefreshLotTable").click(function () {
        pageUtils.loadLotTable();
    });

    //seleziona lotto
    $("#tblLotList").on('click', 'a', function () {
        pageUtils.openLotDetail($(this).text());
    });

    //estrae codici fornitura per CF
    $("#btnSearchUtenze").on('click', function () {
        pageUtils.loadCustomerCandidates();
    });

    $("#codIntegra").keyup(function (e) {
        if (e.keyCode == 13) {
            pageUtils.openCustomerUtenza($("#codIntegra").val());
        }
    });

    $("#tblLotBody").on('click', 'tr', function (e) {
        if (status == 1) {
            status = 0;
            return;
        }
        row = $(this)
        pageUtils.selectLotTableRow(row);
        step1 = step;
    });

    $("#tblLotBody").blur(function () {
        pageUtils.selectLotTableRow();
    })

    $("#tblCustomerUtenzeBody").on('click', 'tr', function () {
        var utenza = $(this).find("td:eq(2)").text();
        ana.sel = 1;
        pageUtils.openGetCustomerDetail(utenza);
    });

    $("#btnSearch").click(displaySearchPopover());

    $('[data-toggle="tooltip"]').tooltip();

    appStart();
});


function appStart() {
    pageUtils.cleanLotTable();
    pageUtils.loadLotTable();
}

function displaySearchPopover() { }


function displayOff() {
    $("#btnValidateLot").attr("disabled", "disabled");
    $("#btnConfirmLot").attr("disabled", "disabled");
    $("#btnDownloadExcel").attr("disabled", "disabled");
    $("#btnDownloadEsito").attr("disabled", "disabled");

    $("#btnForzaUtenza").hide();
    $("#pls").show();
    $("#pls").removeClass("pulse");
}


function displayOffForn() {
    displayOff();
    $("#btnUploadFile").show();
    $("#pv-fornitura").show();
    $("#btninfoLottoUpload").show();
    $("#uploader").hide();
    $("#utente").show();
}



function displaySearch() {
    $("#CustomerDetail").empty();
    $(".interno").show();
    $("#utenza").show();
    $("#codIntegra").show();
    $("#btnForzaUtenza").show();
    $("#btnGetUtenza").show();
    $("#tblEsito").show();
}

function GetLotti() {
    var label = "";
    $.post(
        "biacquisizione",
        function (res) {
            FillTable(res);
            return;
        });
}


function GetCheched(lt) {
    var dic = {};
    var ar = [];
    var lsId = [];
    var lsck1 = [];
    var xx = 0;
    var id = 0;

    $("#tb61 tr").each(function () {
        if ($(this).find('input[type="checkbox"]').is(':checked')) {
            var t = ($(this).children().find("td:eq(7)").val());

            lt = $(this).find("td:eq(6)").text();
            lsId.push($(this).find("td:eq(7)").text());
        }
        else
            ar.push($(this));
        t = 1;
    });
    Validate(lt, lsId);
    return ar;
}

function Validate(lotto, lsId) {
    $.post(
        "sgatevalidate",
        { lotto: lotto, reqId: lsId },
        function (res) {
            $('#tblLotList').find("tbody").empty();
            FillTable(res);
        });
}

function getEsito() {
    var label = "";
    var lscode = [];

    lscode.push("OK");
    lscode.push("OK*");
    lscode.push("OK**");
    lscode.push("KO1");
    lscode.push("KO2");
    lscode.push("KO3");
    lscode.push("KO4");
    lscode.push("KO5");
    lscode.push("KO6");
    lscode.push("KO7");

    label = label + '<tr class="header">';
    $.each(lscode, function (index, value) {
        label = label + '<td align="right"><input type="checkbox" >' + value + '</input></td>';
        label = label + '<td style="display: none">' + value + '</td>';
    });
    label = label + '</tr>';

    //  cliente = codcliente != null ? codcliente : "nd";

    $("#tblEsitoBody").html(label);

}

function CheckUser() {
    var lscode = [];

    $("#tblEsitoBody tr td").each(function () {
        if ($(this).find('input[type="checkbox"]').is(':checked')) {
            lscode.push($(this).text());
        }
        else
            var t = 0;
    });
    if (lscode.length == 0) {
        alert("Inserire esito richiesta");
        return 1;
    }
    var integra = $("#codIntegra").val() == "" ? ana.integra : $("#codIntegra").val();

    $.post(
     "forceintegra",
     { id: ana.id, integra: integra, lscode: lscode },
     function (res) {
         //    if (res == 1)
         alert("Inserimento andato a buon fine")
         //else
         //    alert("Inserimento non effettuato")
         var btnrequest = document.getElementById("btnRequestDetails");
         btnrequest.click();

         var btnlotdetail = document.getElementById("btnLotDetails");
         btnlotdetail.click();
     });

}

function Filter() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    $("#tblLotDetailsBody tr").each(function () {
        td = $(this).find("td:eq(5)").text();

        if (td) {
            if (td.toUpperCase().indexOf(filter) > -1) {
                $(this).show();
            } else {
                $(this).hide();
            }
        }
    });
}


function HandleBrowseClick() {
    var DataCarico = $("#lottodateload").val();
    if (DataCarico == null || DataCarico == "") {
        notifyWarning("Data di presa in carico obbligatoria");
        return;
    }
    var fileinput = document.getElementById("filesInput");
    fileinput.click();
}

function Handlechange() {
    var DataAcquisizione = $("#lottodateacq").text();
    var DataCarico = $("#lottodateload").val();
    var DataScadenza = $("#lottodateend").text();
    var Desc = $("#lottodescription").val();


    var xhre = new XMLHttpRequest();
    var fdata = new FormData();
    fdata.append("file", document.getElementById('filesInput').files[0]);
    fdata.append('DataAcquisizione', DataAcquisizione);
    fdata.append('DataCarico', DataCarico);
    fdata.append('DataScadenza', DataScadenza);
    fdata.append('Desc', Desc);

    xhre.open("POST", "upload", true);
    xhre.send(fdata);

    //  var results;
    $("#btnCloseUpload").click();
    pageUtils.showActivityIndicator("Caricamento lotti in corso...");

    xhre.onreadystatechange = function () {
        if (this.readyState > 3) {

            var content = JSON.parse(xhre.responseText);
            if (content != 'undefined' && content != null && content != '') {
                $("#tblLotBody").empty();
                $("#tmpl-lotTableRow").tmpl(content).appendTo("#tblLotBody");
                pageUtils.hideActivityIndicator();
            }
            notifyInfo("Caricamento del file eseguito.");
        }
    }


    //xhre.onreadystatechange = function () {
    //    $("#btnCloseUpload").click();
    //    if (this.readyState === 4) {
    //        pageUtils.showActivityIndicator("Caricamento lotti in corso...");

    //        var content = JSON.parse(xhre.responseText);
    //        if (content != 'undefined' && content != null && content != '' && content != results) {
    //    //        pageUtils.loadLotTable();
    //            $("#tblLotBody").empty();
    //            $("#tmpl-lotTableRow").tmpl(content).appendTo("#tblLotBody");
    //            pageUtils.hideActivityIndicator();
    //        }           
    //        notifyInfo("Caricamento del file eseguito.");
    //        pageUtils.hideActivityIndicator();
    //    }
    //}


}