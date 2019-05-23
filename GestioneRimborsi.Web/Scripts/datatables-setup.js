
$.extend( $.fn.dataTable.defaults, {
    searching: false,
    pageLength: 10,
    pagingType: "full_numbers",
    language: {
        info:           "Record da <strong>_START_</strong> a <strong>_END_</strong> di <strong>_TOTAL_</strong>",
        infoEmpty:      "Nessun record visualizzato",
        zeroRecords:    "Nessun record trovato",
        emptyTable:     "Nessun record trovato",
        paginate: {
            first:      "<<",
            previous:   "<",
            next:       ">",
            last:       ">>"
        }
	},
	stateSave: false
});