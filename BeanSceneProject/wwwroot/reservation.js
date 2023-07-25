//var tableCounter = 1;
$(() => {
    droppable();
    //var droppedItem = $("#mytableId").clone().removeClass("ui-draggable tables");
    //droppedItem.addClass('tablesCss');
    //$("#mainId").append(droppedItem);
});

function GetTables() {
    const objData = {
        DiningAreaId: $("#table-id").val(),

    }
    $.ajax({
        url: '/Administration/InterReservation/GetRestaurantTable',
        type: 'GET',
        data: objData,
        success: function (response) {
            //console.log(response);
            $("#mainId").empty();
            let area = response.find(x => x.diningId == objData.DiningAreaId);
            for (let t of area.tables) {
                let el = $(`<ul>${t.name}</ul>`);
                //el = $("#mytableId").clone().removeClass("ui-draggable tables");
                el.addClass('tablesCss');
                $("#mainId").append(el);
                el.draggable();
                el.droppable({
                    accept: '#reservationId',
                    over: function (event, ui) {
                        $(this).addClass('highlight');
                    },
                    out: function (event, ui) {
                        $(this).removeClass('highlight');
                    },
                    drop: function (event, ui) {
                        $(this).append(ui.draggable.css({
                            position: 'relative',
                            left: '0px',
                            top: '0px'
                        }));
                        const reservationEl = ui.draggable;
                        const id = reservationEl.attr("reservation-id");
                        $.ajax({
                            url: '/Administration/InterReservation/EditStatus?id=' + id, 
                            type: 'POST',
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ "statusToSet": "SEATED" }),
                            success: function (response) {
                                debugger;
                                console.log(response);
                            }
                        });

                    },
                });
            }
        },
        error: function () {
            // Handle the error response from the server
            alert('Table cannot be added');
        }
    })
}


function AddTables(droppedItem) {
    var itemData = droppedItem.text();
    //console.log(itemData)
    var objData = {
        SeatName: itemData,
        DiningAreaId: $("#table-id").val(),
    };
    //Options 1 - Store vairable array from API
    //Option 2 - Call api whenever you change area and return the tables
    debugger
    $.ajax({

        url: '/Administration/InterReservation/AddRestaurantTable',
        type: 'POST',
        data: objData,
        success: function () {
            // Handle the success response from the server
            console.log('Table created ');
        },
        error: function () {
            // Handle the error response from the server
            alert('Table cannot be created');
        }
    });
}


function droppable() {
    var tableCounter = 1;
    $(".tables").draggable({
        helper: 'clone',
        cursor: 'pointer',
    });
    $("#source li").draggable({
        /* helper: 'clone',*/
        //revert: 'invalid'
        cursor: 'pointer',
    });
    $(".droppable").droppable({
        accept: ".tables",

        drop: function (event, ui) {
            //$(this).append($(".tables:first").clone().text("Table " + tableCounter));
            //var table = $("#table-id");
            $(droppedItem).empty();
            var droppedItem = $(ui.draggable).clone().css({
                position: 'absolute',
                left: ui.position.left,
                top: ui.position.top
            }).removeClass("ui-draggable tables").text("Table " + tableCounter);
            droppedItem.addClass('tablesCss');
            $(this).append(droppedItem);
            tableCounter++;
            AddTables(droppedItem);
            droppedItem.draggable();
            droppedItem.droppable({
                accept: '#reservationId',
                over: function (event, ui) {
                    $(this).addClass('highlight');
                },
                out: function (event, ui) {
                    $(this).removeClass('highlight');
                },
                drop: function (event, ui) {
                    $(droppedItem).append(ui.draggable.css({
                        position: 'relative',
                        left: '0px',
                        top: '0px'
                    }));

                },

            });
        },

    });


    $(".completeStatus").droppable({
        accept: '#reservationId',

        over: function (event, ui) {
            $(this).addClass('highlight');
        },
        out: function (event, ui) {
            $(this).removeClass('highlight');
        },
        drop: function (event, ui) {
            $(this).append(ui.draggable.css({
                position: 'relative',
                left: '0px',
                top: '0px'
            }));
            const reservationEl = ui.draggable;
            const id = reservationEl.attr("reservation-id");
            $.ajax({
                url: '/Administration/InterReservation/EditStatus?id=' + id,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "statusToSet": "COMPLETED" }),
                success: function (response) {
                    debugger;
                    console.log(response);
                }
            });
        }
    });
}

