$(document).ready(function () {
    $('#EventAttendeesEventReportsSearchModel_EventId').on('change', function () {
        var eventId = $(this).val();
        console.log(eventId);
        var templateList = $('#EventAttendeesEventReportsSearchModel_SelectedTemplateIds');
        console.log(eventId);
        templateList.empty();
        templateList.append('<option></option>');

        if (eventId !== '') {
            $.ajax({
                url: '/EventAttendee/GetTemplates?eventId=' + eventId,
                success: function (templates) {
                    $.each(templates, function (i, template) {
                        var item = $('<option></option>').attr("value", template.value).text(template.text);
                        templateList.append(item);
                    });
                },
                error: function () {
                    showErrorMessage();
                }
            });
        }
    });
});