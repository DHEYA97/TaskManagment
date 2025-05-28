var localization = {
    "noBooksFound": "No books found!",
    "toggleStatusConfirmation": "Are you sure you want to change the status of this item?",
    "yes": "Yes",
    "no": "No",
    "savedSuccessfully": "Saved successfully!",
    "somethingWrong": "Something went wrong!",
    "goodJob": "Good job",
    "Oops": "Warning",
    "ok": "OK",
    "cancel": "Cancel",
    "AddnewEvent": "Add new task",
    "newEventAdded": "Task added successfully",
    "eventUpdateed": "Task updated successfully",
    "errorAdd": "Sorry, some errors were detected. Please try again.",
    "editEvent": "Edit task",
    "eventNameRequired": "Task name is required",
    "startDateRequired": "Start date is required",
    "endDateRequired": "End date is required",
    "deleteEvent": "Are you sure you want to delete the task?",
    "confirmDeleteEventText": "Yes, delete it",
    "eventNotDelet": "Task was not deleted",
    "eventDeletedSuccessfully": "Task deleted successfully",
    "eventCancel": "Are you sure you want to cancel?",
    "noRetuern": "No, go back",
    "eventNoCancel": "The cancellation will not occur",
    "eventNameLength": "The name must be between 1 and 100 characters",
    "startDateInvalid": "Task start date must be greater than the current time",
    "endDateInvalid": "Task end date must be greater than now and the start date",
    "locale": "en",
    "direction": "ltr",
    "taskAdded": "New task added",
    "taskEdited": "Task edited",
    "reminder": "Reminder",
    "taskStarted": "Task started",
    "taskStartsIn": "The task",
    "oneMinute": "will start in one minute",
    "twoMinutes": "will start in two minutes",
    "after": "will start in",
    "minutes": "minutes",
    "allDay": "All day"
};


var daterangePickerLocale = {
    "applyLabel": "Apply",
    "cancelLabel": "Cancel",
    "fromLabel": "From",
    "toLabel": "To",
    "customRangeLabel": "Custom",
    "weekLabel": "W",
    "daysOfWeek": [
        "Su",
        "Mo",
        "Tu",
        "We",
        "Th",
        "Fr",
        "Sa"
    ],
    "monthNames": [
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December"
    ]
}

var daterangePickerRanges = {
    'Today': [moment(), moment()],
    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
    'This Month': [moment().startOf('month'), moment().endOf('month')],
    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
}