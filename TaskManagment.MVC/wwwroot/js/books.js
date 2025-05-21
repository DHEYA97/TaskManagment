$(document).ready(function () {

    var eventId = $('#booksTable').data('event-id');
    var templateId = $('#booksTable').data('template-id');
    var templateIdInt = $('#booksTable').data('template-idint');
    if (!eventId || !templateId) {
        console.error("❌ eventId أو templateId غير معرفين!");
        return;
    }
    console.log("✅ template Id Int:", templateIdInt);
    console.log("✅ eventId:", eventId, "✅ templateId:", templateId);
    if ($("#booksTable").length === 0) {
        console.error("❌ خطأ: الجدول #booksTable غير موجود في الـ DOM!");
        return;
    }
    $('[data-kt-filter="search"]').on('keyup', function () {
        datatable.search($(this).val()).draw();
    });
    var datatable = $("#booksTable").DataTable({
        serverSide: true,
        stateSave: true,
        processing: true,
        language: {
            processing: '<div class="d-flex justify-content-center text-primary align-items-center dt-spinner"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div><span class="text-muted ps-2">Loading...</span></div>'
        },
        ajax: {
            url: `/EventRegistration/GetEventRegistrations?eventId=${eventId}&templateId=${templateId}`,
            type: 'POST'
        },
        'drawCallback': function () {
            KTMenu.createInstances();
        },
        order: [[1, 'asc']],
        columnDefs: [{
            targets: [0],
            visible: false,
            searchable: false
        }],
        columns: [
            { 'data': 'id', 'name': 'Id', 'className': 'd-none' },
            { 'data': 'name', 'name': 'Name' },
            { 'data': 'jopTitle', 'name': 'JopTitle' },
            { 'data': 'companyName', 'name': 'CompanyName' },
            { 'data': 'city', 'name': 'City' },
            { 'data': 'email', 'name': 'Email' },
            { 'data': 'phone', 'name': 'Phone' },
            {
                'name': 'CreatedOn',
                'render': function (data, type, row) {
                    return moment(row.createdOn).format('ll');
                }
            },
            {
                'name': 'LastUpdatedOn',
                'render': function (data, type, row) {
                    return moment(row.lastUpdatedOn).format('ll');
                }
            },
            {
                "className": 'text-end',
                "orderable": false,
                "render": function (data, type, row) {
                    return `<a href="#" class="btn btn-light btn-active-light-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end">
                            Actions
                            <span class="svg-icon svg-icon-5 m-0">
                                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path d="M11.4343 12.7344L7.25 8.55005C6.83579 8.13583 6.16421 8.13584 5.75 8.55005C5.33579 8.96426 5.33579 9.63583 5.75 10.05L11.2929 15.5929C11.6834 15.9835 12.3166 15.9835 12.7071 15.5929L18.25 10.05C18.6642 9.63584 18.6642 8.96426 18.25 8.55005C17.8358 8.13584 17.1642 8.13584 16.75 8.55005L12.5657 12.7344C12.2533 13.0468 11.7467 13.0468 11.4343 12.7344Z" fill="currentColor"></path>
                                </svg>
                            </span>
                        </a>
                        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-semibold w-200px py-3" data-kt-menu="true">
                            <div class="menu-item px-3">
                                <a href="/EventRegistration/${templateIdInt == 1 ? `EditRegisterForm` : templateIdInt == 2 ? `EditSponsorForm` : `EditLocationForm`}/${row.id}" class="menu-link px-3">
                                    Edit
                                </a>
                            </div>
                            <div class="menu-item px-3">
                                <a href="/EventRegistration/${templateIdInt == 1 ? `DeleteRegisterForm` : templateIdInt == 2 ? `DeleteSponsorForm` : `DeleteLocationForm`}/${row.id}" class="menu-link px-3">
                                    Delete
                                </a>
                            </div>
                            <div class="menu-item px-3">
                                <a href="/EventRegistration/QrCode/${row.id}?templateId=${templateId}" class="menu-link px-3">
                                    QrCode
                                </a>
                            </div>
                            <div class="menu-item px-3">
                                <a href="/EventRegistration/VisitorCard/${row.id}?templateId=${templateId}" target="_blank"  class="menu-link px-3">
                                    Visitor Card
                                </a>
                            </div>
                        </div>`;
                }
            }
            ]
    });
});