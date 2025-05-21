var updateedRow;
var table;
var datatable;
var exportedCols = [];
var copyCount;
var currentLanguage = $('#CurrentLanguage').text();
function showSuccessMessage(message = localization.savedSuccessfully) {
    Swal.fire({
        icon: 'success',
        title: localization.goodJob,
        text: message,
        confirmButtonText: localization.ok,
        cancelButtonText: localization.cancel,
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}

function disablesSubmetButton(btn) {
    $(btn).attr('disabled', 'disabled').attr('data-kt-indicator','on');
}
function onModelBegin() {
    disablesSubmetButton($('#Modal').find(':submit'));
}
function onModelSuccess(row) {
    showSuccessMessage();
    $(".modal").modal("hide");
    console.log(row);
    if (updateedRow !== undefined) {
        datatable.row(updateedRow).remove().draw();
        updateedRow = undefined;
    }
    var newRow = $(row);
    datatable.row.add(newRow).draw();
    if ($("js-alert") !== undefined) {
        $('.js-alert').addClass('d-none');
        $('table').removeClass('d-none');
    }
    if (copyCount !== undefined && updateedRow === undefined) {
        $('#count').text(parseInt($('#count').text()) + 1);
    }
    KTMenu.init();
    KTMenu.initHandlers();
}
function onModelFailure(message) {
    showErrorMessage(message)
}
function onModelComplete() {
    $("body :submit").removeAttr('disabled').removeAttr('data-kt-indicator');
}
function showErrorMessage(message = localization.somethingWrong) {
    Swal.fire({
        icon: 'error',
        title: localization.Oops,
        text: message.responseText !== undefined ? message.responseText : message,
        confirmButtonText: localization.ok,
        cancelButtonText: localization.cancel,
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}
function applaySelect2() {
    $('.js-select2').select2();
    $('.js-select2').on('select2:select', function (e) {
        var select = $(this);
        $('form').not("#SignOut").validate().element('#' + select.attr('id'));
    });
}

//DataTable
var headers = $('th');
$.each(headers, function (i) {
    if (!$(this).hasClass('js-no-export')) {
        exportedCols.push(i);
    }
});

var KTDatatables = function () {
    // Shared variables

    // Private functions
    var initDatatable = function () {
        // Set date data order
        const tableRows = table.querySelectorAll('tbody tr');
        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'pageLength': 10,
            order: [[2, 'desc']],
            'drawCallback': function () {
                KTMenu.createInstances();
            },
            language: {
                url: currentLanguage !== 'en' ? `../assets/plugins/datatables/i18n/${currentLanguage}.json` : undefined,
            },
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $('.js-datatables').data('document-title');
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    },
                    customize: function (doc) {
                        pdfMake.fonts = {
                            Arial: {
                                normal: 'arial',
                                bold: 'arial',
                                italics: 'arial',
                                bolditalics: 'arial'
                            }
                        }
                        doc.defaultStyle.font = 'Arial';
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatabels');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();

$(document).ready(function () {
    //TyneMce
    if ($('.js-tinymce').length > 0) {
        var options = {
            selector: ".js-tinymce",
            height: "430",
            directionality: currentLanguage == 'ar' ? 'rtl' : 'ltr',
            language: currentLanguage != 'en' ? currentLanguage : undefined,
        };

        if (KTThemeMode.getMode() === "dark") {
            options["skin"] = "oxide-dark";
            options["content_css"] = "dark";
        }

        tinymce.init(options);
    }

    //Select2
    applaySelect2();


    //Date picker
    $('.js-datepicker').daterangepicker({
        singleDatePicker: true,
        autoApply: true,
        drops: 'up',
        locale: {
            format: 'yyyy-MM-DD'
        },
        maxDate: new Date(),
        showDropdowns: true
    });
    $('#js-datepicker').val($('#js-datepicker').data('getpublishingdate'));
    //SweetAlert
    var message = $('#message').text();
    if (message != '') {
        showSuccessMessage(message)
    }

    //DataTable 
    KTUtil.onDOMContentLoaded(function () {
        KTDatatables.init();
    });
    
    //disablesSubmetButton
    $('form').not('#SignOut').not('.js-excluded-validation').on('submit', function () {
        if ($('.js-tinymce').length > 0) {
            $('.js-tinymce').each(function () {
                var input = $(this);

                var content = tinyMCE.get(input.attr('id')).getContent();
                input.val(content);
            });
        }

        var isValid = $(this).valid();
        if (isValid) disableSubmitButton($(this).find(':submit')); 
    });

    //Add Model
    $('body').delegate('.js-render-model','click', function () {
        var model = $(".modal");
        btn = $(this);
        var title = btn.data('title');
        var url = btn.data('url');
        var body = model.find('.modal-body');
        model.find('.modal-title').text(title);
        if (btn.data('updated') !== undefined) {
            updateedRow = btn.parents('tr');
        }
        if (btn.data('count') !== undefined) {
            copyCount = btn.data('count');
        } 
        $.get({
            url: url,                   
            success: function (form) {
                body.html(form);;
                $.validator.unobtrusive.parse(model);
                applaySelect2();
            },
            error: function () {
                showErrorMessage()
            }
        });
        model.modal('show');
    })

    //Toggle Status
    $('body').delegate('.js-toggle-status', 'click', function () {
        btn = $(this);
        var title = btn.data('title');
        Swal.fire({
            title: localization.toggleStatusConfirmation,
            showDenyButton: true,
            confirmButtonText: localization.yes,
            denyButtonText: localization.no,
            customClass: {
                confirmButton: "btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary",
                denyButton: "btn btn-outline btn-outline-dashed btn-outline-danger btn-active-light-danger"
            }
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                var btn = $(this);
                $.post({
                    url: btn.data('url'),
                    data: {
                        '__RequestVerificationToken': $('input[name = "__RequestVerificationToken"]').val()
                    },
                    success: function (LastUpdatedOn) {
                        var row = btn.parents('tr');
                        if (LastUpdatedOn == true) {
                            datatable.row(row).remove().draw();
                            showSuccessMessage("Deleted Succuss");
                        } else {

                            var status = row.find('.js-status');
                            var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                            status.text(newStatus);
                            status.toggleClass('badge-light-danger badge-light-success');
                            row.find('.js-last-updated').html(LastUpdatedOn);
                            row.toggleClass('animate__animated animate__flash');
                            showSuccessMessage()
                        }
                    },
                    error: function () {
                        showErrorMessage()
                    }
                })
            }
        });
    });

    //Handle Confirm
    $('body').delegate('.js-confirm', 'click', function () {
        var btn = $(this);

        bootbox.confirm({
            message: localization.toggleStatusConfirmation,
            buttons: {
                confirm: {
                    label: localization.yes,
                    className: 'btn-danger'
                },
                cancel: {
                    label: localization.no,
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function () {
                            showSuccessMessage();
                        },
                        error: function () {
                            showErrorMessage();
                        }
                    });
                }
            }
        });
    });

    $(".js-signOut").on('click', function () {
        $("#SignOut").submit();
    });
});