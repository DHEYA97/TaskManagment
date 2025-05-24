"use strict";
var KTAppCalendar = (function () {
    var e,
        t,
        n,
        a,
        o,
        r,
        i,
        l,
        d,
        c,
        s,
        m,
        u,
        v,
        f,
        p,
        y,
        D,
        k,
        _,
        b,
        g,
        S,
        h,
        T,
        Y,
        w,
        x,
        L,
        E = {
            id: "",
            eventName: "",
            eventDescription: "",
            eventLocation: "",
            startDate: "",
            endDate: "",
            allDay: !1,
        };
        var connection = new signalR.HubConnectionBuilder()
                                    .withUrl("/taskHub", 
                                        {
                                            accessTokenFactory: () => {
                                                return localStorage.getItem('accessToken') || '';
                                            }
                                        }
                                    )
                                    .configureLogging(signalR.LogLevel.Debug)
                                    .build();

        console.log("Is Manager:", window.isManager, typeof window.isManager);

        if (window.isManager == true) {
            console.log("yes yes yes");
            connection.on("addtask", function (task) {
                console.log("Connection addtask successfully.");
                console.log("Received task:", task);
                e.addEvent({
                    id: task.id,
                    title: task.title,
                    description: task.description,
                    location: task.location,
                    start: task.start,
                    end: task.end,
                    allDay: task.allDay,
                });
                e.render();
                f.reset();
            });
        }

        connection.start()
            .then(function () {
                console.log("Connection started successfully.");
                console.log(window.isManager);
            })
            .catch(function (err) {
                console.error("Error while starting connection: ", err);
            });
            
    const M = () => {
        (v.innerText = localization.AddnewEvent), u.show();
        const o = f.querySelectorAll('[data-kt-calendar="datepicker"]'),
            i = f.querySelector("#kt_calendar_datepicker_allday");
        i.addEventListener("click", (e) => {
            e.target.checked
                ? o.forEach((e) => {
                    e.classList.add("d-none");
                })
                : (l.setDate(E.startDate, !0, "Y-m-d"),
                    o.forEach((e) => {
                        e.classList.remove("d-none");
                    }));
        }),
            C(E),
            D.addEventListener("click", function (o) {
                o.preventDefault(),
                    p &&
                    p.validate().then(function (o) {
                        console.log("validated!"),
                            "Valid" == o
                                ? (D.setAttribute("data-kt-indicator", "on"),
                                    (D.disabled = !0),
                                    setTimeout(function () {
                                        D.removeAttribute("data-kt-indicator"),
                                            u.hide(), (D.disabled = !1);
                                            let o = !1;
                                            i.checked && (o = !0),
                                                0 === c.selectedDates.length && (o = !0);
                                            var d = moment(r.selectedDates[0]).format(),
                                                s = moment(
                                                    l.selectedDates[l.selectedDates.length - 1]
                                                ).format();
                                            if (!o) {
                                                const e = moment(r.selectedDates[0]).format(
                                                    "YYYY-MM-DD"
                                                ),
                                                    t = e;
                                                (d =
                                                    e +
                                                    "T" +
                                                    moment(c.selectedDates[0]).format(
                                                        "HH:mm:ss"
                                                    )),
                                                    (s =
                                                        t +
                                                        "T" +
                                                        moment(m.selectedDates[0]).format(
                                                            "HH:mm:ss"
                                                        ));
                                            }
                                            const token = $('input[name="__RequestVerificationToken"]').val();
                                            const formData = new FormData();
                                            formData.append("Name", t.value);
                                            formData.append("Description", n.value);
                                            formData.append("Location", a.value);
                                            formData.append("StartDate", d);
                                            formData.append("EndDate", s);
                                            formData.append("__RequestVerificationToken", token);
                                            console.log(formData);
                                            $.ajax({
                                                url: '/Event/Create',
                                                type: 'POST',
                                                data: formData,
                                                processData: false,
                                                contentType: false,
                                                success: function (result) {
                                                    if (result.success) {
                                                        Swal.fire({
                                                        text: localization.newEventAdded,
                                                        icon: "success",
                                                        buttonsStyling: !1,
                                                        confirmButtonText: localization.yes,
                                                        customClass: { confirmButton: "btn btn-primary" },
                                                        }).then(function (o) {
                                                            if (o.isConfirmed) {
                                                                const newEvent = {
                                                                        id: A(),
                                                                        title: t.value,
                                                                        description: n.value,
                                                                        location: a.value,
                                                                        start: d,
                                                                        end: s,
                                                                        allDay: o,
                                                                    };
                                                                    e.addEvent(newEvent);
                                                                    e.render();
                                                                    f.reset();
                                                                    connection.invoke("SendTaskToManagers", newEvent)
                                                                        .catch(function (err) {
                                                                            console.error("SignalR Error:", err.toString());
                                                                        });
                                                                    // location.reload();
                                                                }});
                                                    } else {
                                                        Swal.fire({
                                                            text: result.message || localization.errorAdd,
                                                            icon: "error",
                                                            buttonsStyling: !1,
                                                            confirmButtonText: localization.yes,
                                                            customClass: { confirmButton: "btn btn-primary" },
                                                        });
                                                    }
                                                },
                                                error: function (xhr, status, error) {
                                                    Swal.fire({
                                                        text: localization.errorAdd,
                                                        icon: "error",
                                                        buttonsStyling: !1,
                                                        confirmButtonText: localization.yes,
                                                        customClass: { confirmButton: "btn btn-primary" },
                                                    });
                                                    console.error("Error:", xhr.responseText);
                                                }
                                            });
                                    }, 2e3))
                                : Swal.fire({
                                    text: localization.errorAdd,
                                    icon: "error",
                                    buttonsStyling: !1,
                                    confirmButtonText: localization.yes,
                                    customClass: { confirmButton: "btn btn-primary" },
                                });
                    });
            });
    },
        B = () => {
            var e, t, n;
            w.show(),
                E.allDay
                    ? ((e = "All Day"),
                        (t = moment(E.startDate).format("Do MMM, YYYY")),
                        (n = moment(E.endDate).format("Do MMM, YYYY")))
                    : ((e = ""),
                        (t = moment(E.startDate).format("Do MMM, YYYY - h:mm a")),
                        (n = moment(E.endDate).format("Do MMM, YYYY - h:mm a"))),
                (b.innerText = E.eventName),
                (g.innerText = e),
                (S.innerText = E.eventDescription ? E.eventDescription : "--"),
                (h.innerText = E.eventLocation ? E.eventLocation : "--"),
                (T.innerText = t),
                (Y.innerText = n);
        },
        q = () => {
            x.addEventListener("click", (o) => {
                o.preventDefault(),
                    w.hide(),
                    (() => {
                        (v.innerText = localization.editEvent), u.show();
                        const o = f.querySelectorAll('[data-kt-calendar="datepicker"]'),
                            i = f.querySelector("#kt_calendar_datepicker_allday");
                        i.addEventListener("click", (e) => {
                            e.target.checked
                                ? o.forEach((e) => {
                                    e.classList.add("d-none");
                                })
                                : (l.setDate(E.startDate, !0, "Y-m-d"),
                                    o.forEach((e) => {
                                        e.classList.remove("d-none");
                                    }));
                        }),
                            C(E),
                            D.addEventListener("click", function (o) {
                            o.preventDefault(),
                                p &&
                                p.validate().then(function (o) {
                                    console.log("validated!"),
                                        "Valid" == o
                                            ? (D.setAttribute("data-kt-indicator", "on"),
                                                (D.disabled = !0),
                                                setTimeout(function () {
                                                    // إعداد بيانات الإرسال
                                                    let isAllDay = i.checked || c.selectedDates.length === 0;
                                                    let d = moment(r.selectedDates[0]).format(),
                                                        s = moment(
                                                            l.selectedDates[l.selectedDates.length - 1]
                                                        ).format();

                                                    if (!isAllDay) {
                                                        const e = moment(r.selectedDates[0]).format("YYYY-MM-DD"),
                                                            t = e;
                                                        d = e + "T" + moment(c.selectedDates[0]).format("HH:mm:ss");
                                                        s = t + "T" + moment(m.selectedDates[0]).format("HH:mm:ss");
                                                    }

                                                    const eventData = {
                                                        Id: E.id,
                                                        Name: t.value,
                                                        Description: n.value,
                                                        Location: a.value,
                                                        StartDate: d,
                                                        EndDate: s,
                                                        __RequestVerificationToken: document.querySelector('input[name="__RequestVerificationToken"]')?.value
                                                    };

                                                    $.ajax({
                                                        url: "/Event/Edit",
                                                        type: "POST",
                                                        data: eventData,
                                                        success: function (res) {
                                                            if (!res.success) {
                                                                Swal.fire({
                                                                    text: res.message,
                                                                    icon: "error",
                                                                    buttonsStyling: !1,
                                                                    confirmButtonText: localization.yes,
                                                                    customClass: { confirmButton: "btn btn-primary" },
                                                                });
                                                                D.removeAttribute("data-kt-indicator");
                                                                D.disabled = !1;
                                                                return;
                                                            }
                                                                Swal.fire({
                                                                text: localization.eventUpdateed,
                                                                icon: "success",
                                                                buttonsStyling: !1,
                                                                confirmButtonText: localization.yes,
                                                                customClass: {
                                                                    confirmButton: "btn btn-primary",
                                                                },
                                                            }).then(function (o) {
                                                                if (o.isConfirmed) {
                                                                    u.hide(),
                                                                        (D.disabled = !1),
                                                                        e.getEventById(E.id).remove();
                                                                    let o = !1;
                                                                    i.checked && (o = !0),
                                                                        0 === c.selectedDates.length && (o = !0);
                                                                    var d = moment(r.selectedDates[0]).format(),
                                                                        s = moment(
                                                                            l.selectedDates[
                                                                            l.selectedDates.length - 1
                                                                            ]
                                                                        ).format();
                                                                    if (!o) {
                                                                        const e = moment(
                                                                            r.selectedDates[0]
                                                                        ).format("YYYY-MM-DD"),
                                                                            t = e;
                                                                        (d =
                                                                            e +
                                                                            "T" +
                                                                            moment(c.selectedDates[0]).format(
                                                                                "HH:mm:ss"
                                                                            )),
                                                                            (s =
                                                                                t +
                                                                                "T" +
                                                                                moment(m.selectedDates[0]).format(
                                                                                    "HH:mm:ss"
                                                                                ));
                                                                    }
                                                                    e.addEvent({
                                                                        id: A(),
                                                                        title: t.value,
                                                                        description: n.value,
                                                                        location: a.value,
                                                                        start: d,
                                                                        end: s,
                                                                        allDay: o,
                                                                    }),
                                                                        e.render(),
                                                                        f.reset();
                                                                        location.reload();
                                                                }
                                                            });
                                                        },
                                                        error: function (xhr) {
                                                            D.removeAttribute("data-kt-indicator");
                                                            D.disabled = !1;
                                                            Swal.fire({
                                                                text: xhr.responseText || localization.errorUpdate,
                                                                icon: "error",
                                                                buttonsStyling: !1,
                                                                confirmButtonText: localization.yes,
                                                                customClass: { confirmButton: "btn btn-primary" },
                                                            });
                                                        }
                                                    });
                                                }, 2e3))
                                            : Swal.fire({
                                                text: localization.errorAdd,
                                                icon: "error",
                                                buttonsStyling: !1,
                                                confirmButtonText: localization.yes,
                                                customClass: { confirmButton: "btn btn-primary" },
                                            });
                                });
                            });

                    })();
            });
        },
        C = () => {
            (t.value = E.eventName ? E.eventName : ""),
                (n.value = E.eventDescription ? E.eventDescription : ""),
                (a.value = E.eventLocation ? E.eventLocation : ""),
                r.setDate(E.startDate, !0, "Y-m-d");
            const e = E.endDate ? E.endDate : moment(E.startDate).format();
            l.setDate(e, !0, "Y-m-d");
            const o = f.querySelector("#kt_calendar_datepicker_allday"),
                i = f.querySelectorAll('[data-kt-calendar="datepicker"]');
            E.allDay
                ? ((o.checked = !0),
                    i.forEach((e) => {
                        e.classList.add("d-none");
                    }))
                : (c.setDate(E.startDate, !0, "Y-m-d H:i"),
                    m.setDate(E.endDate, !0, "Y-m-d H:i"),
                    l.setDate(E.startDate, !0, "Y-m-d"),
                    (o.checked = !1),
                    i.forEach((e) => {
                        e.classList.remove("d-none");
                    }));
        },
        N = (e) => {
            (E.id = e.id),
                (E.eventName = e.title),
                (E.eventDescription = e.description),
                (E.eventLocation = e.location),
                (E.startDate = e.startStr),
                (E.endDate = e.endStr),
                (E.allDay = e.allDay);
        },
        A = () =>
            Date.now().toString() + Math.floor(1e3 * Math.random()).toString();
    return {
        init: function () {
            const C = document.getElementById("kt_modal_add_event");
            (f = C.querySelector("#kt_modal_add_event_form")),
                (t = f.querySelector('[name="calendar_event_name"]')),
                (n = f.querySelector('[name="calendar_event_description"]')),
                (a = f.querySelector('[name="calendar_event_location"]')),
                (o = f.querySelector("#kt_calendar_datepicker_start_date")),
                (i = f.querySelector("#kt_calendar_datepicker_end_date")),
                (d = f.querySelector("#kt_calendar_datepicker_start_time")),
                (s = f.querySelector("#kt_calendar_datepicker_end_time")),
                (y = document.querySelector('[data-kt-calendar="add"]')),
                (D = f.querySelector("#kt_modal_add_event_submit")),
                (k = f.querySelector("#kt_modal_add_event_cancel")),
                (_ = C.querySelector("#kt_modal_add_event_close")),
                (v = f.querySelector('[data-kt-calendar="title"]')),
                (u = new bootstrap.Modal(C));
            const H = document.getElementById("kt_modal_view_event");
            var F, O, I, R, V, P;
            (w = new bootstrap.Modal(H)),
                (b = H.querySelector('[data-kt-calendar="event_name"]')),
                (g = H.querySelector('[data-kt-calendar="all_day"]')),
                (S = H.querySelector('[data-kt-calendar="event_description"]')),
                (h = H.querySelector('[data-kt-calendar="event_location"]')),
                (T = H.querySelector('[data-kt-calendar="event_start_date"]')),
                (Y = H.querySelector('[data-kt-calendar="event_end_date"]')),
                (x = H.querySelector("#kt_modal_view_event_edit")),
                (L = H.querySelector("#kt_modal_view_event_delete")),
                (F = document.getElementById("kt_calendar_app")),
                (O = moment().startOf("day")),
                (I = O.format("YYYY-MM")),
                (R = O.clone().subtract(1, "day").format("YYYY-MM-DD")),
                (V = O.format("YYYY-MM-DD")),
                (P = O.clone().add(1, "day").format("YYYY-MM-DD")),
                (e = new FullCalendar.Calendar(F, {
                    locale: localization.locale,
                    direction: localization.direction,
                    headerToolbar: {
                        left: "prev,next today",
                        center: "title",
                        right: "dayGridMonth,timeGridWeek,timeGridDay,listWeek",
                    },
                    initialDate: V,
                    initialView: 'timeGridDay',
                    navLinks: !0,
                    selectable: !0,
                    selectMirror: !0,
                    slotDuration: '00:30:00',          
                    slotLabelInterval: '00:30:00',    
                    slotMinTime: '00:00:00',           
                    slotMaxTime: '24:00:00',
                    scrollTime: '08:00:00',           
                    select: function (e) {
                        N(e), M();
                    },
                    eventClick: function (e) {
                        N({
                            id: e.event.id,
                            title: e.event.title,
                            description: e.event.extendedProps.description,
                            location: e.event.extendedProps.location,
                            startStr: e.event.startStr,
                            endStr: e.event.endStr,
                            allDay: e.event.allDay,
                        }),
                            B();
                    },
                    editable: 1,
                    dayMaxEvents: !0,
                    events: function(fetchInfo, successCallback, failureCallback) {
                            $.ajax({
                                url: '/Event/GetAll',
                                method: 'GET',
                                success: function(response) {
                                    if (response.success) {
                                        const mappedEvents = response.message.map(function(event) {
                                            return {
                                                id: event.id,
                                                title: event.name,
                                                start: new Date(event.startDate).toISOString(),
                                                end: new Date(event.endDate).toISOString(),
                                                description: event.description,
                                                location: event.location,
                                                className: 'fc-event-primary'
                                            };
                                        });

                                        successCallback(mappedEvents);
                                    } else {
                                        failureCallback('حدث خطأ أثناء تحميل البيانات');
                                    }
                                },
                                error: function() {
                                    failureCallback('تعذر الاتصال بالسيرفر');
                                }
                            });
                        },
                    datesSet: function () { },
                })).render(),
                (p = FormValidation.formValidation(f, {
                    fields: {
                        calendar_event_name: {
                            validators: {
                                notEmpty: {
                                    message: localization.eventNameRequired
                                },
                                stringLength: {
                                    min: 1,
                                    max: 100,
                                    message: localization.eventNameLength
                                }
                            }
                        },
                        calendar_event_start_date: {
                            validators: {
                                notEmpty: {
                                    message: localization.startDateRequired
                                },
                                callback: {
                                    message: localization.startDateInvalid,
                                    callback: function (input) {
                                        const isAllDay = document.getElementById('kt_calendar_datepicker_allday').checked;
                                        const date = new Date(input.value);
                                        const now = new Date();

                                        if (isAllDay) {
                                            const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
                                            const selectedDate = new Date(date.getFullYear(), date.getMonth(), date.getDate());
                                            console.log("-----Start AllDay-----")
                                            console.log(startDate.getTime());
                                            console.log(endDate.getTime());
                                            console.log("-----Start AllDay------")
                                            return selectedDate >= today;
                                        } else {
                                            const timeInput = document.getElementById('kt_calendar_datepicker_start_time');
                                            if (!timeInput) return false;

                                            const [hours, minutes] = timeInput.value.split(':').map(Number);
                                            date.setHours(hours, minutes, 0, 0);
                                            console.log("-----Start------")
                                            console.log(startDate.getTime());
                                            console.log(endDate.getTime());
                                            console.log("-----Start------")
                                            
                                            return date.getTime() >= now.getTime();
                                        }
                                    }
                                }
                            }
                        },
                        calendar_event_end_date: {
                            validators: {
                                notEmpty: {
                                    message: localization.endDateRequired
                                },
                                callback: {
                                    message: localization.endDateInvalid,
                                    callback: function (input) {
                                        const isAllDay = document.getElementById('kt_calendar_datepicker_allday').checked;
                                        const startDateInput = document.querySelector('[name="calendar_event_start_date"]');
                                        if (!startDateInput) return false;

                                        const startDate = new Date(startDateInput.value);
                                        const endDate = new Date(input.value);
                                        const now = new Date();

                                        if (isAllDay) {
                                            const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
                                            const selectedEnd = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());
                                            const selectedStart = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate());
                                            console.log("------End AllDay------")
                                            console.log(startDate.getTime());
                                            console.log(endDate.getTime());
                                            console.log(now.getTime());
                                            console.log("------End AllDay------")
                                            return selectedEnd >= today && selectedEnd >= selectedStart;
                                        } else {
                                            const startTimeInput = document.getElementById('kt_calendar_datepicker_start_time');
                                            const endTimeInput = document.getElementById('kt_calendar_datepicker_end_time');
                                            if (!startTimeInput || !endTimeInput) return false;

                                            const [startHours, startMinutes] = startTimeInput.value.split(':').map(Number);
                                            const [endHours, endMinutes] = endTimeInput.value.split(':').map(Number);

                                            startDate.setHours(startHours, startMinutes, 0, 0);
                                            endDate.setHours(endHours, endMinutes, 0, 0);
                                            console.log("------End------")
                                            console.log(startDate.getTime());
                                            console.log(endDate.getTime());
                                            console.log(now.getTime());
                                            console.log("------End------")
                                            return endDate.getTime() >= now.getTime() && endDate.getTime() > startDate.getTime();
                                        }
                                    }
                                }
                            }
                        }   
                    },
                    plugins: {
                        trigger: new FormValidation.plugins.Trigger(),
                        bootstrap: new FormValidation.plugins.Bootstrap5({
                            rowSelector: ".fv-row",
                            eleInvalidClass: "",
                            eleValidClass: "",
                        }),
                    },
                })),
                (r = flatpickr(o, { enableTime: !1, dateFormat: "Y-m-d" })),
                (l = flatpickr(i, { enableTime: !1, dateFormat: "Y-m-d" })),
                (c = flatpickr(d, {
                    enableTime: !0,
                    noCalendar: !0,
                    dateFormat: "H:i",
                })),
                (m = flatpickr(s, {
                    enableTime: !0,
                    noCalendar: !0,
                    dateFormat: "H:i",
                })),
                q(),
                y.addEventListener("click", (e) => {
                    (E = {
                        id: "",
                        eventName: "",
                        eventDescription: "",
                        startDate: new Date(),
                        endDate: new Date(),
                        allDay: !1,
                    }),
                        M();
                }),
                L.addEventListener("click", (t) => {
                    t.preventDefault(),
                        Swal.fire({
                            text: localization.deleteEvent,
                            icon: "warning",
                            showCancelButton: !0,
                            buttonsStyling: !1,
                            confirmButtonText: localization.confirmDeleteEventText,
                            cancelButtonText: localization.cancelDeleteEventText,
                            customClass: {
                                confirmButton: "btn btn-primary",
                                cancelButton: "btn btn-active-light",
                            },
                        }).then(function (t) {
                            if (t.value) {
                                $.ajax({
                                    url: `/Event/Delete/${E.id}`,
                                    method: 'POST',
                                    data: {
                                        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                                    },
                                    success: function (result) {
                                        if (result.success) {
                                            e.getEventById(E.id).remove();
                                            w.hide();
                                            Swal.fire({
                                                text: localization.eventDeletedSuccessfully,
                                                icon: "success",
                                                buttonsStyling: !1,
                                                confirmButtonText: localization.ok,
                                                customClass: { confirmButton: "btn btn-primary" },
                                            });
                                        } else {
                                            Swal.fire({
                                                text: result.message || localization.eventNotDelet,
                                                icon: "error",
                                                buttonsStyling: !1,
                                                confirmButtonText: localization.ok,
                                                customClass: { confirmButton: "btn btn-primary" },
                                            });
                                        }
                                    },
                                    error: function (xhr) {
                                        Swal.fire({
                                            text: localization.eventNotDelet,
                                            icon: "error",
                                            buttonsStyling: !1,
                                            confirmButtonText: localization.ok,
                                            customClass: { confirmButton: "btn btn-primary" },
                                        });
                                        console.error("Error deleting event:", xhr.responseText);
                                    }
                                });
                            } else if (t.dismiss === "cancel") {
                                Swal.fire({
                                    text: localization.eventNotDelet,
                                    icon: "error",
                                    buttonsStyling: !1,
                                    confirmButtonText: localization.ok,
                                    customClass: { confirmButton: "btn btn-primary" },
                                });
                            }
                        });
                }),
                k.addEventListener("click", function (e) {
                    e.preventDefault(),
                        Swal.fire({
                            text: localization.eventCancel,
                            icon: "warning",
                            showCancelButton: !0,
                            buttonsStyling: !1,
                            confirmButtonText: localization.yes,
                            cancelButtonText: localization.noRetuern,
                            customClass: {
                                confirmButton: "btn btn-primary",
                                cancelButton: "btn btn-active-light",
                            },
                        }).then(function (e) {
                            e.value
                                ? (f.reset(), u.hide())
                                : "cancel" === e.dismiss &&
                                Swal.fire({
                                    text: localization.eventNoCancel,
                                    icon: "error",
                                    buttonsStyling: !1,
                                    confirmButtonText: localization.ok,
                                    customClass: { confirmButton: "btn btn-primary" },
                                });
                        });
                }),
                _.addEventListener("click", function (e) {
                    e.preventDefault(),
                        Swal.fire({
                            text: localization.eventCancel,
                            icon: "warning",
                            showCancelButton: !0,
                            buttonsStyling: !1,
                            confirmButtonText: localization.yes,
                            cancelButtonText: localization.noRetuern,
                            customClass: {
                                confirmButton: "btn btn-primary",
                                cancelButton: "btn btn-active-light",
                            },
                        }).then(function (e) {
                            e.value
                                ? (f.reset(), u.hide())
                                : "cancel" === e.dismiss &&
                                Swal.fire({
                                    text: localization.eventNoCancel,
                                    icon: "error",
                                    buttonsStyling: !1,
                                    confirmButtonText: localization.ok,
                                    customClass: { confirmButton: "btn btn-primary" },
                                });
                        });
                }),
                ((e) => {
                    e.addEventListener("hidden.bs.modal", (e) => {
                        p && p.resetForm(!0);
                    });
                })(C);
        },
    };
})();
KTUtil.onDOMContentLoaded(function () {
    KTAppCalendar.init();
});


