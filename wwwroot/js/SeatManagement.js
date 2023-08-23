$(document).ready(function () {
    const seatGrid = $("#seat-grid");
    const seatTypeSelect = $("#seatType");
    var changedSeat = false;

    seatGrid.on("click", ".seat", function () {
        const selectedSeatType = seatTypeSelect.find("option:selected");
        const seat = $(this);

        if (seat.css("background-color") === "rgba(0, 0, 0, 0)") {
            seat.css("background-color", selectedSeatType.attr("data-color"));
            seat.attr("data-seatTypeId", selectedSeatType.val());
            changedSeat = true;
        } else {
            seat.css("background-color", "rgba(0, 0, 0, 0)");
            seat.attr("data-seatTypeId", "");
            seat.text("");
            changedSeat = true;
        }
    });

    $("#saveButton").click(function () {
        const selectedSeats = [];
        const emptySeatNames = [];
        seatGrid.find(".seat").each(function () {
            const seat = $(this);
            const seatId = seat.attr("data-seatTypeId");

            if (seatId) {
                const seatName = seat.text().trim();
                const seatData = {
                    row: seat.attr("data-row"),
                    col: seat.attr("data-col"),
                    typeId: seatId,
                    available: seat.data("available"),
                    seatName: seatName,
                    roomId: roomId
                };
                if (!seatName) {
                    emptySeatNames.push(seatData);
                } else {
                    selectedSeats.push(seatData);
                }
            }

            
        });

        if (changedSeat) {
            alert("You just modify some seats, you should re-name all the seats by click 'Assign Name'");
            return; // Don't proceed with submission
        }

        if (emptySeatNames.length > 0) {
            const warningMessage = `There are ${emptySeatNames.length} seats don't have name'`;
            alert(warningMessage);
            return; // Don't proceed with submission
        }

        //Post to backend
        if (selectedSeats.length > 0) {
            try {
                $.post({
                    url: "/SeatManagement",
                    dataType: 'json',
                    contentType: "application/json; charset=UTF-8",
                    data: JSON.stringify(selectedSeats),
                    headers: {
                        RequestVerificationToken:
                            $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    success: function (response) {
                        console.log(response);
                    },
                    error: function (error) {
                        console.log("Error:", error);
                    }
                });
            } catch (error) {
                // Handle error, e.g., show an error message
                console.log("Error saving selected seats:", error);
            }
        }
    });

    //Assign Name
    $("#assignName").click(function () {
        const selectedSeats = [];
        const selectedRows = new Set();
        const selectedColumns = new Set();
        changedSeat = false;

        // Loop through selected seats to collect rows and columns and store selected seat data
        seatGrid.find(".seat").each(function () {
            const seat = $(this);
            const seatId = seat.attr("data-seatTypeId");

            if (seatId) {
                const row = seat.attr("data-row");
                const col = seat.attr("data-col");
                selectedSeats.push({ seat, row, col });
                selectedRows.add(row);
                selectedColumns.add(col);
            }
        });

        var sortedRows;
        var sortedCols;

        if ($("#RowDirection1").is(":checked")) {
            sortedRows = Array.from(selectedRows).sort((a, b) => a - b);
        }
        else {
            sortedRows = Array.from(selectedRows).sort((a, b) => b - a);
        }
        
        if ($("#ColumnDirection1").is(":checked")) {
            sortedCols = Array.from(selectedColumns).sort((a, b) => a - b);
        }
        else {
            sortedCols = Array.from(selectedColumns).sort((a, b) => b - a);
        }

        const alphabet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';

        for (let row = 0; row < sortedRows.length; row++) {
            const rowName = alphabet[row];
            let colName = 1;
            for (let col = 0; col < sortedCols.length; col++) {
                const seatData = selectedSeats.find(s => s.row === sortedRows[row] && s.col === sortedCols[col]);
                if (seatData) {
                    seatData.seat.text(rowName + colName.toString());
                    colName++;
                }
            }
        }
    });
});
