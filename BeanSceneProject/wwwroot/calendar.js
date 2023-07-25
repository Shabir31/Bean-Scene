$(() => {
    let calendar = new FullCalendar.Calendar($('#calendar')[0], {
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'
        },
        timeZone: 'utc',
        /*timeZone: 'utc',*/
        navLinks: true, // can click day/week names to navigate views
        weekNumbers: true,
        events: "/Administration/Calendar/GetEvents",
        initialView: 'timeGridWeek',
        allDay: false,
        eventClick: function (info) {
            $('#myModal #eventTitle').text(info.event.title);
            debugger
            selectedEvent = info;
            var $description = $('<div/>');
            $description.append($('<p/>').html('<b> Start:</b>' + " " + info.event.start.toUTCString("DD-MMM-YYYY HH:mm a")));
            if (info.event.end != null) {
                $description.append($('<p/>').html('<b> End:</b>' + " " + info.event.end.toUTCString("DD-MMM-YYYY HH:mm a")));
            }
            $description.append($('<p/>').html('<b> Capacity:</b>' + "  " + info.event.extendedProps.capacity));
            $description.append($('<p/>').html('<b> Active:</b>' + "  " + info.event.extendedProps.active));
            $description.append($('<p/>').html('<b> Restaurant Name:</b>' + "  " + info.event.extendedProps.restaurantName));
            $description.append($('<p/>').html('<b> Sitting Type:</b>' + "  " + info.event.extendedProps.sittingTypeName));
            $description.append($('<p/>').html('<b> <a href="#" class="btn btn-primary" onclick="Edit(' + info.event.id + ')" >Edit</a>  <a href="#" class="btn btn-danger" onclick= "DeleteSitting(' + info.event.id + ')" >Delete</a> </b>'));
            //First we clear the modal by using .empty and than added the description init 
            $('#myModal #pDetails').empty().html($description);
            $('#myModal').modal("show");
        },
        selectable: true,
        select: function (info) {
            $('#popupSittingForm').modal("show");
            $('#start').val(info.start.toUTCString());
            $('#end').val(info.end.toUTCString());
            //if (info.start.isBefore(moment())) {
            //    $('#calendar').fullCalendar('unselect');
            //    return false;
            //}
            calendar.unselect();
        },

        //Here we are crating the event drop function so that we can drag and change the event date by keeping all the information same 
        editable: true,
        eventDrop: function (info) {
            debugger

            var objData = {
                Id: info.event.id,
                SittingName: info.event.title,
                StartTime: info.event.start.toUTCString(),
                EndTime: info.event.end != null ? info.event.end.toUTCString() : null,
                Capacity: info.event.extendedProps.capacity,
                Active: info.event.extendedProps.active,
                RestaurantId: info.event.extendedProps.restaurantId,
                SittingTypeId: info.event.extendedProps.sittingTypeId,
                selectable: false,
            }
            SaveEvent(objData);
        },

        eventResize: function (info) {
            debugger
            var objData = {
                Id: info.event.id,
                SittingName: info.event.title,
                StartTime: info.event.start.toUTCString(),
                EndTime: info.event.end != null ? info.event.end.toUTCString() : null,
                Capacity: info.event.extendedProps.capacity,
                Active: info.event.extendedProps.active,
                RestaurantId: info.event.extendedProps.restaurantId,
                SittingTypeId: info.event.extendedProps.sittingTypeId,
                selectable: false,

            }
            SaveEvent(objData);
        }
    });
    calendar.render();
})

function AddSitting() {
    debugger
    var startDate = new Date($('#start').val()).toUTCString();
    var endDate = new Date($('#end').val()).toUTCString();
    //Here we are storing all the data inside one object name objData
    debugger
    var objData = {
        SittingName: $('#title').val(),
        StartTime: startDate,
        EndTime: endDate,
        Capacity: $('#Capacity').val(),
        Active: $('#Active').val(),
        RestaurantId: $('#restaurant-id').val(),
        SittingTypeId: $('#sittingType-id').val(),

    }
    //console.log(objData);
    $.ajax({
        url: '/Administration/Calendar/CreateSitting',
        type: 'POST',
        data: objData,
        dataType: "json",
        success: function () {
            alert('Sitting is created successfully');
        },
        error: function () {
            alert('Sitting is not created ');
        },
    })
    $('#popupSittingForm').modal("hide");
}

function Edit(id) {
    $.ajax({
        url: '/Administration/Calendar/EditSitting?id=' + id,
        type: 'GET',
        dataType: "json",
        success: function (response) {
            $('#popupSittingForm').modal("show");
            //From here we are binding the data inside the input that is on modal 
            $('#SittindId').val(response.id);
            $('#title').val(response.sittingName);
            $('#start').val(response.startTime);
            $('#end').val(response.endTime);
            $('#Capacity').val(response.capacity);
            $('#Active').val(response.active);
            $('#restaurant-id').val(response.restaurantId);
            $('#sittingType-id').val(response.sittingTypeId);
            $('#btnAdd').hide();
            $('#btnUpdate').show();
            $('#myModal').modal("hide");
        },
        error: function () {
            alert('data is not found');
        }
    })
}

function UpdateSitting() {
    var objData = {
        Id: $('#SittindId').val(),
        SittingName: $('#title').val(),
        StartTime: $('#start').val(),
        EndTime: $('#end').val(),
        Capacity: $('#Capacity').val(),
        Active: $('#Active').val(),
        RestaurantId: $('#restaurant-id').val(),
        SittingTypeId: $('#sittingType-id').val(),
    }
    $.ajax({
        url: '/Administration/Calendar/UpdateSitting',
        type: 'POST',
        data: objData,
        dataType: "json",
        success: function () {
            alert('data is saved');
        },
        error: function () {
            alert('Data is not saved');
        },
    });
    $('#popupSittingForm').modal("hide");
}

function DeleteSitting(id) {
    if (confirm('Are you sure you want to delete this record')) {
        $.ajax({
            url: '/Administration/Calendar/DeleteSitting?id=' + id,
            type: 'POST',
            dataType: 'json',
            success: function () {
                alert('Record Deleted');
                $('#myModal').modal("hide");
            },
            error: function () {
                alert('Record cannot be deleted');
            }
        });
    }
    else {
        alert('all good');
    }
}


function SaveEvent(data) {
    $.ajax({
        url: '/Administration/Calendar/SaveEvent',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function () {
            alert('Record saved succesfully');
            $('#myModal').modal("hide");
        },
        error: function () {
            alert('Record cannot be deleted');
        }
    });
};
//$(function () {
//    $('#start, #end').datetimepicker();
//});
//$('#start').tempusDominus({
//  // options here

//});
const pick1 = new tempusDominus.TempusDominus(document.getElementById("start"), {
    localization: {
        dateFormats: {
            LTS: 'h:mm:ss T',
            LT: 'h:mm T',
            L: 'MM/dd/yyyy',
            LL: 'MMMM d, yyyy',
            LLL: 'MMMM d, yyyy h:mm T',
            LLLL: 'dddd, MMMM d, yyyy h:mm T'
        },
        ordinal: (n) => n,
        format: 'LLLL'
    },
    display: {
        icons: {
            time: "fas fa-clock",
            date: "fas fa-calendar",
            up: "fas fa-arrow-up",
            down: "fas fa-arrow-down",
            previous: "fas fa-chevron-left",
            next: "fas fa-chevron-right",
            today: "fas fa-calendar-check",
            clear: "fas fa-trash",
            close: "fas fa-xmark"
        },
        buttons: {
            today: true,
            clear: true,
            close: true,
        }
    }
});
const picker = new tempusDominus.TempusDominus(document.getElementById("end"), {
    localization: {
        dateFormats: {
            LTS: 'h:mm:ss T',
            LT: 'h:mm T',
            L: 'MM/dd/yyyy',
            LL: 'MMMM d, yyyy',
            LLL: 'MMMM d, yyyy h:mm T',
            LLLL: 'dddd, MMMM d, yyyy h:mm T' 
        },
        ordinal: (n) => n,
        format: 'LLLL'
    },
    display: {
        icons: {
            time: "fas fa-clock",
            date: "fas fa-calendar",
            up: "fas fa-arrow-up",
            down: "fas fa-arrow-down",
            previous: "fas fa-chevron-left",
            next: "fas fa-chevron-right",
            today: "fas fa-calendar-check",
            clear: "fas fa-trash",
            close: "fas fa-xmark"
        },
        buttons: {
            today: true,
            clear: true,
            close: true,
        }
    },
});


//picker.dates.formatInput = function (date) { { return moment(date).format('DD-MMM-YYYY h:mm   ') } }
//picker.dates.formatInput = function (date) { { return moment(date).format('DD-MMM-YYYY h:mm   ') } }
