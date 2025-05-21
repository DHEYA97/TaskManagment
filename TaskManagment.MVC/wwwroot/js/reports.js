$(document).ready(function () {
    $('.page-link').on('click', function () {
        var btn = $(this);
        console.log(btn);
        var pageNumber = btn.data('page-number');
        console.log(pageNumber);

        if (btn.parent().hasClass('active')) return;

        $('#EventAttendeesEventReportsSearchModel_PageNumber').val(pageNumber);
        $('#Filters').submit();
    });
});